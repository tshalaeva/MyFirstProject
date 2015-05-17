using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Routing;
using FLS.MyFirstProject.Infrastructure;
using MVCProject.Models;

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
            articleModel.Comments = new List<CommentViewModel>();//
            foreach (var comment in comments)
            {
                //articleModel.Comments.Add(new CommentViewModel(comment));
                articleModel.Comments.Add(new CommentViewModel()
                {
                    Content = comment.Content,
                    ArticleId = comment.Article.Id,
                    UserAge = comment.User.Age,
                    UserFirstName = comment.User.FirstName,
                    UserLastName = comment.User.LastName
                });
            }
            return View(articleModel);
        }

        public ActionResult CreateComment(ArticleViewModel model)
        {
            var user = _mFacade.CreateUser(model.NewComment.UserFirstName, model.NewComment.UserLastName, model.NewComment.UserAge);
            _mFacade.CreateComment(model.Id, _mFacade.GetUserById(user), model.NewComment.Content);
            return RedirectToAction("OpenDetails", "ArticleListing", new RouteValueDictionary() { { "id", model.Id } });
        }

        public ActionResult DeleteArticle(int id)
        {
            _mFacade.DeleteArticle(id);
            return Redirect("~/ArticleListing/Index");
        }

        public ActionResult DeleteComment(int id)
        {
            var articleId = _mFacade.GetCommentById(id).Article.Id;
            _mFacade.DeleteComment(id);
            return RedirectToAction("OpenDetails", "ArticleListing", new RouteValueDictionary() { { "id", articleId } });
        }

        public ActionResult EditComment(CommentViewModel model)
        {
            _mFacade.UpdateComment(model.Id, model.Content);
            return RedirectToAction("OpenDetails", "ArticleListing", new RouteValueDictionary() { { "id", model.ArticleId} });
        }

        public ActionResult OpenEditComment(int? id)
        {
            var comment = _mFacade.GetCommentById(id);
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
            return View(commentModel);
        }
    }
}
