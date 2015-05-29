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
        private readonly List<BaseComment> m_data;

        public bool Initialized { get { return m_data.Count != 0; }
        }

        public CommentMock(List<BaseComment> users)
        {
            m_data = users;
        }

        public int GetCount()
        {
            return m_data.Count;
        }

        public BaseComment GetById(int? id)
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

        public BaseComment GetRandom()
        {
            var random = new Random();
            return m_data[random.Next(0, m_data.Count)];
        }

        public List<BaseComment> Get()
        {
            return m_data;
        }

        public List<BaseComment> Get(int from, int count)
        {
            //
            return m_data;
        }

        public int Save(BaseComment entity)
        {
            m_data.Add(entity);
            m_flag = true;
            m_entity = entity;
            return entity.Id;
        }

        public int Update(int oldEntity, BaseComment newEntity)
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
}
