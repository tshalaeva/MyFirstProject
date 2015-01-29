using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFirstProject;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class FacadeTest
    {
        [TestMethod]
        public void TestCommentsExist()
        {
            Facade facade = new Facade();
            IRepository repository = new FacadeRepository();
            repository.Initialize();
            List<Comment> after = facade.FilterCommentsByArticle(repository, new Article(0));
            Assert.AreEqual(2, after.Count);
        }

        [TestMethod]
        public void TestCommentsDoNotExist()
        {
            FacadeRepository repository = new FacadeRepository();
            Facade facade = new Facade();            
            List<Comment> after = facade.FilterCommentsByArticle(repository, new Article(5));
            Assert.AreEqual(0, after.Count);
        }
    }
}
