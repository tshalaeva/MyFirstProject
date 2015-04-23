using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectRepository.Entities;

namespace Tests
{
    [TestClass]
    public class BaseCommentTest
    {
        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForComment()
        {            
            var article = new Article(0);
            var user = new User(4) { FirstName = "Test", LastName = "User" };
            var comment = new Comment(0, "Test content", user, article);  
                      
            var result = comment.ToString();

            Assert.AreEqual("Test User:\nTest content", result);
        }

        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForReview()
        {
            var article = new Article(0);
            var user = new User(4) { FirstName = "Test", LastName = "User" };
            var review = new Review(0, "Review Content", user, article, new Rating(4));             
               
            var result = review.ToString();

            Assert.AreEqual("Test User:\nReview Content \nRating: 4", result);
        }

        [TestMethod]
        [Description("Test ToString method")]
        public void TestToStringForReviewText()
        {
            var article = new Article(0);
            var user = new User(4) { FirstName = "Test", LastName = "User" };
            var review = new ReviewText(0, "Review Content", user, article, new Rating(4));

            var result = review.ToString();

            Assert.AreEqual("Test User:\nReview Content\nRating: Good", result);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsCommentReturn0()
        {            
            var comment = new Comment(0);

            var code = comment.GetEntityCode();

            Assert.IsTrue(code == 0);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsReviewReturn1()
        {
            var review = new Review(0);

            var code = review.GetEntityCode();

            Assert.IsTrue(code == 1);
        }

        [TestMethod]
        [Description("Test GetEntityCode method")]
        public void IfEntityIsReviewTextReturn2()
        {
            var reviewText = new ReviewText(0);

            var code = reviewText.GetEntityCode();

            Assert.IsTrue(code == 2);
        }
    }
}
