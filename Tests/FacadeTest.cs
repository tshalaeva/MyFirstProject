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
            var facade = new Facade(m_facadeRepository);
            var article = new Article(10);
            var user = new User(0);
            var comment0 = new Comment(0, "Comment 0", user, article);
            var comment1 = new Comment(1, "Comment 1", user, article);

            facade.Save(article);
            facade.Save(comment0);
            facade.Save(comment1);
            var count = facade.FilterCommentsByArticle<BaseComment>(article).Count;

            Assert.AreEqual(2, count);
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

            articleFacade.CreateArticle(10, new Author(10), "Test Title", "Test Content");            

            Assert.IsTrue(m_facadeRepository.MethodIsCalled<Article>(10));
        }

        [TestMethod]
        [Description("Test: Creation of new comment")]
        public void CreationOfComment()
        {
            var facade = new Facade(m_facadeRepository);

            facade.CreateComment(10, facade.Get<Article>().First(), new User(10), "CommentTest");

            Assert.IsTrue(m_facadeRepository.MethodIsCalled<Comment>(10));
        }

        [TestMethod]
        [Description("Test: Creation of new review")]
        public void CreationOfReview()
        {
            var facade = new Facade(m_facadeRepository);

            facade.CreateReview(
                10,
                "Test Review",
                new Rating(7),
                new User(10),
                facade.Get<Article>().First());

            Assert.IsTrue(m_facadeRepository.MethodIsCalled<Review>(10));
        }

        [TestMethod]
        [Description("Test: Updating of rating")]
        public void UpdateRating()
        {
            var facade = new Facade(m_facadeRepository);      
                  
            facade.CreateReview(
                11,
                "Test Review",
                new Rating(1),
                facade.Get<User>().First(),
                facade.Get<Article>()[1]);

            Assert.IsTrue(m_facadeRepository.MethodIsCalled<Review>(11));
        }

        [TestMethod]
        [Description("Test: Creation of new review text")]
        public void CreationOfReviewText()
        {
            var facade = new Facade(m_facadeRepository);

            facade.CreateReviewText(
                10,
                "Test Review",
                new Rating(7),
                new User(10),
                facade.Get<Article>().First());

            Assert.IsTrue(m_facadeRepository.MethodIsCalled<ReviewText>(10));
        }

        [TestMethod]
        [Description("Save new article")]
        public void SaveNewArticle()
        {
            var facade = new Facade(m_facadeRepository);
            var article = new Article(8);

            facade.Save(article);

            Assert.IsTrue(m_facadeRepository.MethodIsCalled<Article>(8));
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

            var article = facade.Get<Article>()[3];
            facade.Delete(article);

            Assert.IsTrue(m_facadeRepository.MethodIsCalled<Article>(article.Id));
        }

        [TestMethod]
        [Description("Test Update method")]
        public void UpdateArticle()
        {
            var facade = new Facade(m_facadeRepository);
            var article = new Article(0);

            facade.Save(article);
            var newArticle = article;
            newArticle.Title = "Updated title";
            facade.Update(article, newArticle);

            Assert.IsTrue(m_facadeRepository.MethodIsCalled<Article>(0));
        }

        [TestMethod]
        [Description("Test GetById method")]
        public void GetById()
        {
            var facade = new Facade(m_facadeRepository);

            var comment = facade.GetById<Comment>(0);

            Assert.AreEqual(0, comment.Id);
        }

        [TestMethod]
        [Description("Test GetRandomArticle method")]
        public void GetRandom()
        {
            var facade = new Facade(m_facadeRepository);

            var article0 = facade.GetRandomArticle();
            var id0 = article0.Id;
            facade.Delete(article0);
            var article1 = facade.GetRandomArticle();

            Assert.AreNotEqual(id0, article1.Id);
        }
    }
}
