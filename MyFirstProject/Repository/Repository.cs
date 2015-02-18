using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    public class Repository : IRepository
    {
        protected List<IEntity> m_data;

        public bool Initialized
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

        public List<T> Get<T>() where T : IEntity
        {
            return m_data.OfType<T>().ToList();
        }

        public void Save<T>(T entity) where T : IEntity
        {
            m_data.Add(entity);
        }

        public void Delete<T>(T entity) where T : IEntity
        {
            m_data.Remove(entity);
        }

        public void Update<T>(T existingEntity, T newEntity) where T : IEntity
        {
            m_data[m_data.IndexOf(existingEntity)] = newEntity;
        }
    }
}
