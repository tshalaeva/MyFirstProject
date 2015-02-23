using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFirstProject;
using MyFirstProject.Entities;
using MyFirstProject.Repository;
using NMock;

namespace NmockTests
{
    [TestClass]
    public class FacadeNmockTests
    {
        [TestMethod]
        public void NmAddingNewRating()
        {
            var mocks = new MockFactory();
            var mockRepository = mocks.CreateMock<Repository>();
            mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);        
            var article = new Article(1);
            var rating = new Rating(1);
            var newArticle = article;
            newArticle.AddRating(rating);
            mockRepository.Expects.One.Method(r => r.Update(article, newArticle))
                .With(new object[] { article, newArticle })
                .Will();
            var facade = new Facade(mockRepository.MockObject);
            facade.Update(article, newArticle);
            mocks.VerifyAllExpectationsHaveBeenMet();
            Assert.AreEqual(1, article.Ratings.Last().Value);
        }

        [TestMethod]
        public void NmCreationOfArticle()
        {
            var mocks = new MockFactory();
            var mockRepository = mocks.CreateMock<Repository>();
            mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
            var author = new Author(0);
            var article = new Article(0);
            article.Author = author;
            article.Content = "Content NMock Test";
            article.Title = "Title NMock Test";            
            mockRepository.Expects.One.Method(r => r.Save(article)).WithAnyArguments().Will();
            mockRepository.Expects.One.Method(r => r.Get<Article>()).WillReturn(new List<Article>() { article });
            var facade = new Facade(mockRepository.MockObject);
            facade.CreateArticle(0, author, "Title NMock Test", "Content NMock Test");            
            Assert.AreEqual(0, facade.Get<Article>().Last().Id);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void NmCreationOfComment()
        {
            var mocks = new MockFactory();
            var mockRepository = mocks.CreateMock<Repository>();
            mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
            var article = new Article(0);
            var user = new User(0);
            var comment = new Comment(10, "CommentTest", user, article);
            mockRepository.Expects.One.Method(r => r.Save(comment)).WithAnyArguments().Will();
            mockRepository.Expects.One.Method(r => r.Get<Comment>()).WillReturn(new List<Comment>() { comment });
            var facade = new Facade(mockRepository.MockObject);
            facade.CreateComment(10, article, user, "CommentTest");            
            Assert.AreEqual(10, facade.Get<Comment>().Last().Id);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void NmCreationOfReview()
        {
            var mocks = new MockFactory();
            var mockRepository = mocks.CreateMock<Repository>();
            mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
            var user = new User(0);
            var article = new Article(0);
            var rating = new Rating(7);
            var review = new Review(10, "Test Review", user, article, rating);
            mockRepository.Expects.One.Method(r => r.Save(review)).WithAnyArguments().Will();
            mockRepository.Expects.One.Method(r => r.Get<Review>()).WillReturn(new List<Review>() { review });
            mockRepository.Expects.One.Method(r => r.Get<BaseComment>()).WillReturn(new List<BaseComment>() { review });
            var facade = new Facade(mockRepository.MockObject);
            facade.CreateReview(
                10,
                "Test Review",
                rating,
                user,
                article);
            Assert.AreEqual(10, facade.Get<Review>().Last().Id);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
