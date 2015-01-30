using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Report
    {
        private Facade facade = new Facade();
        public void PrintArticleTitles(Repository repository)
        {
            List<Article> articles = repository.GetArticles();
            for (int i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Title of article " + (i + 1).ToString() + ": " + articles[i].Title);
            }

            Console.WriteLine();
        }

        public void PrintAverageRatingForArticle(Repository repository)
        {
            List<Article> articles = repository.GetArticles();
            for (int i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Average rating of article" + (i + 1).ToString() + ": " + articles[i].GetAverageRating());
            }

            Console.WriteLine();
        }

        public void PrintListOfPrivilegies(Repository repository)
        {
            List<Admin> admins = repository.GetAdmins();
            for (int i = 0; i < admins.Count; i++)
            {
                Console.Write("List of " + admins[i].FirstName + " " + admins[i].LastName + " privilegies: ");
                for (int j = 0; j < admins[i].Privilegies.Count; j++)
                {
                    if (j == admins[i].Privilegies.Count - 1)
                    {
                        Console.WriteLine(admins[i].Privilegies[j]);
                        break;
                    }
                    Console.Write(admins[i].Privilegies[j] + ", ");
                }
            }
            Console.WriteLine();
        }

        public void PrintListOfCommentsForArticles(Repository repository)
        {
            List<Article> articles = repository.GetArticles();
            List<IComment> comments = repository.GetComments();
            Console.WriteLine("List of comments for each article:");
            List<IComment> articleComments;
            for (int i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Article " + (i + 1).ToString() + ": ");
                Console.WriteLine();
                articleComments = new List<IComment>();
                articleComments = facade.FilterCommentsByArticle(repository, articles[i]);
                for (int j = 0; j < articleComments.Count; j++)
                {
                    articleComments[j].Display();
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
