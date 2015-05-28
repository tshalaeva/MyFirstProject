using System;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
    public class DbReviewRepository : DbCommentRepository
    {
        private readonly AdoHelper m_reviewAdoHelper = new AdoHelper();

        public int SaveReview(Review review)
        {
            var command1 = string.Format("INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) OUTPUT Inserted.Id VALUES({0},{1},'{2}')", review.User.Id, review.Article.Id, review.Content);
            var commentId = m_reviewAdoHelper.CrudOperation(command1);
            var command2 = string.Format("INSERT INTO [dbo].[Rating](ArticleId,Value,CommentId) VALUES({0},{1},{2}) ", review.Article.Id, review.Rating.Value, commentId);
            m_reviewAdoHelper.CrudOperation(command2);
            return (int)commentId;
        }

        public int UpdateReview(int oldReviewId, Review newReview)
        {
            var command1 = string.Format("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1}", newReview.Content, oldReviewId);
            var command2 = string.Format("UPDATE [dbo].[Rating] SET Value={0} WHERE CommentId={1}", newReview.Rating.Value, oldReviewId);
            return Convert.ToInt32(m_reviewAdoHelper.CrudOperation(command1, command2));
        }

        public void DeleteReview(int id)
        {
            var command1 = string.Format("DELETE FROM [dbo].[Comments] WHERE Id={0}", id);
            var command2 = string.Format("DELETE FROM [dbo].[Rating] WHERE CommentId={0}", id);
            m_reviewAdoHelper.CrudOperation(command1, command2);
        }
    }
}
