using System.Collections.Generic;
using MyFirstProject.Entity;

namespace MyFirstProject.Repository
{
    public interface IRepository<T>
    {
        List<T> Get();       

        void Save(T entity);       

        void Delete(T entity);

        void Initialize();
    }
}
