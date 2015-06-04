using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Repositories;
using ObjectRepository.Entities;

namespace Tests
{
    public class ArticleMock : IRepository<Article>
    {
        private bool m_flag;
        private Article m_entity;
        private readonly List<Article> m_data;

        public bool Initialized { get { return m_data.Count != 0; }
        }

        public ArticleMock(List<Article> articles)
        {
            m_data = articles;
        }

        public int GetCount()
        {
            return m_data.Count;
        }

        public Article GetById(int? id)
        {
            foreach (var item in m_data)
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
            return m_data[random.Next(0, m_data.Count)];
        }

        public List<Article> Get()
        {
            return m_data;
        }

        public List<Article> Get(int from, int count)
        {
        //
            return m_data;
        }

        public List<Article> Get(int from)
        {
            //
            return m_data;
        }

        public int Save(Article entity)
        {
            m_data.Add(entity);
            m_flag = true;
            m_entity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, Article newEntity)
        {
            m_data[oldEntity] = newEntity;
            m_entity = newEntity;
            m_flag = true;
            return oldEntity;
        }

        public void Delete(int entityId)
        {
            m_entity = m_data[entityId];
            m_data.Remove(m_entity);            
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
        private readonly List<User> m_data;

        public bool Initialized { get { return m_data.Count != 0; }
        }

        public UserMock(List<User> users)
        {
            m_data = users;
        }

        public int GetCount()
        {
            return m_data.Count;
        }

        public User GetById(int? id)
        {
            return m_data.FirstOrDefault(item => item.Id == id);
        }

        public User GetRandom()
        {
            var random = new Random();
            return m_data[random.Next(0, m_data.Count)];
        }

        public List<User> Get()
        {
            return m_data;
        }

        public List<User> Get(int from, int count)
        {
            //
            return m_data;
        }

        public List<User> Get(int from)
        {
            //
            return m_data;
        } 

        public int Save(User entity)
        {
            m_data.Add(entity);
            m_flag = true;
            m_entity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, User newEntity)
        {
            m_data[oldEntity] = newEntity;
            m_entity = newEntity;
            m_flag = true;
            return oldEntity;
        }

        public void Delete(int entityId)
        {
            m_entity = m_data[entityId];
            m_data.Remove(m_entity);            
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
        protected List<BaseComment> Data;

        public bool Initialized
        {
            get { return Data.Count != 0; }
        }

        public CommentMock(List<BaseComment> comments)
        {
            Data = comments;
        }

        public CommentMock()
        {
        }

        public int GetCount()
        {
            return Data.Count;
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

        public List<BaseComment> Get(int from, int count)
        {
            //
            return Data;
        }

        public List<BaseComment> Get(int from)
        {
            //
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

        public void Delete(int entityId)
        {
            m_entity = Data[entityId];
            Data.Remove(m_entity);
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

    public class ReviewMock : CommentMock, IRepository<Review>
    {
        private bool m_flag;
        private BaseComment m_entity;

        public ReviewMock(List<BaseComment> comments)
        {
            Data = comments;
        }

        public new Review GetById(int? id)
        {
            return Data.Where(item => item.Id == id).Cast<Review>().FirstOrDefault();
        }

        public new Review GetRandom()
        {
            var random = new Random();
            return (Review)Data[random.Next(0, Data.Count)];
        }

        public new List<Review> Get()
        {
            return Data.OfType<Review>().ToList();
        }

        public new List<Review> Get(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public new List<Review> Get(int filteredById)
        {
            throw new NotImplementedException();
        }

        public int Save(Review entity)
        {
            Data.Add(entity);
            m_flag = true;
            m_entity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, Review newEntity)
        {
            Data[oldEntity] = newEntity;
            m_entity = newEntity;
            m_flag = true;
            return oldEntity;
        }

        public new void Delete(int entityId)
        {
            m_entity = Data[entityId];
            Data.Remove(m_entity);
            m_flag = true;
        }

        public new bool MethodIsCalled(int id)
        {
            if (m_flag && m_entity.Id == id)
            {
                m_flag = false;
                return true;
            }
            return false;
        }
    }

    public class ReviewTextMock : CommentMock, IRepository<ReviewText>
    {
        private bool m_flag;
        private BaseComment m_entity;

        public ReviewTextMock(List<BaseComment> comments)
        {
            Data = comments;
        }

        public new ReviewText GetById(int? id)
        {
            return Data.Where(item => item.Id == id).Cast<ReviewText>().FirstOrDefault();
        }

        public new ReviewText GetRandom()
        {
            var random = new Random();
            return (ReviewText)Data[random.Next(0, Data.Count)];
        }

        public new List<ReviewText> Get()
        {
            return Data.OfType<ReviewText>().ToList();
        }

        public new List<ReviewText> Get(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public new List<ReviewText> Get(int filteredById)
        {
            throw new NotImplementedException();
        }

        public int Save(ReviewText entity)
        {
            Data.Add(entity);
            m_flag = true;
            m_entity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, ReviewText newEntity)
        {
            Data[oldEntity] = newEntity;
            m_entity = newEntity;
            m_flag = true;
            return oldEntity;
        }

        public new void Delete(int entityId)
        {
            m_entity = Data[entityId];
            Data.Remove(m_entity);
            m_flag = true;
        }

        public new bool MethodIsCalled(int id)
        {
            if (!m_flag || m_entity.Id != id) return false;
            m_flag = false;
            return true;
        }
    }
}
