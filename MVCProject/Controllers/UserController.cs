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
        private readonly Facade m_facade = MvcApplication.Facade;

        public ActionResult Create()
        {
            return View("~/Views/User/CreateUser.cshtml");
        }

        public ActionResult Save(UserViewModel model)
        {
            if (!ModelState.IsValid) return View("CreateUser");
            m_facade.CreateUser(model.FirstName, model.LastName, model.Age);
            return UserList();
        }        

        public ActionResult Delete(int? id)
        {
            m_facade.DeleteUser((int)id);
            return UserList();
        }

        public ActionResult Edit(int id)
        {
            var user = m_facade.GetUserById(id);
            var userModel = new UserViewModel()
            {
                Age = user.Age,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = id
            };
            return View("~/Views/User/EditUser.cshtml", userModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Submit(UserViewModel model)
        {
            m_facade.UpdateUser(model.Id, model.FirstName, model.LastName, model.Age);
            return UserList();
        }

        public ActionResult UserList()
        {
            var users = m_facade.GetAllUsers();
            var userModels = users.Select(user => new UserViewModel { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Age = user.Age }).ToList();
            return View("~/Views/User/UserList.cshtml", userModels);
        }        
    }
}
