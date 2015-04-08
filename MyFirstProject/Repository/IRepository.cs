using System.Collections.Generic;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        bool Initialized { get; }

        List<T> Get();

        int Save(T entity);

        void Delete(T entity);

        void Update(T existingEntity, T newEntity);

        T GetById(int? id);

        T GetRandom();
    }
}
