using System;

namespace MyFirstProject
{
    public class Program
    {
        private static void Main()
        {
            var report = new Report();            

            report.PrintArticleTitles();

            report.PrintAverageRatingForArticle();

            report.PrintListOfPrivilegies();

            report.PrintListOfCommentsForArticles();

            Console.ReadLine();
        }
    }
}
