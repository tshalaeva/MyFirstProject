using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Repositories;
using FLS.MyFirstProject.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectRepository.Entities;

namespace Tests
{
    [TestClass]
    public class FacadeTest
    {
        private readonly ArticleMock m_articleRepository = new ArticleMock(new List<Article> { new Article(0), new Article(1), new Article(2), new Article(3) });
        private readonly UserMock m_userRepository = new UserMock(new List<User> { new User(0), new User(1) });
        private readonly CommentMock m_commentRepository = new CommentMock(new List<BaseComment> {new Comment(0, "Comment 0", new User(2), new Article(4))});
        private readonly ReviewMock m_reviewRepository = new ReviewMock(new List<BaseComment> { new Review(0, "Review 0", new User(2), new Article(4), new Rating(4)) });
        private readonly ReviewTextMock m_reviewTextRepository = new ReviewTextMock(new List<BaseComment> { new ReviewText(0, "Review 0", new User(2), new Article(4), new Rating(4)) });

        [TestMethod]
        [Description("Test: Filter comments by existing article")]
        public void FilterCommentsByExistingArticle()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);
            var article = new Article(10);
            var user = new User(0);
            var comment0 = new Comment(0, "Comment 0", user, article);
            var comment1 = new Comment(1, "Comment 1", user, article);

            facade.SaveArticle(article);
            facade.SaveComment(comment0);
            facade.SaveComment(comment1);
            var count = facade.FilterCommentsByArticle(article).Count;

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [Description("Test: Filter comments by non-existing article")]
        public void FilterCommentsByNonExistingArticle()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            var after = facade.FilterCommentsByArticle(new Article(5));

            Assert.AreEqual(0, after.Count);
        }

        [TestMethod]
        [Description("Test: Creation of new article")]
        public void CreationOfArticle()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            facade.CreateArticle(10, new Author(10), "Test Title", "Test Content");

            Assert.IsTrue(m_articleRepository.MethodIsCalled(10));
        }

        [TestMethod]
        [Description("Test: Creation of new comment")]
        public void CreationOfComment()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            var id = facade.CreateComment(facade.GetArticles().First().Id, new User(10), "CommentTest");

            Assert.IsTrue(m_commentRepository.MethodIsCalled(id));
        }

        [TestMethod]
        [Description("Test: Creation of new review")]
        public void CreationOfReview()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            facade.CreateReview(
                10,
                "Test Review",
                new Rating(7),
                new User(10),
                facade.GetArticles().First());

            Assert.IsTrue(m_reviewRepository.MethodIsCalled(10));
        }

        [TestMethod]
        [Description("Test: Updating of rating")]
        public void UpdateRating()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            facade.CreateReview(
                11,
                "Test Review",
                new Rating(1),
                facade.GetAllUsers().First(),
                facade.GetArticles()[1]);

            Assert.IsTrue(m_reviewRepository.MethodIsCalled(11));
        }

        [TestMethod]
        [Description("Test: Creation of new review text")]
        public void CreationOfReviewText()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            facade.CreateReviewText(
                10,
                "Test Review",
                new Rating(7),
                new User(10),
                facade.GetArticles().First());

            Assert.IsTrue(m_reviewTextRepository.MethodIsCalled(10));
        }

        [TestMethod]
        [Description("Save new article")]
        public void SaveNewArticle()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);
            var article = new Article(8);

            facade.SaveArticle(article);

            Assert.IsTrue(m_articleRepository.MethodIsCalled(8));
        }

        [TestMethod]
        [Description("Get last article")]
        public void GetLastArticle()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            var article = facade.GetArticles().Last();

            Assert.AreEqual(3, article.Id);
        }

        [TestMethod]
        [Description("Test deleting of article")]
        public void DeleteArticle()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            var article = facade.GetArticles()[3];
            facade.DeleteArticle(article.Id);

            Assert.IsTrue(m_articleRepository.MethodIsCalled(article.Id));
        }

        [TestMethod]
        [Description("Test Update method")]
        public void UpdateArticle()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);
            var article = new Article(0);

            facade.SaveArticle(article);
            var newArticle = article;
            facade.UpdateArticle(article.Id, "Updated title", newArticle.Content);

            Assert.IsTrue(m_articleRepository.MethodIsCalled(0));
        }

        [TestMethod]
        [Description("Test GetById method")]
        public void GetById()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            var comment = facade.GetCommentById(0);

            Assert.AreEqual(0, comment.Id);
        }

        [TestMethod]
        [Description("Test GetRandomArticle method")]
        public void GetRandom()
        {
            var facade = new Facade(m_userRepository, m_articleRepository, m_commentRepository, new DbAdminRepository(), new DbAuthorRepository(), m_reviewRepository, m_reviewTextRepository);

            var article0 = facade.GetRandomArticle();
            var id0 = article0.Id;
            facade.DeleteArticle(article0.Id);
            var article1 = facade.GetRandomArticle();

            Assert.AreNotEqual(id0, article1.Id);
        }
    }
}
