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
            var article = facade.Get<Article>().First();
            var user0 = facade.Get<User>().First();
            var user1 = facade.Get<User>()[1];
            facade.Save(new Review(0, "Review 0", user0, article, new Rating(5)));
            facade.Save(new ReviewText(1, "Review text 1", user1, article, new Rating(3)));
            var avgRating = article.GetAverageRating();
            Assert.AreEqual(3, avgRating);
        }

        [TestMethod]
        [Description("If article does not have reviews, average rating = 0")]
        public void IfArticleDoesNotHaveReviewsGet0()
        {
            var facade = new Facade(m_articleRepository);
            var article = facade.Get<Article>()[2];
            var avgRating = article.GetAverageRating();
            Assert.AreEqual(0, avgRating);
        }

        [TestMethod]
        [Description("Add rating for article")]
        public void AddingNewRating()
        {
            var facade = new Facade(m_articleRepository);
            var rating = new Rating(1);
            var article = facade.Get<Article>().First();
            var newArticle = article;
            newArticle.AddRating(rating);
            facade.Update(article, newArticle);
            Assert.AreEqual(1, article.Ratings.Last().Value);
        }
    }
}
