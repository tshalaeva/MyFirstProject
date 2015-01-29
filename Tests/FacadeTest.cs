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
        public void Test1()
        {
            Facade facade = new Facade();
            FacadeRepository repository = new FacadeRepository();
            repository.Initialize();
            List<Comment> after = facade.filterCommentsByArticle(repository.getComments(), new Article(0));
            Assert.AreEqual(2, after.Count);
        }

        [TestMethod]
        public void Test2()
        {
            FacadeRepository repository = new FacadeRepository();
            Facade facade = new Facade();            
            List<Comment> after = facade.filterCommentsByArticle(repository.getComments(), new Article(5));
            Assert.AreEqual(0, after.Count);
        }
    }
}
