using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    class DbRepository : IRepository
    {
        private AdoHelper adoHelper;


        public bool Initialized
        {
            get { throw new NotImplementedException(); }
        }

        public List<T> Get<T>() where T : IEntity
        {
            throw new NotImplementedException();
        }

        public void Save<T>(T entity) where T : IEntity
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : IEntity
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T existingEntity, T newEntity) where T : IEntity
        {
            throw new NotImplementedException();
        }

        public T GetById<T>(int? id) where T : IEntity
        {
            throw new NotImplementedException();
        }

        public T GetRandom<T>() where T : IEntity
        {
            throw new NotImplementedException();
        }
    }
}
