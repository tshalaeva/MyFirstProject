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
    }
}
