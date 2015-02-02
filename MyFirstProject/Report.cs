using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Report
    {
        private Facade facade = new Facade(new Repository());
        
        public void PrintArticleTitles()
        {
            List<Article> articles = facade.Repository.GetArticles();
            for (int i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Title of article " + (i + 1).ToString() + ": " + articles[i].Title);
            }

            Console.WriteLine();
        }

        public void PrintAverageRatingForArticle()
        {
            List<Article> articles = facade.Repository.GetArticles();
            for (int i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Average rating of article" + (i + 1).ToString() + ": " + articles[i].GetAverageRating());
            }

            Console.WriteLine();
        }

        public void PrintListOfPrivilegies()
        {
            List<Admin> admins = facade.Repository.GetAdmins();
            foreach (Admin admin in admins)
            {
                Console.Write("List of " + admin.FirstName + " " + admin.LastName + " privilegies: ");
                foreach(string privilegy in admin.Privilegies)
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
            List<Article> articles = facade.Repository.GetArticles();
            List<BaseComment> comments = facade.Repository.GetComments();
            Console.WriteLine("List of comments for each article:");
            List<BaseComment> articleComments;
            foreach (Article article in articles)
            {
                Console.WriteLine("Artcle " + article.Title + ": ");
                Console.WriteLine();
                articleComments = new List<BaseComment>();
                articleComments = facade.FilterCommentsByArticle(article);
                foreach (BaseComment comment in articleComments)
                {
                    comment.Display();
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
