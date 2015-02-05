using System.Collections.Generic;
using MyFirstProject.Entity;

namespace MyFirstProject.Repository
{
    public interface IRepository <T> : IBaseRepository
    {
        void Save(T entity);       

        void Delete(T entity);     
    }
}
