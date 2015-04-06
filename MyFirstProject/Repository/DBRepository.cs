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
        private readonly AdoHelper _adoHelper;

        public DbUserRepository()
        {
            _adoHelper = new AdoHelper();
        }

        public bool Initialized
        {
            get
            {
                if (_adoHelper.GetUsersCount() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<User> Get()
        {
            return _adoHelper.GetUsers();
        }

        public void Save(User entity)
        {
            if (!(entity is Admin))
            {
                _adoHelper.SaveUser(entity);
            }
            else
            {
                _adoHelper.SaveAdmin((Admin)entity);
            }
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
