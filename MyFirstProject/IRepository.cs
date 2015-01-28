using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    interface IRepository
    {
        void saveArticle(Article article);

        void saveUser(User user);

        void saveAdmin(Admin admin);

        void saveAuthor(Author author);

        void saveComment(Comment comment);

        List<Article> getArticles();

        List<User> getUsers();

        List<Admin> getAdmins();

        List<Author> getAuthors();

        List<Comment> getComments();

        void deleteArticle(Article article);

        void deleteUser(User user);

        void deleteAdmin(Admin admin);

        void deleteAuthor(Author author);

        void deleteComment(Comment comment);

        void Initialize();      
    }
}
