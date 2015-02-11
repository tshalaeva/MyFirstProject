using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    public class Repository : IRepository
    {
        private List<IEntity> m_data;
        private bool m_initialized = false;

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
            if (m_initialized == false)
            {
                for (var i = 0; i < 3; i++)
                {
                    Save(new Author(i + 1));
                }

                var authors = Get<Author>();

                for (var i = 0; i < authors.Count; i++)
                {
                    authors[i].FirstName = "Author";
                    authors[i].LastName = (i + 1).ToString();
                    authors[i].Age = 50 + i;
                    authors[i].NickName = "Author" + (i + 1).ToString();
                    authors[i].Popularity = i + 0.5;
                }

                for (var i = 0; i < 3; i++)
                {
                    Save(new User(i + 5));
                }

                var users = Get<User>();

                for (var i = 0; i < users.Count; i++)
                {
                    users[i].FirstName = "User";
                    users[i].LastName = string.Format("{0}", i + 1);
                    users[i].Age = 30 + i;
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

                var articles = Get<Article>();

                for (var i = 0; i < articles.Count; i++)
                {
                    articles[i].Content = string.Format("Text {0}", i + 1);
                    articles[i].Title = string.Format("Title {0}", i + 1);
                    if (i == articles.Count - 1)
                    {
                        articles[i].Author = authors[0];
                        break;
                    }

                    articles[i].Author = authors[i];
                }

                m_initialized = true;
            }
        }
    }
}
