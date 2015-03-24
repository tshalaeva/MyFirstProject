using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCProject.Models;
using MyFirstProject;
using MyFirstProject.Entities;
using MyFirstProject.Repository;

namespace MVCProject.Controllers
{
    public class ArticleDetailsController : Controller
    {
        //
        // GET: /ArticleDetails/

        private Facade m_facade = IocContainer.Container.GetInstance<Facade>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowComments(Article article)
        {            
            var articleComments = m_facade.FilterCommentsByArticle<Comment>(article);
            var commentModels = new List<CommentModel>(articleComments.Count);
            commentModels.AddRange(articleComments.Select(comment => new CommentModel(comment)));
            return View("Index");
        }
    }
}
