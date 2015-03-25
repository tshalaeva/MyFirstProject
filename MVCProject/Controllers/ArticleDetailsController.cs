using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCProject.Models;
using MyFirstProject;
using MyFirstProject.Entities;
using MyFirstProject.Repository;

namespace MVCProject.Controllers
{
    public class ArticleDetailsController : Controller
    {
        //
        // GET: /ArticleDetails/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowComments()
        {            
            return View("Index");
        }
    }
}
