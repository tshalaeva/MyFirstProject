using System.Web.Mvc;
using System.Web.Routing;
using FLS.MyFirstProject.Infrastructure;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        private readonly Facade m_facade = MvcApplication.Facade;

        public ActionResult Save(ArticleViewModel model)
        {
            var user = m_facade.CreateUser(model.NewComment.UserFirstName, model.NewComment.UserLastName, model.NewComment.UserAge);
            m_facade.CreateComment(model.Id, m_facade.GetUserById(user), model.NewComment.Content);
            return RedirectToAction("Details", "Article", new RouteValueDictionary() { { "id", model.Id } });
        }

        public ActionResult Delete(int id)
        {
            var articleId = m_facade.GetCommentById(id).Article.Id;
            m_facade.DeleteComment(id);
            return RedirectToAction("Details", "Article", new RouteValueDictionary() { { "id", articleId } });
        }

        public ActionResult SaveChanges(CommentViewModel model)
        {
            m_facade.UpdateComment(model.Id, model.Content);
            return RedirectToAction("Details", "Article", new RouteValueDictionary() { { "id", model.ArticleId } });
        }

        public ActionResult Edit(int? id)
        {
            var comment = m_facade.GetCommentById(id);
            var commentModel = new CommentViewModel()
            {
                Content = comment.Content,
                UserFirstName = comment.User.FirstName,
                UserLastName = comment.User.LastName,
                Rating = null,
                Id = comment.Id,
                ArticleId = comment.Article.Id,
                UserAge = comment.User.Age
            };
            return View("~/Views/Comment/Edit.cshtml", commentModel);
        }

    }
}
