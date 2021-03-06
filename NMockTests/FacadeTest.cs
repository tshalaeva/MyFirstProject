﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NmockTests
{
    [TestClass]
    public class FacadeNmockTests
    {
        //[TestMethod]
        //[Description("Add new rating fot article")]
        //public void AddingNewRating()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    var article = new Article(1);
        //    var rating = new Rating(1);
        //    var newArticle = article;
        //    mockRepository.Expects.One.Method(r => r.Update(article, newArticle))
        //        .With(article, newArticle)
        //        .Will();
        //    var facade = new Facade(mockRepository.MockObject);

        //    newArticle.AddRating(rating);
        //    facade.UpdateArticle(article, newArticle);

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //    Assert.AreEqual(1, article.Ratings.Last().Value);
        //}

        //[TestMethod]
        //[Description("Test of CreateArticle method")]
        //public void CreationOfArticle()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    var author = new Author(0);
        //    var article = new Article(0);
        //    mockRepository.Expects.One.Method(r => r.Save(article)).WithAnyArguments().Will();
        //    var facade = new Facade(mockRepository.MockObject);

        //    facade.CreateArticle(0, author, "Title NMock Test", "Content NMock Test");

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test of CreateComment method")]
        //public void CreationOfComment()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    var article = new Article(0);
        //    var user = new User(0);
        //    var comment = new Comment(10, "CommentTest", user, article);
        //    mockRepository.Expects.One.Method(r => r.Save(comment)).WithAnyArguments().Will();
        //    var facade = new Facade(mockRepository.MockObject);

        //    facade.CreateComment(10, article, user, "CommentTest");

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test of CreateReview method")]
        //public void CreationOfReview()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    var user = new User(0);
        //    var article = new Article(0);
        //    var rating = new Rating(7);
        //    var review = new Review(10, "Test Review", user, article, rating);
        //    mockRepository.Expects.One.Method(r => r.Save(review)).WithAnyArguments().Will();
        //    mockRepository.Expects.One.Method(r => r.Get<Review>()).WillReturn(new List<Review>() { review });
        //    var facade = new Facade(mockRepository.MockObject);

        //    facade.CreateReview(
        //        10,
        //        "Test Review",
        //        rating,
        //        user,
        //        article);

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test of CreateReviewText method")]
        //public void CreationOfReviewText()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    var user = new User(0);
        //    var article = new Article(0);
        //    var rating = new Rating(7);
        //    var review = new ReviewText(10, "Test Review", user, article, rating);
        //    mockRepository.Expects.One.Method(r => r.Save(review)).WithAnyArguments().Will();
        //    mockRepository.Expects.One.Method(r => r.Get<Review>()).WillReturn(new List<Review>() { review });
        //    var facade = new Facade(mockRepository.MockObject);

        //    facade.CreateReviewText(
        //        10,
        //        "Test Review Text",
        //        rating,
        //        user,
        //        article);

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test Delete method was called")]
        //public void DeleteArticle()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    var article = new Article(0);
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    mockRepository.Expects.One.Method(r => r.Delete(article)).WithAnyArguments().Will();
        //    var facade = new Facade(mockRepository.MockObject);

        //    facade.Delete(article);

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test FilterCommentsByArticle method: there are comments for article")]
        //public void FilterCommentsByArticle()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    var article = new Article(10);
        //    var user = new User(0);
        //    var comment0 = new Comment(0, "Comment 0", user, article);
        //    var comment1 = new Comment(1, "Comment 1", user, article);
        //    var comment2 = new Comment(2, "Comment 2", user, new Article(11));
        //    mockRepository.Stub.Out.GetProperty(r => r.Initialized).WillReturn(true);
        //    mockRepository.Stub.Out.Method(r => r.Get<BaseComment>())
        //        .WillReturn(new List<BaseComment>() { comment0, comment1, comment2 });
        //    var facade = new Facade(mockRepository.MockObject);

        //    var count = facade.FilterCommentsByArticle(article).OfType<Comment>().ToList().Count;

        //    Assert.AreEqual(2, count);
        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test FilterCommentsByArticle method: there are no comments for article")]
        //public void FilterCommentsByNonExistingArticle()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    var article = new Article(10);
        //    var user = new User(0);
        //    var comment0 = new Comment(0, "Comment 0", user, article);
        //    var comment1 = new Comment(1, "Comment 1", user, article);
        //    mockRepository.Stub.Out.GetProperty(r => r.Initialized).WillReturn(true);
        //    mockRepository.Stub.Out.Method(r => r.Get<BaseComment>())
        //        .WillReturn(new List<BaseComment>() { comment0, comment1 });
        //    var facade = new Facade(mockRepository.MockObject);

        //    var count = facade.FilterCommentsByArticle(new Article(11)).Count;

        //    Assert.AreEqual(0, count);
        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test GetById method")]
        //public void GetById()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Stub.Out.GetProperty(r => r.Initialized).WillReturn(true);
        //    var comment = new Comment(10, "content", new User(10), new Article(10));
        //    mockRepository.Stub.Out.Method(r => r.GetById<BaseComment>(10)).WithAnyArguments().WillReturn(comment);

        //    var facade = new Facade(mockRepository.MockObject);

        //    var commentId = facade.GetCommentById(10).Id;

        //    Assert.AreEqual(10, commentId);
        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test Get method: get last article")]
        //public void GetLastArticle()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Stub.Out.GetProperty(r => r.Initialized).WillReturn(true);
        //    var facade = new Facade(mockRepository.MockObject);
        //    var article = new Article(10);
        //    mockRepository.Stub.Out.Method(r => r.Get<Article>()).WillReturn(new List<Article>() { article });

        //    var expectedArticle = facade.Get<Article>().Last();

        //    Assert.AreEqual(10, expectedArticle.Id);
        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test GetRandom method")]
        //public void GetRandom()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Stub.Out.GetProperty(r => r.Initialized).WillReturn(true);
        //    var facade = new Facade(mockRepository.MockObject);
        //    var article0 = new Article(10);

        //    mockRepository.Expects.One.Method(r => r.GetRandom<Article>()).WillReturn(article0);
        //    var id0 = facade.GetRandomArticle().Id;

        //    Assert.AreEqual(id0, 10);
        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test Save method was called")]
        //public void SaveArticle()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    var facade = new Facade(mockRepository.MockObject);
        //    var article = new Article(8);
        //    mockRepository.Expects.One.Method(r => r.Save(article)).WithAnyArguments().Will();

        //    facade.Save(article);

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test Update method was called")]
        //public void UpdateArticle()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    var facade = new Facade(mockRepository.MockObject);
        //    var article = new Article(10);
        //    var newArticle = article;
        //    newArticle.Title = "Updated title";
        //    mockRepository.Expects.One.Method(r => r.Update(article, newArticle)).WithAnyArguments().Will();
           
        //    facade.UpdateArticle(article, newArticle);

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}

        //[TestMethod]
        //[Description("Test UpdateRating method: new rating value will replace existing value, if User has already created review")]
        //public void UpdateRating()
        //{
        //    var mocks = new MockFactory();
        //    var mockRepository = mocks.CreateMock<Repository>();
        //    mockRepository.Expects.One.GetProperty(r => r.Initialized).WillReturn(true);
        //    var facade = new Facade(mockRepository.MockObject);
        //    var user = new User(10);
        //    var article = new Article(11);
        //    var reivew = new Review(11, "Test Review", user, article, new Rating(1));
        //    mockRepository.Expects.One.Method(r => r.Get<Review>())
        //        .WillReturn(new List<Review> { new Review(10, "Content", user, new Article(10), new Rating(1)) });
        //    mockRepository.Expects.One.Method(r => r.Save(reivew)).WithAnyArguments().Will();            

        //    facade.CreateReview(
        //        11,
        //        "Test Review",
        //        new Rating(1),
        //        user,
        //        article);

        //    mocks.VerifyAllExpectationsHaveBeenMet();
        //}
    }
}
