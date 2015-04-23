using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using DataAccessLayer;
using MVCProject.Models;
using ObjectRepository.Entities;

namespace MVCProject.Controllers
{
    public class WebReportController : Controller
    {
        //
        // GET: /WebReport/        
        private readonly Facade _mFacade = MvcApplication.Facade;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Submit(WebReportModel model)
        {
            switch (model.SelectedOption.Value)
            {
                case "1":
                    {
                        var articles = _mFacade.GetArticles();
                        model.Content = string.Format("Title of article {0}: {1}", articles.First().Id,
                            articles.First().Title);
                        for (var i = 1; i < articles.Count; i++)
                        {
                            model.Content = string.Format("{0}\nTitle of article {1}: {2}", model.Content, articles[i].Id,
                                articles[i].Title);

                        }

                        model.Content = TrimSpaces(model.Content);
                        break;
                    }
                case "2":
                    {
                        var articles = _mFacade.GetArticles();
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
                    var admins = _mFacade.GetAdmins();
                    foreach (var admin in admins)
                    {
                        var privilegies = admin.Privilegies.First();
                        for (var i = 1; i < admin.Privilegies.Count; i++)
                        {
                            privilegies = string.Format("{0}, {1}", privilegies, admin.Privilegies[i]);
                        }
                        model.Content = string.Format("{0}\nPrivilegies of {1} {2}: {3}", model.Content, admin.FirstName, admin.LastName, privilegies);
                    }
                    model.Content = TrimSpaces(model.Content);
                    break;
                }
                case "4":
                {
                    var articles = _mFacade.GetArticles();
                    model.Content = "List of comments for each article:";
                    foreach (var article in articles)
                    {
                        model.Content = string.Format("{0}\nArticle {1}: ", model.Content, article.Title);                        
                        var articleComments = _mFacade.FilterCommentsByArticle(article).OfType<Comment>().ToList();
                        var articleReviews = _mFacade.FilterCommentsByArticle(article).OfType<Review>().ToList();
                        var articleReviewTexts = _mFacade.FilterCommentsByArticle(article).OfType<ReviewText>().ToList();
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

                    foreach (var comment in _mFacade.GetComments())
                    {
                        model.Content = string.Format("{0}\n{1}: {2}", model.Content, comment.Content, comment.GetEntityCode());
                    }

                    var reviews = _mFacade.GetReviews();
                    foreach (var comment in reviews.Where(comment => !(comment is ReviewText)))
                    {
                        model.Content = string.Format("{0}\n{1}: {2}", model.Content, comment.Content, comment.GetEntityCode());
                    }

                    foreach (var comment in _mFacade.GetReviewTexts())
                    {
                        model.Content = string.Format("{0}\n{1}: {2}", model.Content, comment.Content, comment.GetEntityCode());
                    }
                    model.Content = TrimSpaces(model.Content);
                    break;                   
                }
                case "6":
                {
                    model.Content = string.Format("Random article id = {0}", _mFacade.GetRandomArticle().Id);
                    model.Content = TrimSpaces(model.Content);                 
                    break;
                }
            }
            return View(model);
        }

        private string TrimSpaces(string input)
        {
            const string pattern = "\\s{2,}";
            var replacement = Environment.NewLine;
            var rgx = new Regex(pattern);
            return rgx.Replace(input, replacement);
        }
    }
}
