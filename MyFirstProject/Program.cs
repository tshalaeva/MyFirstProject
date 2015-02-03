using System;

namespace MyFirstProject
{
    public class Program
    {
        private static void Main()
        {
            var report = new Report();
            var repository = new Repository.Repository();
            repository.Initialize();

            report.PrintArticleTitles();

            report.PrintAverageRatingForArticle();

            report.PrintListOfPrivilegies();

            report.PrintListOfCommentsForArticles();

            Console.ReadLine();
        }
    }
}
