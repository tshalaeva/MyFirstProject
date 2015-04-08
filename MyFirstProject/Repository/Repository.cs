using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
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

        public List<User> Get()
        {
            return Data;
        }

        public int Save(User entity)
        {
            Data.Add(entity);
            return entity.Id;
        }

        public void Delete(User entity)
        {
            Data.Remove(entity);
        }

        public void Update(User existingEntity, User newEntity)
        {
            Data[Data.IndexOf(existingEntity)] = newEntity;
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

        public List<Article> Get()
        {
            return Data;
        }

        public int Save(Article entity)
        {
            Data.Add(entity);
            return entity.Id;
        }

        public void Delete(Article entity)
        {
            Data.Remove(entity);
        }

        public void Update(Article existingEntity, Article newEntity)
        {
            Data[Data.IndexOf(existingEntity)] = newEntity;
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

        public int Save(BaseComment entity)
        {
            Data.Add(entity);
            return entity.Id;
        }

        public void Delete(BaseComment entity)
        {
            Data.Remove(entity);
        }

        public void Update(BaseComment existingEntity, BaseComment newEntity)
        {
            Data[Data.IndexOf(existingEntity)] = newEntity;
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
