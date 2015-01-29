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
        private List<Comment> comments;

        public FacadeRepository()
        {
            articles = new List<Article>();
            users = new List<User>();
            admins = new List<Admin>();
            authors = new List<Author>();
            comments = new List<Comment>();
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

        public void deleteComment(Comment comment) { }

        public void deleteAuthor(Author author) { }

        public void deleteAdmin(Admin admin) { }

        public void deleteUser(User user) { }

        public void deleteArticle(Article article) { }

        public List<Comment> getComments() 
        {
            return comments;
        }

        public List<Author> getAuthors()
        {
            return authors;
        }

        public List<Admin> getAdmins()
        {
            return admins;
        }

        public List<User> getUsers()
        {
            return users;
        }

        public List<Article> getArticles()
        {
            return articles;
        }

        public void saveComment(Comment comment) { }

        public void saveAuthor(Author author) { }

        public void saveAdmin(Admin admin) { }

        public void saveUser(User user) { }

        public void saveArticle(Article article) { }
    }
}
