using System.Web.Mvc;
using MVCProject.Models;
using MyFirstProject.Entities;

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
    }
}
