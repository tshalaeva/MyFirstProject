using System.Web.Mvc;

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
