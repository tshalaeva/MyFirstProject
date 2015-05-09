using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using FLS.MyFirstProject.Infrastructure;

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
