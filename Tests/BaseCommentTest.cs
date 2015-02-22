using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFirstProject;
using MyFirstProject.Entities;

namespace Tests
{
    [TestClass]
    public class BaseCommentTest
    {
        private readonly Mock m_baseCommentRepository = new Mock();
        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForComment()
        {            
            var article = m_baseCommentRepository.Get<Article>().First();
            var user = new User(4) { FirstName = "Test", LastName = "User" };
            var comment = new Comment(0, "Test content", user, article);            
            var result = comment.ToString();
            Assert.AreEqual("Test User:\nTest content", result);
        }

        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForReview()
        {
            var article = m_baseCommentRepository.Get<Article>().First();
            var user = new User(4) { FirstName = "Test", LastName = "User" };
            var review = new Review(0, "Review Content", user, article, new Rating(4));                
            var result = review.ToString();
            Assert.AreEqual("Test User:\nReview Content \nRating: 4", result);
        }

        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForReviewText()
        {
            var article = m_baseCommentRepository.Get<Article>().First();
            var user = new User(4) { FirstName = "Test", LastName = "User" };
            var review = new ReviewText(0, "Review Content", user, article, new Rating(4));
            var result = review.ToString();
            Assert.AreEqual("Test User:\nReview Content\nRating: Good", result);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsCommentReturn0()
        {
            var facade = new Facade(m_baseCommentRepository); 
            var comment = facade.Get<Comment>().First();
            Assert.IsTrue(comment.GetEntityCode() == 0);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsReviewReturn1()
        {
            var facade = new Facade(m_baseCommentRepository);         
            var review = facade.Get<Review>().First();
            Assert.IsTrue(review.GetEntityCode() == 1);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsReviewTextReturn2()
        {
            var facade = new Facade(m_baseCommentRepository);        
            var reviewText = facade.Get<ReviewText>().First();
            Assert.IsTrue(reviewText.GetEntityCode() == 2);
        }
    }
}
