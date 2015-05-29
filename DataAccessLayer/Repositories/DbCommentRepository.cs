using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;
using StructureMap;

namespace DataAccessLayer.Repositories
{
    public class DbCommentRepository : IRepository<BaseComment>
    {
        protected readonly AdoHelper AdoHelper;

        public readonly DtoMapper DtoMapper;

        private readonly DbReviewRepository m_reviewRepository;

        private readonly DbReviewTextRepository m_reviewTextRepository;

        public DbCommentRepository()
        {
        }

        [DefaultConstructor]
        public DbCommentRepository(DbReviewRepository reviewRepository, DbReviewTextRepository reviewTextRepository)
        {
            AdoHelper = new AdoHelper();
            DtoMapper = new DtoMapper();
            m_reviewRepository = reviewRepository;
            m_reviewTextRepository = reviewTextRepository;
        }

        public int GetCount()
        {
            return AdoHelper.GetCount("Comment");
        }

        public bool Initialized
        {
            get
            {
                if (AdoHelper.GetData("Comments").Rows.Count != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public int Update(int oldComment, BaseComment newComment)
        {
            if (!(newComment is Review))
            {
                return
                    Convert.ToInt32(AdoHelper.CrudOperation(
                        string.Format("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1}",
                            newComment.Content, oldComment)));
            }
            if (newComment is ReviewText)
            {
                return m_reviewTextRepository.UpdateReviewText(oldComment, (ReviewText)newComment);
            }
            return m_reviewRepository.UpdateReview(oldComment, (Review)newComment);
        }

        public int Save(BaseComment comment)
        {
            if (Exists(comment.Id)) return Update(comment.Id, comment);
            var cmdText = new StringBuilder();
            if (!(comment is Review))
            {
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) OUTPUT Inserted.Id VALUES('{0}','{1}','{2}') ",
                    comment.User.Id, comment.Article.Id, comment.Content.Trim());
                return (int)AdoHelper.CrudOperation(cmdText.ToString());
            }
            if (comment is ReviewText)
            {
                return m_reviewTextRepository.SaveReviewText((ReviewText)comment);
            }
            return m_reviewRepository.SaveReview((Review)comment);
        }

        public BaseComment GetRandom()
        {
            var random = new Random();
            var table = AdoHelper.GetData("Comments");
            var commentTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var ratingInt = AdoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            var ratingText = AdoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            var rating = ratingInt ?? ratingText;
            var dtoComment = new DtoComment
            {
                Id = (int)commentTable["Id"],
                Content = commentTable["Content"].ToString(),
                ArticleId = (int)commentTable["ArticleId"],
                UserId = (int)commentTable["UserId"],
                Rating = rating
            };
            return DtoMapper.GetComment(dtoComment);
        }

        public BaseComment GetById(int? id)
        {
            var commentData = AdoHelper.GetData("Comments", (int)id);
            var commentTable = commentData.Rows[0];
            var comment = new DtoComment
            {
                Id = (int)commentTable["Id"],
                Content = commentTable["Content"].ToString(),
                ArticleId = (int)commentTable["ArticleId"],
                UserId = (int)commentTable["UserId"],
            };
            var rating = AdoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            var textRating = AdoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            if ((rating == null) && (textRating == null)) return DtoMapper.GetComment(comment);
            if ((rating != null))
            {
                comment.Rating = rating;
            }
            comment.Rating = textRating;
            return DtoMapper.GetComment(comment);
        }

        public List<BaseComment> Get()
        {
            var comments = new List<BaseComment>();
            var commentsTable = AdoHelper.GetData("Comments");
            for (var i = 0; i < commentsTable.Rows.Count; i++)
            {
                var dtoComment = new DtoComment
                {
                    Id = (int)commentsTable.Rows[i]["Id"],
                    Content = commentsTable.Rows[i]["Content"].ToString(),
                    ArticleId = (int)commentsTable.Rows[i]["ArticleId"],
                    UserId = (int)commentsTable.Rows[i]["UserId"],
                    UserName = AdoHelper.GetCellValue("User", "FirstName", (int)commentsTable.Rows[i]["UserId"]).ToString(),
                    UserLastName = AdoHelper.GetCellValue("User", "LastName", (int)commentsTable.Rows[i]["UserId"]).ToString(),
                    UserAge = (int)AdoHelper.GetCellValue("User", "Age", (int)commentsTable.Rows[i]["UserId"])
                };
                var rating = AdoHelper.GetCellValue("Rating", "Value", "CommentId", commentsTable.Rows[i]["Id"]);
                var textRating = AdoHelper.GetCellValue("TextRating", "Value", "CommentId", commentsTable.Rows[i]["Id"]);
                if (rating != null || textRating != null)
                {
                    dtoComment.Rating = rating ?? textRating;
                }
                comments.Add(DtoMapper.GetComment(dtoComment));
            }
            return comments;
        }

        public List<BaseComment> Get(int from, int count)
        {
            //
            return Get();
        }

        public void Delete(int commentId)
        {
            var entity = GetById(commentId);
            if (!(entity is Review))
            {
                var cmdText = string.Format("DELETE FROM [dbo].[Comments] WHERE Id={0}", commentId);
                AdoHelper.CrudOperation(cmdText);
            }
            else
            {
                if (entity is ReviewText)
                {
                    m_reviewTextRepository.Delete(commentId);
                    return;
                }
                m_reviewRepository.Delete(commentId);
            }
        }

        private bool Exists(int id)
        {
            var comment = AdoHelper.GetCellValue("Comments", "Id", id);
            return comment != null;
        }
    }
}
