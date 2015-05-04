using System.Collections.Generic;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        bool Initialized { get; }

        List<T> Get();

        int Save(T entity);

        //void Delete(T entity);
        void Delete(int id);

        int Update(int existingEntityId, T newEntity);

        T GetById(int? id);

        T GetRandom();
    }
}
