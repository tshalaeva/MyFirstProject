using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFirstProject;
using MyFirstProject.Entities;

namespace Tests
{
    [TestClass]
    public class FacadeTest
    {
        private Mock m_facadeRepository = new Mock();
        [TestMethod]
        public void TestCommentsExist()
        {
            var facade = new Facade<Article>(m_facadeRepository);
            var after = facade.FilterCommentsByArticle(new Article(0));
            Assert.AreEqual(2, after.Count);
        }

        [TestMethod]
        public void TestCommentsDoNotExist()
        {
            var facade = new Facade<Article>(m_facadeRepository);
            var after = facade.FilterCommentsByArticle(new Article(5));
            Assert.AreEqual(0, after.Count);
        }

        [TestMethod]
        public void TestCreationOfArticle()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var article = articleFacade.CreateArticle(10, new Author(10), "Test Title", "Test Content");
            Assert.AreEqual(article.Title, "Test Title");
            Assert.AreEqual("Test Content", article.Content);
        }

        [TestMethod]
        public void TestCreationOfComment()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var comment = articleFacade.CreateComment(10, articleFacade.Get()[0], new User(10), "CommentTest");
            Assert.AreEqual(articleFacade.Get()[0].Id, comment.Article.Id);
            Assert.AreEqual("CommentTest", comment.Content);
        }

        [TestMethod]
        public void TestCreationOfNewReview()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var review = articleFacade.CreateReview(
                10,
                "Test Review",
                new Rating(7),
                new User(10),
                articleFacade.Get()[0]);
            Assert.IsTrue((review.Content == "Test Review") && (review.Rating.Value == 5));
        }

        [TestMethod]
        public void TestUpdatingOfRating()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var userFacade = new Facade<User>(m_facadeRepository);
            Assert.AreEqual(articleFacade.Get()[1].Ratings[0].Value, 3);
            var review = articleFacade.CreateReview(
                11,
                "Test Review",
                new Rating(1),
                userFacade.Get()[0],
                articleFacade.Get()[1]);
            Assert.IsTrue((review.Content == "Test Review") && (review.Rating.Value == 1));
        }

        [TestMethod]
        public void TestCreationOfReviewText()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var review = articleFacade.CreateReviewText(
                10,
                "Test Review",
                new Rating(7),
                new User(10),
                articleFacade.Get()[0]);
            Assert.IsTrue((review.Content == "Test Review") && (review.Rating.Value == 5));
        }
    }
}
