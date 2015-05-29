using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FLS.MyFirstProject.Infrastructure;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class ArticleController : HomeController
    {
        //
        // GET: /ArticleListing/

        private readonly Facade m_facade = MvcApplication.Facade;

        public ActionResult Index(int page = 1, int size = 8)
        {                                               
            var from = size * (page - 1) + 1;
            var articles = m_facade.GetArticles(from, size - 1);
            var model = new ArticleListingViewModel();            
            var count = m_facade.GetArticlesCount()/size;
            if (m_facade.GetArticlesCount() > size * count)
            {
                count++;
            }
            model.TotalCount = count;
            model.Articles = new List<ArticleViewModel>();
            
            foreach (var article in articles)
            {
                model.Articles.Add(new ArticleViewModel {Id = article.Id, Author = article.Author.NickName, Title = article.Title, Content = article.Content});
            }
            model.PageNumber = page;
            model.PageSize = size;

            return View("~/Views/Article/ArticleListing.cshtml", model);
        }

        public ActionResult Home()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult Details(int? id)
        {
            if (!m_facade.ArticleExists(id)) return Redirect("~/Article/ArticleListing");
            var article = m_facade.GetArticleById(id);
            var articleModel = new ArticleViewModel
            {
                Author = article.Author.NickName,
                Content = article.Content,
                Title = article.Title,
                Id = article.Id
            };
            var comments = m_facade.FilterCommentsByArticle(article);
            articleModel.Comments = new List<CommentViewModel>();
            foreach (var comment in comments)
            {
                articleModel.Comments.Add(new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    ArticleId = comment.Article.Id,
                    UserFirstName = comment.User.FirstName,
                    UserAge = comment.User.Age,
                    UserLastName = comment.User.LastName
                });
            }
            return View("~/Views/Article/Details.cshtml", articleModel);
        }

        public ActionResult Reports()
        {
            var webReportModel = new WebReportModel();
            return View("~/Views/WebReport/WebReport.cshtml", webReportModel);
        }

        public ActionResult Create()
        {
            return View("~/Views/Article/Create.cshtml");
        }        

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(ArticleViewModel articleModel)
        {
            if (!ModelState.IsValid) return View("~/Views/User/CreateUser.cshtml");
            m_facade.CreateArticle(0, m_facade.GetAuthors().First(), articleModel.Title, articleModel.Content);

            return Index();
        }

        public ActionResult ShowComments()
        {
            return View("~/Views/Article/Details.cshtml");
        }

        public ActionResult Edit(int? id)
        {
            var article = m_facade.GetArticleById(id);
            var articleModel = new ArticleViewModel()
            {
                Author = article.Author.NickName,
                Content = article.Content,
                Title = article.Title,
                Id = article.Id
            };
            var comments = m_facade.FilterCommentsByArticle(article);
            articleModel.Comments = new List<CommentViewModel>();//
            foreach (var comment in comments)
            {
                articleModel.Comments.Add(new CommentViewModel()
                {
                    Content = comment.Content,
                    ArticleId = comment.Article.Id,
                    UserAge = comment.User.Age,
                    UserFirstName = comment.User.FirstName,
                    UserLastName = comment.User.LastName
                });
            }
            return View("~/Views/Article/Edit.cshtml", articleModel);
        }        

        public ActionResult Delete(int id)
        {
            m_facade.DeleteArticle(id);
            return Index();
        }                

        public ActionResult SaveChanges(ArticleViewModel model)
        {
            m_facade.UpdateArticle(model.Id, model.Title, model.Content);
            return Index();
        }
    }
}
