using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http;
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
        
        private readonly Facade m_facade = MvcApplication.Facade;

        public ActionResult Index()
        {
            var articles = m_facade.GetArticles();
            var articleModels = articles.Select(article => new ArticleModel(new ArticleAdapter(article))).ToList();
            return View(articleModels);
        }

        public RedirectResult GoBack()
        {
            return Redirect("~/Home/Index");
        }

        public ActionResult OpenDetails(int? id)
        {
            if (!m_facade.ArticleExists(id)) return Redirect("~/ArticleListing/Index");
            var article = m_facade.GetArticleById(id);
            var articleModel = new ArticleModel(new ArticleAdapter(article));
            var comments = m_facade.FilterCommentsByArticle(article);
            articleModel.Comments = new List<CommentModel>();
            foreach (var comment in comments)
            {
                articleModel.Comments.Add(new CommentModel(new CommentAdapter(comment)));
            }
            return View(articleModel);
        }

        public ActionResult OpenReports()
        {
            var webReportModel = new WebReportModel();
            return View(webReportModel);
        }
    }
}
