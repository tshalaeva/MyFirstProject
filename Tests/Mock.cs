using System.Linq;
using MyFirstProject.Entities;
using System.Collections.Generic;
using MyFirstProject.Repository;

namespace Tests
{
    public class Mock : Repository
    {
        private bool m_flag = false;
        private IEntity m_entity;

        public Mock()
        {
            m_data = new List<IEntity>() { new Article(0), new Article(1), new Article(2), new Article(2), new User(0), new User(1) };
            Save(new Comment(0, "Content 0", Get<User>().First(), Get<Article>().First()));
            Save(new Comment(1, "Content 1", Get<User>()[1], Get<Article>()[0]));
            Save(new Review(0, "Review 0", Get<User>().First(), Get<Article>().First(), new Rating(5)));
            Save(new ReviewText(0, "Review Text 0", Get<User>().First(), Get<Article>().First(), new Rating(1)));
        }

        public override sealed void Save<T>(T entity)
        {
            base.Save(entity);
            m_flag = true;
            m_entity = entity;
        }

        public override sealed void Update<T>(T oldEntity, T newEntity)
        {
            base.Update(oldEntity, newEntity);
            m_entity = newEntity;
            m_flag = true;
        }

        public override sealed void Delete<T>(T entity)
        {
            base.Delete(entity);
            m_entity = entity;
            m_flag = true;
        }

        public bool MethodIsCalled<T>(T entity) where T : IEntity
        {
            if (m_flag && m_entity.Id == entity.Id)
            {
                m_flag = false;
                return true;
            }
            return false;
        }
    }
}
