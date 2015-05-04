using System;
using System.IO;

namespace MyFirstProject
{
    public class Program
    {
        private static void Main()
        {
            var baseProjectPath = AppDomain.CurrentDomain.BaseDirectory;
            var appDataPath = Path.Combine(baseProjectPath, ".\\..\\..\\..\\App_Data\\");
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(appDataPath));

            var report = new Report();

            report.CreateAticles();

            report.CreateComments();

            //report.CreateReviews();

            //report.PrintArticleTitles();

            //report.PrintAverageRatingForArticle();

            //report.PrintListOfPrivilegies();

            //report.PrintListOfCommentsForArticles();

            //report.PrintEntityCodeForEachComment();

            //report.PrintRandomArticleId();

            Console.ReadLine();
        }
    }
}
