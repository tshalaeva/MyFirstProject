using System.Collections.Generic;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    public interface IRepository
    {
        List<T> Get<T>() where T : IEntity;

        void Save<T>(T entity) where T : IEntity;

        void Delete<T>(T entity) where T : IEntity;

        void Initialize();
    }
}
