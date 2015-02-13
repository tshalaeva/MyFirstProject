using System;
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
            m_articleRepository.Initialize();
            Article article = m_articleRepository.Get<Article>()[0];
            m_articleRepository.Save<User>(new User(0));
            m_articleRepository.Save<User>(new User(1));
            User user0 = m_articleRepository.Get<User>()[0];
            User user1 = m_articleRepository.Get<User>()[1];
            m_articleRepository.Save<Review>(new Review(0, "Review 0", user0, article, new Rating(5)));
            m_articleRepository.Save<ReviewText>(new ReviewText(1, "Review text 1", user1, article, new Rating(3)));                        
            int avgRating = article.GetAverageRating();
            Assert.AreEqual(4, avgRating);
        }

        [TestMethod]
        [Description("If article does not have reviews, average rating = 0")]
        public void IfArticleDoesNotHaveReviewsGet0()
        {
            m_articleRepository.Initialize();
            Article article = m_articleRepository.Get<Article>()[2];
            int avgRating = article.GetAverageRating();
            Assert.AreEqual(0, avgRating);
        }
    }
}
