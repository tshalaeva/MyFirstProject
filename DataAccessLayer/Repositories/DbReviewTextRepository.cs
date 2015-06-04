using System;
using System.Collections.Generic;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
    public class DbReviewTextRepository : DbCommentRepository, IRepository<ReviewText>
    {
        private readonly AdoHelper m_reviewTextAdoHelper = new AdoHelper();

        public void DeleteReviewText(int id)
        {
            var command1 = string.Format("DELETE FROM [dbo].[Comments] WHERE Id={0} ", id);
            var command2 = string.Format("DELETE FROM [dbo].[TextRating] WHERE CommentId={0} ", id);
            m_reviewTextAdoHelper.CrudOperation(command1, command2);
        }

        public new List<ReviewText> Get()
        {
            var reviewTexts = new List<ReviewText>();
            var commentsTable = m_reviewTextAdoHelper.GetData("Comments");
            for (var i = 0; i < commentsTable.Rows.Count; i++)
            {
                var textRating = m_reviewTextAdoHelper.GetCellValue("TextRating", "Value", "CommentId", commentsTable.Rows[i]["Id"]);
                if (textRating != null)
                {
                    var dtoReviewText = new DtoComment
                    {
                        Id = (int)commentsTable.Rows[i]["Id"],
                        Content = commentsTable.Rows[i]["Content"].ToString(),
                        ArticleId = (int)commentsTable.Rows[i]["ArticleId"],
                        UserId = (int)commentsTable.Rows[i]["UserId"],
                        UserName =
                            m_reviewTextAdoHelper.GetCellValue("User", "FirstName", (int)commentsTable.Rows[i]["UserId"])
                                .ToString(),
                        UserLastName =
                            m_reviewTextAdoHelper.GetCellValue("User", "LastName", (int)commentsTable.Rows[i]["UserId"]).ToString(),
                        UserAge = (int)m_reviewTextAdoHelper.GetCellValue("User", "Age", (int)commentsTable.Rows[i]["UserId"]),
                        Rating = textRating
                    };
                    reviewTexts.Add((ReviewText)DtoMapper.GetComment(dtoReviewText));
                }
            }
            return reviewTexts;
        }

        public new List<ReviewText> Get(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public new List<ReviewText> Get(int filteredById)
        {
            throw new NotImplementedException();
        }

        public int Save(ReviewText reviewText)
        {
            var command1 = string.Format(
                "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) OUTPUT Inserted.Id VALUES({0},{1},'{2}')", reviewText.User.Id, reviewText.Article.Id, reviewText.Content);
            var reviewTextId = m_reviewTextAdoHelper.CrudOperation(command1);
            var command2 = string.Format("INSERT INTO [dbo].[TextRating](ArticleId,Value,CommentId) VALUES({0},{1},{2}) ", reviewText.Article.Id, reviewText.Rating.Value, reviewTextId);
            m_reviewTextAdoHelper.CrudOperation(command2);
            return (int)reviewTextId;
        }

        public int Update(int oldReviewTextId, ReviewText reviewText)
        {
            var command1 = string.Format("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1} ",
                reviewText.Content, oldReviewTextId);
            var command2 = string.Format("UPDATE [dbo].[TextRating] SET Value={0} WHERE CommentId={1} ", reviewText.Rating.Value, oldReviewTextId);
            return Convert.ToInt32(m_reviewTextAdoHelper.CrudOperation(command1, command2));
        }

        public new ReviewText GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public new ReviewText GetRandom()
        {
            throw new NotImplementedException();
        }
    }
}
