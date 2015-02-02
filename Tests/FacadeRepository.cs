using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject;

namespace Tests
{
    class FacadeRepository : IRepository
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
            for (int i = 0; i < 5; i++)
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

        public void DeleteComment(BaseComment comment) { }

        public void DeleteAuthor(Author author) { }

        public void DeleteAdmin(Admin admin) { }

        public void DeleteUser(User user) { }

        public void DeleteArticle(Article article) { }

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

        public void SaveComment(Comment comment) { }

        public void SaveAuthor(Author author) { }

        public void SaveAdmin(Admin admin) { }

        public void SaveUser(User user) { }

        public void SaveArticle(Article article) { }
    }
}
