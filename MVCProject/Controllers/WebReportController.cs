﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCProject.Models;
using MyFirstProject;
using MyFirstProject.Entities;
using MyFirstProject.Repository;

namespace MVCProject.Controllers
{
    public class WebReportController : Controller
    {
        //
        // GET: /WebReport/
        private readonly Facade m_facade = IocContainer.Container.GetInstance<Facade>();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Submit(WebReportModel model)
        {
            var dropdown = Request["dropdownlist"];

            int value;

            if (!int.TryParse(dropdown, out value))
            {
                return View("Error");
            }

            switch (value)
            {
                case 1:
                    {
                        var articles = m_facade.GetArticles();
                        model.Content = string.Format("Title of article {0}: {1}", articles.First().Id,
                            articles.First().Title);
                        for (var i = 1; i < articles.Count; i++)
                        {
                            model.Content = string.Format("{0}\nTitle of article {1}: {2}", model.Content, articles[i].Id,
                                articles[i].Title);
                        }
                        model.Options.Find(item => item.Value.Equals("1")).Selected = true;
                        break;
                    }
                case 2:
                    {
                        var articles = m_facade.GetArticles();
                        foreach (var article in articles)
                        {
                            model.Content = string.Format("{0}\nAverage rating of article {1}: {2}", model.Content,
                                article.Id, article.GetAverageRating());
                        }
                        model.Options.Find(item => item.Value.Equals("2")).Selected = true;
                        break;
                    }
                case 3:
                {
                    var admins = m_facade.GetAdmins();
                    foreach (var admin in admins)
                    {
                        var privilegies = admin.Privilegies.First();
                        for (var i = 1; i < admin.Privilegies.Count; i++)
                        {
                            privilegies = string.Format("{0}, {1}", privilegies, admin.Privilegies[i]);
                        }
                        model.Content = string.Format("{0}\nPrivilegies of {1} {2}: {3}", model.Content, admin.FirstName, admin.LastName, privilegies);
                    }
                    model.Options.Find(item => item.Value.Equals("3")).Selected = true;
                    break;
                }
                case 4:
                {
                    var articles = m_facade.GetArticles();
                    model.Content = "List of comments for each article:";
                    foreach (var article in articles)
                    {
                        model.Content = string.Format("{0}\nArticle {1}: ", model.Content, article.Title);                        
                        var articleComments = m_facade.FilterCommentsByArticle(article).OfType<Comment>().ToList();
                        var articleReviews = m_facade.FilterCommentsByArticle(article).OfType<Review>().ToList();
                        var articleReviewTexts = m_facade.FilterCommentsByArticle(article).OfType<ReviewText>().ToList();
                        var allArticleComments = new List<BaseComment>(articleComments.Count + articleReviews.Count + articleReviewTexts.Count);
                        allArticleComments.AddRange(articleComments);
                        allArticleComments.AddRange(articleReviews);
                        allArticleComments.AddRange(articleReviewTexts);
                        foreach (var comment in allArticleComments)
                        {
                            model.Content = string.Format("{0}\n{1}", model.Content, comment);
                        }                        
                    }
                    model.Options.Find(item => item.Value.Equals("4")).Selected = true;
                    break;
                }
                case 5:
                {
                    model.Content = "Entity Codes:";

                    foreach (var comment in m_facade.GetComments())
                    {
                        model.Content = string.Format("{0}\n{1}: {2}", model.Content, comment.Content, comment.GetEntityCode());
                    }

                    var reviews = m_facade.GetReviews();
                    foreach (var comment in reviews.Where(comment => !(comment is ReviewText)))
                    {
                        model.Content = string.Format("{0}\n{1}: {2}", model.Content, comment.Content, comment.GetEntityCode());
                    }

                    foreach (var comment in m_facade.GetReviewTexts())
                    {
                        model.Content = string.Format("{0}\n{1}: {2}", model.Content, comment.Content, comment.GetEntityCode());
                    }
                    model.Options.Find(item => item.Value.Equals("5")).Selected = true;
                    break;                   
                }
                case 6:
                {
                    model.Content = string.Format("Random article id = {0}", m_facade.GetRandomArticle().Id);
                    model.Options.Find(item => item.Value.Equals("6")).Selected = true;
                    break;
                }
            }
            return View(model);
        }

        
    }
}