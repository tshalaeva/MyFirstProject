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
            //Data = new List<IEntity>(articles.Count + comments.Count + reviews.Count + reviewTexts.Count + admins.Count + authors.Count + users.Count);
            //Data.AddRange(articles);
            //Data.AddRange(comments);
            //Data.AddRange(reviews);
            //Data.AddRange(reviewTexts);
            //Data.AddRange(admins);
            //Data.AddRange(authors);
            //Data.AddRange(users);
            foreach (var article in articles)
            {
                ArticleRepository.Save(article);
            }

            var baseComments = new List<BaseComment>(comments.Count + reviews.Count + reviewTexts.Count);
            baseComments.AddRange(comments);
            baseComments.AddRange(reviews);
            baseComments.AddRange(reviewTexts);
            foreach (var comment in baseComments)
            {
                CommentRepository.Save(comment);
            }

            var userList = new List<User>(authors.Count + admins.Count + users.Count);
            userList.AddRange(authors);
            userList.AddRange(admins);
            userList.AddRange(users);
            foreach (var user in userList)
            {
                UserRepository.Save(user);
            }
        }

        public override void Save(IEntity entity)
        {
            //ArticleRepository.Save(entity);
            m_flag = true;
            m_entity = entity;
        }

        //public void SaveUser(User entity)
        //{
        //    UserRepository.Save(entity);
        //    m_flag = true;
        //    m_entity = entity;
        //}

        //public void SaveComment(BaseComment entity)
        //{
        //    CommentRepository.Save(entity);
        //    m_flag = true;
        //    m_entity = entity;
        //}

        public override void Update(IEntity oldEntity, IEntity newEntity)
        {
            //ArticleRepository.Update(oldEntity, newEntity);
            m_entity = newEntity;
            m_flag = true;
        }

        public override void Delete(IEntity entity)
        {
            //ArticleRepository.Delete(entity);
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
