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
        private Mock m_baseCommentRepository = new Mock();
        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForComment()
        {            
            Article article = m_baseCommentRepository.Get<Article>()[0];
            User user = new User(4) { FirstName = "Test", LastName = "User" };
            m_baseCommentRepository.Save(user);
            m_baseCommentRepository.Save(new Comment(2, "Test content", user, article));
            string result = m_baseCommentRepository.Get<Comment>().Last().ToString();
            Assert.AreEqual("Test User:\nTest content", result);
        }

        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForReview()
        {
            var article = m_baseCommentRepository.Get<Article>()[0];
            var user = m_baseCommentRepository.Get<User>().Last();
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
            var articleFacade = new Facade<Article>(m_baseCommentRepository);
            var userFacade = new Facade<User>(m_baseCommentRepository);
            var article = articleFacade.Get().First();
            var user = userFacade.Get().Last();
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
            var userFacade = new Facade<User>(m_baseCommentRepository);
            var articleFacade = new Facade<Article>(m_baseCommentRepository);
            var article = articleFacade.Get().First();
            var commentFacade = new Facade<Comment>(m_baseCommentRepository);
            commentFacade.Save(new Comment(0, "Comment 1", userFacade.Get().First(), article));
            commentFacade.Save(new Comment(1, "Comment 2", userFacade.Get().First(), article));            
            var comment = commentFacade.Get().First();
            Assert.IsTrue(comment.GetEntityCode() == 0);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsReviewReturn1()
        {            
            Review review = m_baseCommentRepository.Get<Review>().First();
            Assert.IsTrue(review.GetEntityCode() == 1);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsReviewTextReturn2()
        {            
            ReviewText reviewText = m_baseCommentRepository.Get<ReviewText>()[0];
            Assert.IsTrue(reviewText.GetEntityCode() == 2);
        }
    }
}
