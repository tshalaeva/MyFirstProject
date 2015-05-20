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

        private readonly Facade m_facade = MvcApplication.Facade;

        public ActionResult Index(int page = 1, int size = 2)
        {
            var articles = m_facade.GetArticles();
            var from = size * (page - 1);
            var model = new ArticleListingViewModel();
            var count = articles.Count() / size;
            if (articles.Count() > size * count)
            {
                count++;
            }

            model.TotalCount = count;
            model.Articels =
                articles.Skip(from)
                    .Take(size)
                    .Select(
                        article =>
                            new ArticleViewModel
                            {
                                Id = article.Id,
                                Author = article.Author.NickName,
                                Title = article.Title,
                                Content = article.Content
                            })
                    .ToList();
            model.PageNumber = page;
            model.PageSize = size;

            return View(model);
        }

        public RedirectResult GoBack()
        {
            return Redirect("~/Home/Index");
        }

        public ActionResult OpenDetails(int? id)
        {
            if (!m_facade.ArticleExists(id)) return Redirect("~/ArticleListing/Index");
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

        public ActionResult OpenCreateUser()
        {
            return View();
        }
    }
}
