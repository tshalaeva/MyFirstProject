using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Report report = new Report();
            Repository repository = new Repository();
            repository.Initialize();

            report.PrintArticleTitles(repository);

            report.PrintAverageRatingForArticle(repository);

            report.PrintListOfPrivilegies(repository);

            report.PrintListOfCommentsForArticles(repository);

            Console.ReadLine();
        }
    }
}
