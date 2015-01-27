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

            report.printArticleTitles(repository.getArticles());

            report.printAverageRatingForArticle(repository.getArticles());

            report.printListOfPrivilegies(repository.getAdmins());

            report.printListOfCommentsForArticle(repository.getComments(), repository.getArticles());

            Console.ReadLine();
        }
    }
}
