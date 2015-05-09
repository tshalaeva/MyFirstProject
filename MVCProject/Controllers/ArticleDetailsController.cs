using System.Web.Mvc;
using MVCProject.Models;
using FLS.MyFirstProject.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MVCProject.Controllers
{
    public class ArticleDetailsController : Controller
    {
        //
        // GET: /ArticleDetails/
        private Facade _mFacade = MvcApplication.Facade;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowComments()
        {            
            return View("Index");
        }

        public ActionResult EditArticle(int? id)
        {
            var article = _mFacade.GetArticleById(id);
            var articleModel = new ArticleViewModel()
            {
                Author = article.Author.NickName,
                Content = article.Content,
                Title = article.Title,
                Id = article.Id
            };
            var comments = _mFacade.FilterCommentsByArticle(article);
            articleModel.Comments = new List<CommentModel>();
            foreach (var comment in comments)
            {
                articleModel.Comments.Add(new CommentModel(comment));
            }
            return View(articleModel);
        }
    }
}
