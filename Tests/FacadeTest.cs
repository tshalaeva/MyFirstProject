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
            var commentFacade = new Facade<Comment>(m_facadeRepository);
            var userFacade = new Facade<User>(m_facadeRepository);            
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var article = articleFacade.Get().First();
            //commentFacade.Save(new Comment(0, "Comment 1", userFacade.Get().First(), article));
            //commentFacade.Save(new Comment(1, "Comment 2", userFacade.Get().First(), article));
            var after = commentFacade.FilterCommentsByArticle(article);
            Assert.AreEqual(4, after.Count);
        }

        [TestMethod]
        [Description("Test: Filter comments by non-existing article")]
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
            Assert.AreEqual(articleFacade.Get().First().Ratings.First().Value, 5);
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
            m_facadeRepository.Save(new Article(8));
            int numberOfArticles = m_facadeRepository.Get<Article>().Count;
            Assert.AreEqual(5, numberOfArticles);
        }

        [TestMethod]
        [Description("Get last article")]
        public void GetLastArticle()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            var article = articleFacade.Get().Last();
            Assert.AreEqual(2, article.Id);
        }

        [TestMethod]
        [Description("Test deleting of article")]
        public void DeleteArticle()
        {
            var articleFacade = new Facade<Article>(m_facadeRepository);
            Article article = articleFacade.Get()[3];
            articleFacade.Delete(article);
            int numberOfArticles = articleFacade.Get().Count;
            Assert.AreEqual(3, numberOfArticles);
        }
    }
}
