using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using FLS.MyFirstProject.Infrastructure;

namespace MVCProject.Controllers
{
    public class EditArticleController : Controller
    {
        //
        // GET: /EditArticle/

        private readonly Facade _mFacade = MvcApplication.Facade;

        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Submit(ArticleViewModel model)
        {
            _mFacade.UpdateArticle(model.Id, model.Title, model.Content);
            return Redirect("~/ArticleListing/Index");
        }

    }
}
