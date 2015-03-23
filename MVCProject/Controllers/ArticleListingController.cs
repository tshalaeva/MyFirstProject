using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
using MyFirstProject;
using MyFirstProject.Entities;
using MyFirstProject.Repository;

namespace MVCProject.Controllers
{
    public class ArticleListingController : Controller
    {
        //
        // GET: /ArticleListing/

        private readonly Facade m_facade = IocContainer.Container.GetInstance<Facade>();

        public ActionResult Index()
        {
            var articles = m_facade.Get<Article>();
            var articleModels = articles.Select(article => new ArticleModel(article)).ToList();
            return View(articleModels);
        }

        public RedirectResult GoBack()
        {
            return Redirect("~/Home/Index");
        }

        public ActionResult OpenDetails(int id)
        {
            var article = new ArticleModel(m_facade.GetById<Article>(id));
            return View(article);
        }
    }
}
