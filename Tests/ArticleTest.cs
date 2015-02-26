
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFirstProject;
using MyFirstProject.Entities;

namespace Tests
{
    [TestClass]
    public class ArticleTest
    {
        private readonly Mock m_articleRepository = new Mock();

        [TestMethod]
        [Description("If article has reviews, get average rating")]
        public void IfArticleHasReviewsGetAverageRating()
        {
            var facade = new Facade(m_articleRepository);
            var article = new Article(0);
            var user0 = new User(0);
            var user1 = new User(1);
            var review = new Review(0, "Review 0", user0, article, new Rating(5));
            var reviewText = new ReviewText(1, "Review text 1", user1, article, new Rating(3));

            facade.Save(review);
            facade.Save(reviewText);
            var avgRating = article.GetAverageRating();

            Assert.AreEqual(4, avgRating);
        }

        [TestMethod]
        [Description("If article does not have reviews, average rating = 0")]
        public void IfArticleDoesNotHaveReviewsGet0()
        {
            var facade = new Facade(m_articleRepository);
            var article = new Article(0);

            var avgRating = article.GetAverageRating();

            Assert.AreEqual(0, avgRating);
        }

        [TestMethod]
        [Description("Add rating for article")]
        public void AddingNewRating()
        {
            var facade = new Facade(m_articleRepository);
            var rating = new Rating(1);
            var article = new Article(0);
            var newArticle = article;

            facade.Save(article);
            newArticle.AddRating(rating);
            facade.Update(article, newArticle);

            Assert.AreEqual(1, article.Ratings.Last().Value);
        }
    }
}
