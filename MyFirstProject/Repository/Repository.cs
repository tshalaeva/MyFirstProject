using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    public class Repository : IRepository
    {
        protected List<IEntity> Data;

        public Repository()
        {
            Data = new List<IEntity>();
        }

        public virtual bool Initialized
        {
            get
            {
                return Data.Count != 0;
            }
        }

        public virtual List<T> Get<T>() where T : IEntity
        {
            return Data.OfType<T>().ToList();
        }

        public virtual void Save<T>(T entity) where T : IEntity
        {
            Data.Add(entity);
        }

        public virtual void Delete<T>(T entity) where T : IEntity
        {
            Data.Remove(entity);
        }

        public virtual void Update<T>(T existingEntity, T newEntity) where T : IEntity
        {
            Data[Data.IndexOf(existingEntity)] = newEntity;
        }

        public virtual T GetById<T>(int? id) where T : IEntity
        {
            var result = Data.First(entity => entity.Id == id && entity is T);
            return (T)result;
        }

        public virtual T GetRandom<T>() where T : IEntity
        {
            var random = new Random();
            var entities = Get<T>();
            return entities[random.Next(0, entities.Count)];
        }
    }    
}
