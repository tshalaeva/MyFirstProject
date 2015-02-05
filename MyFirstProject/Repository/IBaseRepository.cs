using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject.Entity;

namespace MyFirstProject.Repository
{
    public interface IBaseRepository
    {
        List<Article> GetArticles();

        List<User> GetUsers();

        List<Admin> GetAdmins();

        List<Author> GetAuthors();

        List<BaseComment> GetComments();

        void Initialize(); 
    }
}
