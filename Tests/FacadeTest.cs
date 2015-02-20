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
        private readonly Mock m_facadeRepository = new Mock();
        [TestMethod]
        [Description("Test: Filter comments by existing article")]
        public void FilterCommentsByExistingArticle()
        {
            var facade = new Facade(m_facadeRepository);
            var article = facade.Get<Article>().First();
            var after = facade.FilterCommentsByArticle<BaseComment>(article);
            Assert.AreEqual(4, after.Count);
        }

        [TestMethod]
        [Description("Test: Filter comments by non-existing article")]
        public void FilterCommentsByNonExistingArticle()
        {
            var facade = new Facade(m_facadeRepository);
            var after = facade.FilterCommentsByArticle<BaseComment>(new Article(5));
            Assert.AreEqual(0, after.Count);
        }

        [TestMethod]
        [Description("Test: Creation of new article")]
        public void CreationOfArticle()
        {            
            var articleFacade = new Facade(m_facadeRepository);
            var article = articleFacade.CreateArticle(10, new Author(10), "Test Title", "Test Content");
            Assert.AreEqual(article.Title, "Test Title");
            Assert.AreEqual("Test Content", article.Content);
        }

        [TestMethod]
        [Description("Test: Creation of new comment")]
        public void CreationOfComment()
        {
            var facade = new Facade(m_facadeRepository);
            var comment = facade.CreateComment(10, facade.Get<Article>().First(), new User(10), "CommentTest");
            Assert.AreEqual(facade.Get<Article>().First().Id, comment.Article.Id);
            Assert.AreEqual("CommentTest", comment.Content);
        }

        [TestMethod]
        [Description("Test: Creation of new review")]
        public void CreationOfReview()
        {
            var facade = new Facade(m_facadeRepository);
            var review = facade.CreateReview(
                10,
                "Test Review",
                new Rating(7),
                new User(10),
                facade.Get<Article>().First());
            Assert.IsTrue((review.Content == "Test Review") && (review.Rating.Value == 5));
        }

        [TestMethod]
        [Description("Test: Updating of rating")]
        public void UpdateRating()
        {
            var facade = new Facade(m_facadeRepository);
            Assert.AreEqual(facade.Get<Article>().First().Ratings.First().Value, 5);
            var review = facade.CreateReview(
                11,
                "Test Review",
                new Rating(1),
                facade.Get<User>().First(),
                facade.Get<Article>()[1]);
            Assert.IsTrue((review.Content == "Test Review") && (review.Rating.Value == 1));
        }

        [TestMethod]
        [Description("Test: Creation of new review text")]
        public void CreationOfReviewText()
        {
            var facade = new Facade(m_facadeRepository);
            var review = facade.CreateReviewText(
                10,
                "Test Review",
                new Rating(7),
                new User(10),
                facade.Get<Article>().First());
            Assert.IsTrue((review.Content == "Test Review") && (review.Rating.Value == 5));
        }

        [TestMethod]
        [Description("Save new article")]
        public void SaveNewArticle()
        {            
            m_facadeRepository.Save(new Article(8));
            int numberOfArticles = m_facadeRepository.Get<Article>().Count;
            Assert.AreEqual(5, numberOfArticles);
        }

        [TestMethod]
        [Description("Get last article")]
        public void GetLastArticle()
        {
            var facade = new Facade(m_facadeRepository);
            var article = facade.Get<Article>().Last();
            Assert.AreEqual(2, article.Id);
        }

        [TestMethod]
        [Description("Test deleting of article")]
        public void DeleteArticle()
        {
            var facade = new Facade(m_facadeRepository);
            Article article = facade.Get<Article>()[3];
            facade.Delete(article);
            int numberOfArticles = facade.Get<Article>().Count;
            Assert.AreEqual(3, numberOfArticles);
        }
    }
}
