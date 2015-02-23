using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    public class Repository : IRepository
    {
        protected List<IEntity> m_data;

        public virtual bool Initialized
        {
            get
            {
                return m_data.Count != 0;
            }
        }

        public Repository()
        {
            m_data = new List<IEntity>();
        }

        public virtual List<T> Get<T>() where T : IEntity
        {
            return m_data.OfType<T>().ToList();
        }

        public virtual void Save<T>(T entity) where T : IEntity
        {
            m_data.Add(entity);
        }

        public virtual void Delete<T>(T entity) where T : IEntity
        {
            m_data.Remove(entity);
        }

        public virtual void Update<T>(T existingEntity, T newEntity) where T : IEntity
        {
            m_data[m_data.IndexOf(existingEntity)] = newEntity;
        }

        public T GetById<T>(int id) where T : IEntity
        {
            var result = (from entity in m_data
                where entity.Id == id && entity is T
                select entity);
            return (T)result.First();
        }
    }
}
