using System.Web.Mvc;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        public RedirectResult OpenArticleListing()
        {
            return Redirect("~/ArticleListing/Index");
        }
    }
}
