using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FLS.MyFirstProject.Infrastructure;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class ArticleListingController : Controller
    {
        //
        // GET: /ArticleListing/

        private readonly Facade _mFacade = MvcApplication.Facade;

        public ActionResult Index()
        {
            var articles = _mFacade.GetArticles();
            var articleModels = articles.Select(article => new ArticleModel(article)).ToList();
            return View(articleModels);
        }

        public RedirectResult GoBack()
        {
            return Redirect("~/Home/Index");
        }

        public ActionResult OpenDetails(int? id)
        {
            if (!_mFacade.ArticleExists(id)) return Redirect("~/ArticleListing/Index");
            var article = _mFacade.GetArticleById(id);
            var articleModel = new ArticleModel(article);
            var comments = _mFacade.FilterCommentsByArticle(article);
            articleModel.Comments = new List<CommentModel>();
            foreach (var comment in comments)
            {
                articleModel.Comments.Add(new CommentModel(comment));
            }
            return View(articleModel);
        }

        public ActionResult OpenReports()
        {
            var webReportModel = new WebReportModel();
            return View(webReportModel);
        }

        public ActionResult OpenCreateArticle()
        {
            return View();
        }
    }
}
