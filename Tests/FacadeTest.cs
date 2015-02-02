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
            Facade facade = new Facade(new FacadeRepository());            
            List<IComment> after = facade.FilterCommentsByArticle(new Article(0));
            Assert.AreEqual(2, after.Count);
        }

        [TestMethod]
        public void TestCommentsDoNotExist()
        {
            Facade facade = new Facade(new FacadeRepository());            
            List<IComment> after = facade.FilterCommentsByArticle(new Article(5));
            Assert.AreEqual(0, after.Count);
        }
    }
}
