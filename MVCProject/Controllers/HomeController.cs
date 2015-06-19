using System;
using System.Web.Mvc;
using FLS.MyFirstProject.Infrastructure;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly Facade m_facade = MvcApplication.Facade;

        public string ShowAllUsers()
        {
            var result = "";
            var users = m_facade.GetAllUsers();
            for (var i = 0; i < users.Count; i++)
            {
                result = string.Format("{0}{1}{2}. {3} {4}, {5}", result, Environment.NewLine, i + 1, users[i].FirstName.Trim(),
                    users[i].LastName.Trim(), users[i].Age);
            }
            return result;
        }

        //
        // GET: /Home/                
    }
}