using System.Collections.Generic;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        bool Initialized { get; }

        List<T> Get();

        List<T> Get(int from, int count);

        List<T> Get(int filteredById);

        int Save(T entity);
        
        void Delete(int id);

        int Update(int existingEntityId, T newEntity);

        T GetById(int? id);

        T GetRandom();

        int GetCount();
    }
}
