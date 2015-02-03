using System.Collections.Generic;
using MyFirstProject.Entity;

namespace MyFirstProject.Repository
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

        List<BaseComment> GetComments();

        void DeleteArticle(Article article);

        void DeleteUser(User user);

        void DeleteAdmin(Admin admin);

        void DeleteAuthor(Author author);

        void DeleteComment(BaseComment comment);

        void Initialize();      
    }
}
