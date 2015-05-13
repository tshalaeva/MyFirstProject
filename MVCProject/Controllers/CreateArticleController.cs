using System.Linq;
using System.Web.Mvc;
using FLS.MyFirstProject.Infrastructure;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class CreateArticleController : Controller
    {
        //
        // GET: /CreateArticle/

        private readonly Facade _mFacade = MvcApplication.Facade;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(ArticleViewModel model)
        {
            _mFacade.CreateArticle(0, _mFacade.GetAuthors().First(), model.Title, model.Content);
            return Redirect("~/ArticleListing/Index");
        }
    }
}
