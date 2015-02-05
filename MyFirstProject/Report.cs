using System;
using System.Linq;
using MyFirstProject.Entity;
using MyFirstProject.Repository;

namespace MyFirstProject
{
    public class Report
    {
        private readonly Facade<Repository.Repository> m_facade = new Facade<Repository.Repository>(new Repository.Repository());
        
        public void PrintArticleTitles()
        {
            var articles = ((IRepository<Article>)m_facade.Repository).Get();
            for (var i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Title of article " + (i + 1) + ": " + articles[i].Title);
            }

            Console.WriteLine();
        }

        public void PrintAverageRatingForArticle()
        {
            var articles = ((IRepository<Article>)m_facade.Repository).Get();
            for (var i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Average rating of article" + (i + 1) + ": " + articles[i].GetAverageRating());
            }

            Console.WriteLine();
        }

        public void PrintListOfPrivilegies()
        {
            var admins = ((IRepository<Admin>)m_facade.Repository).Get();
            foreach (var admin in admins)
            {
                Console.Write("List of " + admin.FirstName + " " + admin.LastName + " privilegies: ");
                foreach (var privilegy in admin.Privilegies)
                {
                    if (privilegy == admin.Privilegies.Last())
                    {
                        Console.WriteLine(privilegy);
                        break;
                    }

                    Console.Write(privilegy + ", ");
                }
            }

            Console.WriteLine();
        }

        public void PrintListOfCommentsForArticles()
        {
            var articles = ((IRepository<Article>)m_facade.Repository).Get();            
            Console.WriteLine("List of comments for each article:");
            foreach (var article in articles)
            {
                Console.WriteLine("Artcle " + article.Title + ": ");
                Console.WriteLine();
                var articleComments = m_facade.FilterCommentsByArticle(article);
                foreach (var comment in articleComments)
                {
                    comment.Display();
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }
    }
}
