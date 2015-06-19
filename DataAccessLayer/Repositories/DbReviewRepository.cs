using System;
using System.Collections.Generic;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
    public class DbReviewRepository : DbCommentRepository, IRepository<Review>
    {
        private readonly AdoHelper m_reviewAdoHelper = new AdoHelper();

        public void DeleteReview(int id)
        {
            var command1 = string.Format("DELETE FROM [dbo].[Comments] WHERE Id={0}", id);
            var command2 = string.Format("DELETE FROM [dbo].[Rating] WHERE CommentId={0}", id);
            m_reviewAdoHelper.CrudOperation(command1, command2);
        }

        public new List<Review> Get()
        {
            var reviews = new List<Review>();
            var commentsTable = m_reviewAdoHelper.GetData("Comments");
            for (var i = 0; i < commentsTable.Rows.Count; i++)
            {
                var rating = m_reviewAdoHelper.GetCellValue("Rating", "Value", "CommentId", commentsTable.Rows[i]["Id"]);
                if (rating != null)
                {
                    var dtoReview = new DtoComment()
                    {
                        Id = (int)commentsTable.Rows[i]["Id"],
                        Content = commentsTable.Rows[i]["Content"].ToString(),
                        ArticleId = (int)commentsTable.Rows[i]["ArticleId"],
                        UserId = (int)commentsTable.Rows[i]["UserId"],
                        UserName =
                            AdoHelper.GetCellValue("User", "FirstName", (int)commentsTable.Rows[i]["UserId"])
                                .ToString(),
                        UserLastName =
                            AdoHelper.GetCellValue("User", "LastName", (int)commentsTable.Rows[i]["UserId"]).ToString()
                        ,
                        UserAge = (int)AdoHelper.GetCellValue("User", "Age", (int)commentsTable.Rows[i]["UserId"]),
                        Rating = rating
                    };
                    reviews.Add((Review)DtoMapper.GetComment(dtoReview));
                }
            }
            return reviews;
        }

        public new List<Review> Get(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public new List<Review> Get(int filteredById)
        {
            throw new NotImplementedException();
        }

        public new List<Review> GetSorted(string sortBy, int from, int count, string order)
        {
            throw new NotImplementedException();
        }

        public int Save(Review review)
        {
            var command1 = string.Format("INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) OUTPUT Inserted.Id VALUES({0},{1},'{2}')", review.User.Id, review.Article.Id, review.Content);
            var commentId = m_reviewAdoHelper.CrudOperation(command1);
            var command2 = string.Format("INSERT INTO [dbo].[Rating](ArticleId,Value,CommentId) VALUES({0},{1},{2}) ", review.Article.Id, review.Rating.Value, commentId);
            m_reviewAdoHelper.CrudOperation(command2);
            return (int)commentId;
        }

        public int Update(int oldReviewId, Review newReview)
        {
            var command1 = string.Format("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1}", newReview.Content, oldReviewId);
            var command2 = string.Format("UPDATE [dbo].[Rating] SET Value={0} WHERE CommentId={1}", newReview.Rating.Value, oldReviewId);
            return Convert.ToInt32(m_reviewAdoHelper.CrudOperation(command1, command2));
        }

        public new Review GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public new Review GetRandom()
        {
            throw new NotImplementedException();
        }
    }
}
