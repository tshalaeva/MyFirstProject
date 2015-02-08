using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    public class Repository : IRepository
    {
        private List<IEntity> m_data; 

        public Repository()
        {
            m_data = new List<IEntity>();
        }

        public List<T> Get<T>() where T : IEntity
        {
            return m_data.OfType<T>().ToList();
        }

        public void Save<T>(T entity) where T : IEntity
        {
            m_data.Add(entity);
        }

        public void Delete<T>(T entity) where T : IEntity
        {
            m_data.Remove(entity);
        }
       
        public virtual void Initialize()
        {
            for (var i = 0; i < 3; i++)
            {
                Save(new Author(i + 1));
            }

            for (var i = 0; i < Get<Author>().Count; i++)
            {
                Get<Author>()[i].FirstName = "Author";
                Get<Author>()[i].LastName = (i + 1).ToString();
                Get<Author>()[i].Age = 50 + i;
                Get<Author>()[i].NickName = "Author" + (i + 1).ToString();
                Get<Author>()[i].Popularity = i + 0.5;
            }            

            for (var i = 0; i < 3; i++)
            {
                Save(new User(i + 5));
            }

            for (var i = 0; i < Get<User>().Count; i++)
            {
                Get<User>()[i].FirstName = "User";
                Get<User>()[i].LastName = (i + 1).ToString();
                Get<User>()[i].Age = 30 + i;
            }

            Save(new Admin(4));
            Get<Admin>()[0].FirstName = "Admin";
            Get<Admin>()[0].LastName = "User";
            Get<Admin>()[0].Age = 58;
            Get<Admin>()[0].Privilegies = new List<string> { "edit", "read", "delete" };

            for (var i = 0; i < 4; i++)
            {
                Save(new Article(i + 1));
            }

            for (var i = 0; i < Get<Article>().Count; i++)
            {
                Get<Article>()[i].Content = string.Format("Text {0}", i + 1);
                Get<Article>()[i].Title = string.Format("Title {0}", i + 1);
                if (i == Get<Article>().Count - 1)
                {
                    Get<Article>()[i].Author = Get<Author>()[0];
                    break;
                }

                Get<Article>()[i].Author = Get<Author>()[i];
            }            
        }        
    }
}
