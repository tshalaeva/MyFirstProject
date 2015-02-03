using System;
using System.Linq;

namespace MyFirstProject
{
    public class Report
    {
        private readonly Facade m_facade = new Facade(new Repository.Repository());
        
        public void PrintArticleTitles()
        {
            var articles = m_facade.Repository.GetArticles();
            for (var i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Title of article " + (i + 1) + ": " + articles[i].Title);
            }

            Console.WriteLine();
        }

        public void PrintAverageRatingForArticle()
        {
            var articles = m_facade.Repository.GetArticles();
            for (var i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Average rating of article" + (i + 1) + ": " + articles[i].GetAverageRating());
            }

            Console.WriteLine();
        }

        public void PrintListOfPrivilegies()
        {
            var admins = m_facade.Repository.GetAdmins();
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
            var articles = m_facade.Repository.GetArticles();
            m_facade.Repository.GetComments();
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
