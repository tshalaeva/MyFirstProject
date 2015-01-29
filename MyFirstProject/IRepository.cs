using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public interface IRepository
    {
        void SaveArticle(Article article);

        void SaveUser(User user);

        void SaveAdmin(Admin admin);

        void SaveAuthor(Author author);

        void SaveComment(Comment comment);

        List<Article> GetArticles();

        List<User> GetUsers();

        List<Admin> GetAdmins();

        List<Author> GetAuthors();

        List<Comment> GetComments();

        void DeleteArticle(Article article);

        void DeleteUser(User user);

        void DeleteAdmin(Admin admin);

        void DeleteAuthor(Author author);

        void DeleteComment(Comment comment);

        void Initialize();      
    }
}
