using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    class DbUserRepository : IRepository<User>
    {
        private AdoHelper adoHelper;

        public bool Initialized
        {
            get { throw new NotImplementedException(); }
        }

        public List<User> Get()
        {
            return adoHelper.GetUsers();
        }

        public void Save(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User existingEntity, User newEntity)
        {
            throw new NotImplementedException();
        }

        public User GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public User GetRandom()
        {
            throw new NotImplementedException();
        }
    }
}
