using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FLS.MyFirstProject.Infrastructure;
using MVCProject.Models;
using ObjectRepository.Entities;

namespace MVCProject.Controllers
{
    public class WebReportController : HomeController
    {
        //
        // GET: /WebReport/        
        private readonly Facade m_facade = MvcApplication.Facade;

        [HttpPost]
        public ViewResult Submit(WebReportModel model)
        {
            switch (model.SelectedOption.Value)
            {
                case "1":
                    {
                        var articles = m_facade.GetArticles();
                        model.Content = TrimSpaces(string.Format("Title of article {0}: {1}", articles.First().Id,
                            articles.First().Title));
                        for (var i = 1; i < articles.Count; i++)
                        {
                            model.Content = string.Format("{0}{1}Title of article {2}: {3}", model.Content, Environment.NewLine, articles[i].Id,
                                TrimSpaces(articles[i].Title));

                        }
                        break;
                    }
                case "2":
                    {
                        var articles = m_facade.GetArticles();
                        foreach (var article in articles)
                        {
                            model.Content = string.Format("{0}\nAverage rating of article {1}: {2}", model.Content,
                                article.Id, article.GetAverageRating());
                        }
                        model.Content = TrimSpaces(model.Content);
                        break;
                    }
                case "3":
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
                    if (model.Content != null)
                    {
                        model.Content = TrimSpaces(model.Content);
                    }
                    break;
                }
                case "4":
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
                    model.Content = TrimSpaces(model.Content);
                    break;
                }
                case "5":
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
                    model.Content = TrimSpaces(model.Content);
                    break;                   
                }
                case "6":
                {
                    model.Content = string.Format("Random article id = {0}", m_facade.GetRandomArticle().Id);
                    model.Content = TrimSpaces(model.Content);                 
                    break;
                }

                case "7":
                {
                    model.Content = ShowAllUsers();
                    break;
                }
            }
            return View("~/Views/WebReport/WebReport.cshtml", model);
        }

        private static string TrimSpaces(string input)
        {
            const string pattern = "\\s{2,}";
            const string replacement = " ";
            var rgx = new Regex(pattern);
            var result = rgx.Replace(input, replacement);

            return result;
        }
    }
}
