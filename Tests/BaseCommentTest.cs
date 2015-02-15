using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFirstProject.Entities;

namespace Tests
{
    [TestClass]
    public class BaseCommentTest
    {
        private Mock m_baseCommentRepository = new Mock();
        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForComment()
        {
            m_baseCommentRepository.Initialize();
            Article article = m_baseCommentRepository.Get<Article>()[0];
            User user = new User(4) { FirstName = "Test", LastName = "User" };
            m_baseCommentRepository.Save(user);
            m_baseCommentRepository.Save(new Comment(0, "Test content", user, article));
            string result = m_baseCommentRepository.Get<Comment>()[5].ToString();
            Assert.AreEqual("Test User:\nTest content", result);
        }

        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForReview()
        {
            m_baseCommentRepository.Initialize();
            Article article = m_baseCommentRepository.Get<Article>()[0];
            User user = m_baseCommentRepository.Get<User>().Last();
            user.FirstName = "Test";
            user.LastName = "User";
            m_baseCommentRepository.Save(new Review(0, "Review Content", user, article, new Rating(4)));
            string result = m_baseCommentRepository.Get<Review>().Last().ToString();
            Assert.AreEqual("Test User:\nReview Content \nRating: 4", result);
        }

        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForReviewText()
        {
            m_baseCommentRepository.Initialize();
            Article article = m_baseCommentRepository.Get<Article>()[0];
            User user = m_baseCommentRepository.Get<User>().Last();
            user.FirstName = "Test";
            user.LastName = "User";
            m_baseCommentRepository.Save(new ReviewText(0, "Review Content", user, article, new Rating(4)));
            string result = m_baseCommentRepository.Get<Review>().Last().ToString();
            Assert.AreEqual("Test User:\nReview Content\nRating: Good", result);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsCommentReturn0()
        {
            m_baseCommentRepository.Initialize();
            Comment comment = m_baseCommentRepository.Get<Comment>()[0];
            Assert.IsTrue(comment.GetEntityCode() == 0);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsReviewReturn1()
        {
            m_baseCommentRepository.Initialize();
            Review review = m_baseCommentRepository.Get<Review>()[0];
            Assert.IsTrue(review.GetEntityCode() == 1);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsReviewTextReturn2()
        {
            m_baseCommentRepository.Initialize();
            ReviewText reviewText = m_baseCommentRepository.Get<ReviewText>()[0];
            Assert.IsTrue(reviewText.GetEntityCode() == 2);
        }
    }
}
