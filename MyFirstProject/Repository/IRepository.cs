using System.Collections.Generic;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    //public interface IRepository
    //{
    //    bool Initialized { get; }

    //    List<T> Get<T>() where T : IEntity;

    //    void Save<T>(T entity) where T : IEntity;

    //    void Delete<T>(T entity) where T : IEntity;

    //    void Update<T>(T existingEntity, T newEntity) where T : IEntity;

    //    T GetById<T>(int? id) where T : IEntity;

    //    T GetRandom<T>() where T : IEntity;
    //}

    public interface IRepository
    {
        bool Initialized { get; }

        void Save(IEntity entity);

        void Delete(IEntity entity);

        void Update(IEntity existingEntity, IEntity newEntity);        
        
        IEntity GetById(int? id);

        IEntity GetRandom();
    }
}
