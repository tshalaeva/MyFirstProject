using System;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
    public class DbReviewTextRepository : DbCommentRepository
    {
        private readonly AdoHelper m_reviewTextAdoHelper = new AdoHelper();

        public int SaveReviewText(ReviewText reviewText)
        {
            var command1 = string.Format(
                "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) OUTPUT Inserted.Id VALUES({0},{1},'{2}')", reviewText.User.Id, reviewText.Article.Id, reviewText.Content);
            var reviewTextId = m_reviewTextAdoHelper.CrudOperation(command1);
            var command2 = string.Format("INSERT INTO [dbo].[TextRating](ArticleId,Value,CommentId) VALUES({0},{1},{2}) ", reviewText.Article.Id, reviewText.Rating.Value, reviewTextId);
            m_reviewTextAdoHelper.CrudOperation(command2);
            return (int)reviewTextId;
        }

        public int UpdateReviewText(int oldReviewTextId, ReviewText reviewText)
        {
            var command1 = string.Format("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1} ",
                reviewText.Content, oldReviewTextId);
            var command2 = string.Format("UPDATE [dbo].[TextRating] SET Value={0} WHERE CommentId={1} ", reviewText.Rating.Value, oldReviewTextId);
            return Convert.ToInt32(m_reviewTextAdoHelper.CrudOperation(command1, command2));
        }

        public void DeleteReviewText(int id)
        {
            var command1 = string.Format("DELETE FROM [dbo].[Comments] WHERE Id={0} ", id);
            var command2 = string.Format("DELETE FROM [dbo].[TextRating] WHERE CommentId={0} ", id);
            m_reviewTextAdoHelper.CrudOperation(command1, command2);
        }
    }
}
