using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject;
using MyFirstProject.Entity;
using MyFirstProject.Repository;

namespace Tests
{
    class FacadeRepository : IRepository<Article>, IRepository<User>, IRepository<Admin>, IRepository<Author>, IRepository<BaseComment>
    {
        private List<Article> articles;
        private List<User> users;
        private List<Admin> admins;
        private List<Author> authors;
        private List<BaseComment> comments;

        public FacadeRepository()
        {
            articles = new List<Article>();
            users = new List<User>();
            admins = new List<Admin>();
            authors = new List<Author>();
            comments = new List<BaseComment>();
        }

        public void Initialize()
        {            
            for (var i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    comments.Add(new Comment(i));
                    comments[i].Article = new Article(0);
                    break;
                }
                comments.Add(new Comment(i));
                comments[i].Article = new Article(i);
            }
        }

        public void Delete(BaseComment comment) { }

        public void Delete(Author author) { }

        public void Delete(Admin admin) { }

        public void Delete(User user) { }

        public void Delete(Article article) { }

        public List<BaseComment> GetComments() 
        {
            return comments;
        }

        public List<Author> GetAuthors()
        {
            return authors;
        }

        public List<Admin> GetAdmins()
        {
            return admins;
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public List<Article> GetArticles()
        {
            return articles;
        }

        public void Save(BaseComment comment) { }

        public void Save(Author author) { }

        public void Save(Admin admin) { }

        public void Save(User user) { }

        public void Save(Article article) { }
    }
}
