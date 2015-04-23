using System;
using System.Collections.Generic;
using DataAccessLayer.Repositories;
using ObjectRepository.Entities;

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
        private bool _mFlag;
        private Article _mEntity;
        private readonly List<Article> _data;

        public bool Initialized { get { return _data.Count != 0; }
        }

        public ArticleMock(List<Article> articles)
        {
            _data = articles;
        }

        public Article GetById(int? id)
        {
            foreach (var item in _data)
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
            return _data[random.Next(0, _data.Count)];
        }

        public List<Article> Get()
        {
            return _data;
        }

        public int Save(Article entity)
        {
            _data.Add(entity);
            _mFlag = true;
            _mEntity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, Article newEntity)
        {
            _data[oldEntity] = newEntity;
            _mEntity = newEntity;
            _mFlag = true;
            return oldEntity;
        }

        public void Delete(Article entity)
        {
            _data.Remove(entity);
            _mEntity = entity;
            _mFlag = true;
        }

        public bool MethodIsCalled(int id)
        {
            if (_mFlag && _mEntity.Id == id)
            {
                _mFlag = false;
                return true;
            }
            return false;
        }
    }

    public class UserMock : IRepository<User>
    {
        private bool _mFlag;
        private User _mEntity;
        private readonly List<User> _data;

        public bool Initialized { get { return _data.Count != 0; }
        }

        public UserMock(List<User> users)
        {
            _data = users;
        }

        public User GetById(int? id)
        {
            foreach (var item in _data)
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
            return _data[random.Next(0, _data.Count)];
        }

        public List<User> Get()
        {
            return _data;
        }

        public int Save(User entity)
        {
            _data.Add(entity);
            _mFlag = true;
            _mEntity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, User newEntity)
        {
            _data[oldEntity] = newEntity;
            _mEntity = newEntity;
            _mFlag = true;
            return oldEntity;
        }

        public void Delete(User entity)
        {
            _data.Remove(entity);
            _mEntity = entity;
            _mFlag = true;
        }

        public bool MethodIsCalled(int id)
        {
            if (_mFlag && _mEntity.Id == id)
            {
                _mFlag = false;
                return true;
            }
            return false;
        }
    }

    public class CommentMock : IRepository<BaseComment>
    {
        private bool _mFlag;
        private BaseComment _mEntity;
        private readonly List<BaseComment> _data;

        public bool Initialized { get { if (_data.Count != 0) { return true; } else { return false; } } }

        public CommentMock(List<BaseComment> users)
        {
            _data = users;
        }

        public BaseComment GetById(int? id)
        {
            foreach (var item in _data)
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
            return _data[random.Next(0, _data.Count)];
        }

        public List<BaseComment> Get()
        {
            return _data;
        }

        public int Save(BaseComment entity)
        {
            _data.Add(entity);
            _mFlag = true;
            _mEntity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, BaseComment newEntity)
        {
            _data[oldEntity] = newEntity;
            _mEntity = newEntity;
            _mFlag = true;
            return oldEntity;
        }

        public void Delete(BaseComment entity)
        {
            _data.Remove(entity);
            _mEntity = entity;
            _mFlag = true;
        }

        public bool MethodIsCalled(int id)
        {
            if (_mFlag && _mEntity.Id == id)
            {
                _mFlag = false;
                return true;
            }
            return false;
        }
    }
}
