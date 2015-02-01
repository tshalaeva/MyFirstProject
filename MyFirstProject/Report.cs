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

        public void PrintListOfCommentsForArticles(Repository repository)
        {
            List<Article> articles = repository.GetArticles();
            List<IComment> comments = repository.GetComments();
            Console.WriteLine("List of comments for each article:");
            List<IComment> articleComments;
            foreach (Article article in articles)
            {
                Console.WriteLine("Artcle " + article.Title + ": ");
                Console.WriteLine();
                articleComments = new List<IComment>();
                articleComments = facade.FilterCommentsByArticle(repository, article);
                foreach (IComment comment in articleComments)
                {
                    comment.Display();
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
