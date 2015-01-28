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
            List<Comment> before = new List<Comment>();
            for(int i = 0; i < 5; i++)
            {
                if(i == 4)
                {
                    before.Add(new Comment(i));
                    before[i].Article = new Article(0);
                    break;
                }
                before.Add(new Comment(i));
                before[i].Article = new Article(i);
            }
            List<Comment> after = facade.filterCommentsByArticle(before, new Article(0));
            Assert.AreEqual(2, after.Count);
        }

        [TestMethod]
        public void Test2()
        {
            Facade facade = new Facade();
            List<Comment> before = new List<Comment>();
            for (int i = 0; i < 5; i++)
            {                
                before.Add(new Comment(i));
                before[i].Article = new Article(i);
            }
            List<Comment> after = facade.filterCommentsByArticle(before, new Article(5));
            Assert.AreEqual(0, after.Count);
        }
    }
}
