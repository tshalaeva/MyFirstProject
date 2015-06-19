using System;
using System.Collections.Generic;
using System.Linq;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IRepository<User>
    {
        protected List<User> Data;

        public UserRepository()
        {
            Data = new List<User>();
        }

        public bool Initialized
        {
            get
            {
                return Data.Count != 0;
            }
        }

        public int GetCount()
        {
            return Data.Count;
        }

        public List<User> Get()
        {
            return Data;
        }

        public List<User> Get(int id)
        {
        //
            return Data;
        }

        public List<User> GetSorted(string sortBy, int from, int count, string order)
        {
            throw new NotImplementedException();
        }

        public List<User> Get(int from, int count)
        {
            //
            return Data;
        }

        public int Save(User entity)
        {
            Data.Add(entity);
            return entity.Id;
        }

        public void Delete(int entityId)
        {
            Data.Remove(Data[entityId]);
        }

        public int Update(int existingEntityId, User newEntity)
        {
            Data[existingEntityId] = newEntity;
            return existingEntityId;
        }

        public User GetById(int? id)
        {
            var result = Data.First(entity => entity.Id == id);
            return result;
        }

        public User GetRandom()
        {
            var random = new Random();
            var entities = Get();
            return entities[random.Next(0, entities.Count)];
        }
    }

    public class ArticleRepository : IRepository<Article>
    {
        protected List<Article> Data;

        public ArticleRepository()
        {
            Data = new List<Article>();
        }

        public bool Initialized
        {
            get
            {
                return Data.Count != 0;
            }
        }

        public int GetCount()
        {
            return Data.Count;
        }

        public List<Article> Get()
        {
            return Data;
        }

        public List<Article> Get(int from, int count)
        {
            //
            return Data;
        }

        public List<Article> Get(int from)
        {
            //
            return Data;
        }

        public List<Article> GetSorted(string sortBy, int from, int count, string order)
        {
            throw new NotImplementedException();
        }

        public int Save(Article entity)
        {
            Data.Add(entity);
            return entity.Id;
        }

        public void Delete(int entityId)
        {
            Data.Remove(Data[entityId]);
        }

        public int Update(int existingEntityId, Article newEntity)
        {
            Data[existingEntityId] = newEntity;
            return existingEntityId;
        }

        public Article GetById(int? id)
        {
            var result = Data.First(entity => entity.Id == id);
            return result;
        }

        public Article GetRandom()
        {
            var random = new Random();
            var entities = Get();
            return entities[random.Next(0, entities.Count)];
        }
    }

    public class CommentRepository : IRepository<BaseComment>
    {
        protected List<BaseComment> Data;

        public CommentRepository()
        {
            Data = new List<BaseComment>();
        }

        public int GetCount()
        {
            return Data.Count;
        }

        public bool Initialized
        {
            get
            {
                return Data.Count != 0;
            }
        }

        public List<BaseComment> Get()
        {
            return Data;
        }

        public List<BaseComment> Get(int from, int count)
        {
            //
            return Get();
        }

        public List<BaseComment> Get(int from)
        {
            //
            return Get();
        }

        public List<BaseComment> GetSorted(string sortBy, int from, int count, string order)
        {
            throw new NotImplementedException();
        }

        public int Save(BaseComment entity)
        {
            Data.Add(entity);
            return entity.Id;
        }

        public void Delete(int entityId)
        {
            Data.Remove(Data[entityId]);
        }

        public int Update(int existingEntityId, BaseComment newEntity)
        {
            Data[existingEntityId] = newEntity;
            return existingEntityId;
        }

        public BaseComment GetById(int? id)
        {
            var result = Data.First(entity => entity.Id == id);
            return result;
        }

        public BaseComment GetRandom()
        {
            var random = new Random();
            var entities = Get();
            return entities[random.Next(0, entities.Count)];
        }
    }    
}
