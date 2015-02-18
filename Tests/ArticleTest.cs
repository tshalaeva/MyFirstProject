﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFirstProject;
using MyFirstProject.Entities;

namespace Tests
{
    [TestClass]
    public class ArticleTest
    {
        private Mock m_articleRepository = new Mock();

        [TestMethod]
        [Description("If article has reviews, get average rating")]
        public void IfArticleHasReviewsGetAverageRating()
        {
            var articleFacade = new Facade<Article>(m_articleRepository);
            var userFacade = new Facade<User>(m_articleRepository);
            var reviewFacade = new Facade<Review>(m_articleRepository);
            var article = articleFacade.Get().First();
            var user0 = userFacade.Get().First();
            var user1 = userFacade.Get()[1];
            reviewFacade.Save(new Review(0, "Review 0", user0, article, new Rating(5)));
            reviewFacade.Save(new ReviewText(1, "Review text 1", user1, article, new Rating(3)));
            var avgRating = article.GetAverageRating();
            Assert.AreEqual(3, avgRating);
        }

        [TestMethod]
        [Description("If article does not have reviews, average rating = 0")]
        public void IfArticleDoesNotHaveReviewsGet0()
        {
            var articleFacade = new Facade<Article>(m_articleRepository);
            var article = articleFacade.Get()[2];
            var avgRating = article.GetAverageRating();
            Assert.AreEqual(0, avgRating);
        }

        [TestMethod]
        [Description("Add rating for article")]
        public void AddingNewRating()
        {
            var articleFacade = new Facade<Article>(m_articleRepository);
            var rating = new Rating(1);
            var article = articleFacade.Get().First();
            var newArticle = article;
            newArticle.AddRating(rating);
            articleFacade.Update(article, newArticle);
            Assert.AreEqual(1, article.Ratings.Last().Value);
        }
    }
}