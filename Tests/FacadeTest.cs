using System;
using System.Linq;
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
        [Description("Test: Filter comments by existing article")] 
        public void FilterCommentsByExistingArticle()
        {
            var facade = new Facade<Article>(m_facadeRepository);
            var after = facade.FilterCommentsByArticle(new Article(0));
            Assert.AreEqual(2, after.Count);
        }

        [TestMethod]
        [Description("Test: Filter comments by non-existing artickle")]
        public void FilterCommentsByNonExistingArticle()
        {
            var facade = new Facade<Article>(m_facadeRepository);
            var after = facade.FilterCommentsByArticle(new Article(5));
            Assert.AreEqual(0, after.Count);
        }

        [TestMethod]
        [Description("Test: Creation of new article")]
        public void CreationOfArticle()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var article = articleFacade.CreateArticle(10, new Author(10), "Test Title", "Test Content");
            Assert.AreEqual(article.Title, "Test Title");
            Assert.AreEqual("Test Content", article.Content);
        }

        [TestMethod]
        [Description("Test: Creation of new comment")]
        public void CreationOfComment()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var comment = articleFacade.CreateComment(10, articleFacade.Get()[0], new User(10), "CommentTest");
            Assert.AreEqual(articleFacade.Get()[0].Id, comment.Article.Id);
            Assert.AreEqual("CommentTest", comment.Content);
        }

        [TestMethod]
        [Description("Test: Creation of new review")]
        public void CreationOfReview()
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
        [Description("Test: Updating of rating")]
        public void UpdateRating()
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
        [Description("Test: Creation of new review text")]
        public void CreationOfReviewText()
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

        [TestMethod]
        [Description("Save new article")]
        public void SaveNewArticle()
        {
            m_facadeRepository.Initialize();
            m_facadeRepository.Save(new Article(8));
            int numberOfArticles = m_facadeRepository.Get<Article>().Count;
            Assert.AreEqual(6, numberOfArticles);
        }

        [TestMethod]
        [Description("Get last article")]
        public void GetLastArticle()
        {
            m_facadeRepository.Initialize();
            Article article = m_facadeRepository.Get<Article>().Last();
            Assert.AreEqual(4, article.Id);
        }

        [TestMethod]
        [Description("Test deleting of article")]
        public void DeleteArticle()
        {
            m_facadeRepository.Initialize();
            Article article = m_facadeRepository.Get<Article>()[3];
            m_facadeRepository.Delete(article);
            int numberOfArticles = m_facadeRepository.Get<Article>().Count;
            Assert.AreEqual(4, numberOfArticles);
        }
    }
}
