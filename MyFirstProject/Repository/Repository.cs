using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    //public class Repository : IRepository
    //{
    //    protected List<IEntity> Data;

    //    public Repository()
    //    {
    //        Data = new List<IEntity>();
    //    }

    //    public virtual bool Initialized
    //    {
    //        get
    //        {
    //            return Data.Count != 0;
    //        }
    //    }

    //    public virtual List<T> Get<T>() where T : IEntity
    //    {
    //        return Data.OfType<T>().ToList();
    //    }

    //    public virtual void Save<T>(T entity) where T : IEntity
    //    {
    //        Data.Add(entity);
    //    }

    //    public virtual void Delete<T>(T entity) where T : IEntity
    //    {
    //        Data.Remove(entity);
    //    }

    //    public virtual void Update<T>(T existingEntity, T newEntity) where T : IEntity
    //    {
    //        Data[Data.IndexOf(existingEntity)] = newEntity;
    //    }

    //    public virtual T GetById<T>(int? id) where T : IEntity
    //    {
    //        var result = Data.First(entity => entity.Id == id && entity is T);
    //        return (T)result;
    //    }

    //    public virtual T GetRandom<T>() where T : IEntity
    //    {
    //        var random = new Random();
    //        var entities = Get<T>();
    //        return entities[random.Next(0, entities.Count)];
    //    }
    //}

    public class Repository : IRepository
    {
        public ArticleRepository ArticleRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public CommentRepository CommentRepository { get; set; }

        public Repository()
        {
            ArticleRepository = new ArticleRepository();
            UserRepository = new UserRepository();
            CommentRepository = new CommentRepository();
        }

        public bool Initialized
        {
            get
            {
                return (ArticleRepository.Initialized) || (UserRepository.Initialized) || (CommentRepository.Initialized);
            }
        }

        public virtual void Save(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(IEntity existingEntity, IEntity newEntity)
        {
            throw new NotImplementedException();
        }

        public IEntity GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public IEntity GetRandom()
        {
            throw new NotImplementedException();
        }
    }

    public class ArticleRepository : IRepository
    {
        private readonly List<Article> m_articles;

        public ArticleRepository()
        {
            m_articles = new List<Article>();
        }

        public bool Initialized
        {
            get
            {
                return m_articles.Count != 0;
            }
        }

        public List<Article> GetArticles()
        {
            return m_articles;
        }

        public void Save(IEntity entity)
        {
            m_articles.Add((Article)entity);
        }

        public void Delete(IEntity entity)
        {
            m_articles.Remove((Article)entity);
        }

        public void Update(IEntity existingEntity, IEntity newEntity)
        {
            m_articles[m_articles.IndexOf((Article)GetById(existingEntity.Id))] = (Article)newEntity;
        }

        public IEntity GetById(int? id)
        {
            var result = m_articles.First(entity => entity.Id == id);
            return result;
        }

        public IEntity GetRandom()
        {
            var random = new Random();
            var entities = GetArticles();
            return entities[random.Next(0, entities.Count)];
        }
    }

    public class UserRepository : IRepository
    {
        private readonly List<User> m_users;

        public UserRepository()
        {
            m_users = new List<User>();
        }

        public bool Initialized
        {
            get
            {
                return m_users.Count != 0;
            }
        }

        public List<User> GetAllUsers()
        {
            return m_users;
        }

        public List<Admin> GetAdmins()
        {
            return m_users.OfType<Admin>().ToList();
        }

        public List<Author> GetAuthors()
        {
            return m_users.OfType<Author>().ToList();
        }

        public void Save(IEntity entity)
        {
            m_users.Add((User)entity);
        }

        public void Delete(IEntity entity)
        {
            m_users.Remove((User)entity);
        }

        public void Update(IEntity existingEntity, IEntity newEntity)
        {
            m_users[m_users.IndexOf((User)existingEntity)] = (User)newEntity;
        }

        public IEntity GetById(int? id)
        {
            var result = m_users.First(entity => entity.Id == id);
            return result;
        }

        public IEntity GetRandom()
        {
            var random = new Random();
            var entities = GetAllUsers();
            return entities[random.Next(0, entities.Count)];
        }
    }    

    public class CommentRepository : IRepository
    {
        private readonly List<BaseComment> m_comments;

        public CommentRepository()
        {
            m_comments = new List<BaseComment>();
        }

        public bool Initialized
        {
            get
            {
                return m_comments.Count != 0;
            }
        }

        public List<BaseComment> GetComments()
        {
            return m_comments;
        }

        public void Save(IEntity entity)
        {
            m_comments.Add((BaseComment)entity);
        }

        public void Delete(IEntity entity)
        {
            m_comments.Remove((BaseComment)entity);
        }

        public void Update(IEntity existingEntity, IEntity newEntity)
        {
            m_comments[m_comments.IndexOf((BaseComment)existingEntity)] = (BaseComment)newEntity;
        }

        public IEntity GetById(int? id)
        {
            var result = m_comments.First(entity => entity.Id == id);
            return result;
        }

        public IEntity GetRandom()
        {
            var random = new Random();
            var entities = GetComments();
            return entities[random.Next(0, entities.Count)];
        }
    }
}
