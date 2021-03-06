﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FLS.MyFirstProject.Infrastructure;
using MVCProject.Models;
using NLog;

namespace MVCProject.Controllers
{
    public class ArticleController : HomeController
    {
        //
        // GET: /ArticleListing/

        private readonly Facade m_facade = MvcApplication.Facade;

        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index(string view, string sortBy = "", string order = "", int page = 1, int size = 8)
        {
            MvcApplication.Cookie.Values["page"] = page.ToString();
            if (view == null)
            {
                view = "List";
                MvcApplication.Cookie.Values["view"] = view;
            }

            var from = size * (page - 1) + 1;
            var articles = m_facade.GetArticles(sortBy, from, size - 1, order);
            var model = new ArticleListingViewModel();
            var count = m_facade.GetArticlesCount() / size;
            if (m_facade.GetArticlesCount() > size * count)
            {
                count++;
            }
            model.TotalCount = count;
            model.Articles = new List<ArticleViewModel>();

            foreach (var article in articles)
            {
                model.Articles.Add(new ArticleViewModel { Id = article.Id, Author = article.Author.NickName, Title = article.Title, Content = article.Content });
            }
            model.PageNumber = page;
            model.PageSize = size;
            model.View = view;

            return View("~/Views/Article/ArticleListing.cshtml", model);
        }

        public ActionResult Home()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult Details(int? id)
        {
            try
            {
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
            catch (Exception)
            {
                m_logger.Error("Article with this id was not found");
                return View("~/Views/Shared/Error.cshtml");
                throw;
            }
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

            return Index("List");
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
            return Index("List");
        }

        public ActionResult SaveChanges(ArticleViewModel model)
        {
            m_facade.UpdateArticle(model.Id, model.Title, model.Content);
            return Index("List");
        }

        public ActionResult ChangeView(string view, string sortBy,string order, string page)
        {
            switch (view)
            {
                case "List":
                    {
                        MvcApplication.Cookie.Values["view"] = "Grid";
                        HttpContext.Response.SetCookie(MvcApplication.Cookie);
                        return Index("Grid",sortBy, order, Convert.ToInt32(page));
                    }
                case "Grid":
                    {
                        MvcApplication.Cookie.Values["view"] = "List";
                        HttpContext.Response.SetCookie(MvcApplication.Cookie);
                        return Index("List", sortBy, order, Convert.ToInt32(page));
                    }
                default:
                    {
                        m_logger.Error("Incorrect page view");
                        return View("~/Views/Shared/Error.cshtml");
                    }
            }
        }

        public ActionResult Sort(string order, string sortBy, string view, int page)
        {
            MvcApplication.Cookie.Values["view"] = view;
            MvcApplication.Cookie.Values["page"] = page.ToString();
            switch (order)
            {
                case "asc":
                    {
                        MvcApplication.Cookie.Values["sortOrder"] = "asc";
                        return Index(view, sortBy, "ASC", page);
                    }
                case "desc":
                    {
                        MvcApplication.Cookie.Values["sortOrder"] = "desc";
                        return Index(view, sortBy, "DESC", page);
                    }

                default:
                    {
                        m_logger.Error("Incorrect sort order");
                        return View("~/Views/Shared/Error.cshtml");
                    }
            }
        }
    }
}
