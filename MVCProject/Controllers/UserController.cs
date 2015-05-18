using System.Linq;
using System.Web.Mvc;
using FLS.MyFirstProject.Infrastructure;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class UserController : HomeController
    {
        //
        // GET: /User/
        private Facade m_Facade = MvcApplication.Facade;

        public ActionResult CreateUser(UserViewModel model)
        {
            m_Facade.CreateUser(model.FirstName, model.LastName, model.Age);
            return UserList();
        }

        public ActionResult UserList()
        {
            var users = m_Facade.GetAllUsers();
            var userModels = users.Select(user => new UserViewModel() {Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Age = user.Age}).ToList();
            return View(userModels);
        }

        public ActionResult DeleteUser(int? id)
        {
            m_Facade.DeleteUser((int)id);
            return UserList();
        }

        public ActionResult EditUser(int id)
        {
            return View();
        }

        public ActionResult Submit(UserViewModel model)
        {        
            m_Facade.UpdateUser(model.Id, model.FirstName, model.LastName, model.Age);
            return UserList();
        }
    }
}
