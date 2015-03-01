using System.Collections.Generic;
using MyFirstProject.Entities;
using MyFirstProject.Repository;

namespace Tests
{
    public class Mock : Repository
    {
        private bool m_flag;
        private IEntity m_entity;

        public Mock(List<Article> articles, List<Comment> comments, List<Review> reviews, List<ReviewText> reviewTexts, List<Author> authors, List<Admin> admins, List<User> users)
        {
            Data = new List<IEntity>(articles.Count + comments.Count + reviews.Count + reviewTexts.Count + admins.Count + authors.Count + users.Count);
            Data.AddRange(articles);
            Data.AddRange(comments);
            Data.AddRange(reviews);
            Data.AddRange(reviewTexts);
            Data.AddRange(admins);
            Data.AddRange(authors);
            Data.AddRange(users);
        }

        public override void Save<T>(T entity)
        {
            base.Save(entity);
            m_flag = true;
            m_entity = entity;
        }

        public override void Update<T>(T oldEntity, T newEntity)
        {
            base.Update(oldEntity, newEntity);
            m_entity = newEntity;
            m_flag = true;
        }

        public override void Delete<T>(T entity)
        {
            base.Delete(entity);
            m_entity = entity;
            m_flag = true;
        }

        public bool MethodIsCalled(int id)
        {
            if (m_flag && m_entity.Id == id)
            {
                m_flag = false;
                return true;
            }

            return false;
        }
    }
}
