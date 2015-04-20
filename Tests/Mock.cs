using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MyFirstProject.Entities;
using MyFirstProject.Repository;


namespace Tests
{
    //public class ArticleMock : IRepository<Article>
    //{
    //    private bool m_flag;
    //    private IEntity m_entity;
    //    private List<IEntity> Data;

    //    public bool Initialized { get { if (Data.Count != 0) { return true; } else { return false; } } }          

    //    public ArticleMock(List<Article> articles, List<Comment> comments, List<Review> reviews, List<ReviewText> reviewTexts, List<Author> authors, List<Admin> admins, List<User> users)
    //    {
    //        Data = new List<IEntity>(articles.Count + comments.Count + reviews.Count + reviewTexts.Count + admins.Count + authors.Count + users.Count);
    //        Data.AddRange(articles);
    //        Data.AddRange(comments);
    //        Data.AddRange(reviews);
    //        Data.AddRange(reviewTexts);
    //        Data.AddRange(admins);
    //        Data.AddRange(authors);
    //        Data.AddRange(users);

    //    }

    //    public IEntity GetById(int? id)
    //    {
    //        foreach (var item in Data)
    //        {
    //            if (item.Id == id)
    //            {
    //                return item;
    //            }
    //        }
    //        return null;
    //    }

    //    public IEntity GetRandom()
    //    {
    //        var random = new Random();            
    //        return Data[random.Next(0, Data.Count)];
    //    }

    //    public List<IEntity> Get()
    //    {
    //        return Data;
    //    }

    //    public void Save(IEntity entity)
    //    {
    //        //base.Save(entity);
    //        m_flag = true;
    //        m_entity = (IEntity)entity;
    //    }

    //    public void Update(IEntity oldEntity, IEntity newEntity)
    //    {
    //        //base.Update(oldEntity, newEntity);
    //        m_entity = (IEntity)newEntity;
    //        m_flag = true;
    //    }

    //    public void Delete(IEntity entity)
    //    {
    //        //base.Delete(entity);
    //        m_entity = (IEntity)entity;
    //        m_flag = true;
    //    }

    //    public bool MethodIsCalled(int id)
    //    {
    //        if (m_flag && m_entity.Id == id)
    //        {
    //            m_flag = false;
    //            return true;
    //        }

    //        return false;
    //    }
    //}

    public class ArticleMock : IRepository<Article>
    {
        private bool m_flag;
        private Article m_entity;
        private List<Article> Data;

        public bool Initialized { get { if (Data.Count != 0) { return true; } else { return false; } } }

        public ArticleMock(List<Article> articles)
        {
            Data = articles;
        }

        public Article GetById(int? id)
        {
            foreach (var item in Data)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public Article GetRandom()
        {
            var random = new Random();
            return Data[random.Next(0, Data.Count)];
        }

        public List<Article> Get()
        {
            return Data;
        }

        public int Save(Article entity)
        {
            Data.Add(entity);
            m_flag = true;
            m_entity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, Article newEntity)
        {
            Data[oldEntity] = newEntity;
            m_entity = newEntity;
            m_flag = true;
            return oldEntity;
        }

        public void Delete(Article entity)
        {
            Data.Remove(entity);
            m_entity = entity;
            m_flag = true;
        }

        public bool MethodIsCalled(int id)
        {
            if (m_flag && m_entity.Id == id)
            {
                m_flag = false;
                return true;
            }
            return false;
        }
    }

    public class UserMock : IRepository<User>
    {
        private bool m_flag;
        private User m_entity;
        private List<User> Data;

        public bool Initialized { get { if (Data.Count != 0) { return true; } else { return false; } } }

        public UserMock(List<User> users)
        {
            Data = users;
        }

        public User GetById(int? id)
        {
            foreach (var item in Data)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public User GetRandom()
        {
            var random = new Random();
            return Data[random.Next(0, Data.Count)];
        }

        public List<User> Get()
        {
            return Data;
        }

        public int Save(User entity)
        {
            Data.Add(entity);
            m_flag = true;
            m_entity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, User newEntity)
        {
            Data[oldEntity] = newEntity;
            m_entity = newEntity;
            m_flag = true;
            return oldEntity;
        }

        public void Delete(User entity)
        {
            Data.Remove(entity);
            m_entity = entity;
            m_flag = true;
        }

        public bool MethodIsCalled(int id)
        {
            if (m_flag && m_entity.Id == id)
            {
                m_flag = false;
                return true;
            }
            return false;
        }
    }

    public class CommentMock : IRepository<BaseComment>
    {
        private bool m_flag;
        private BaseComment m_entity;
        private List<BaseComment> Data;

        public bool Initialized { get { if (Data.Count != 0) { return true; } else { return false; } } }

        public CommentMock(List<BaseComment> users)
        {
            Data = users;
        }

        public BaseComment GetById(int? id)
        {
            foreach (var item in Data)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public BaseComment GetRandom()
        {
            var random = new Random();
            return Data[random.Next(0, Data.Count)];
        }

        public List<BaseComment> Get()
        {
            return Data;
        }

        public int Save(BaseComment entity)
        {
            Data.Add(entity);
            m_flag = true;
            m_entity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, BaseComment newEntity)
        {
            Data[oldEntity] = newEntity;
            m_entity = newEntity;
            m_flag = true;
            return oldEntity;
        }

        public void Delete(BaseComment entity)
        {
            Data.Remove(entity);
            m_entity = entity;
            m_flag = true;
        }

        public bool MethodIsCalled(int id)
        {
            if (m_flag && m_entity.Id == id)
            {
                m_flag = false;
                return true;
            }
            return false;
        }
    }
}
