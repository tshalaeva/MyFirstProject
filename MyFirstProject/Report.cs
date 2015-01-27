using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Report
    {
        public List<Comment> filterCommentsByArticle(List<Comment> comments, Article article)
        {
            List<Comment> result = new List<Comment>();
            for (int i = 0; i < comments.Count; i++)
            {
                if (comments[i].Article.Id == article.Id)
                {
                    result.Add(comments[i]);
                }
            }
            return result;
        }

        public void printArticleTitles(List<Article> articles)
        {
            for (int i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Title of article " + (i + 1).ToString() + ": " + articles[i].Title);
            }

            Console.WriteLine();
        }

        public void printAverageRatingForArticle(List<Article> articles)
        {
            for (int i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Average rating of article" + (i + 1).ToString() + ": " + articles[i].getAverageRating());
            }

            Console.WriteLine();
        }

        public void printListOfPrivilegies(List<Admin> admins)
        {
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

        public void printListOfCommentsForArticle(List<Comment> comments, List<Article> articles)
        {
            Console.WriteLine("List of comments for each article:");
            List<Comment> articleComments;
            for (int i = 0; i < articles.Count; i++)
            {
                Console.Write("Article " + (i + 1).ToString() + ": ");
                articleComments = new List<Comment>();
                articleComments = filterCommentsByArticle(comments, articles[i]);
                for (int j = 0; j < articleComments.Count; j++)
                {
                    if (j == articleComments.Count - 1)
                    {
                        Console.WriteLine(articleComments[j].Content);
                        articleComments = null;
                        break;
                    }
                    Console.Write(articleComments[j].Content + ", ");
                }
                Console.WriteLine();
            }
        }
    }
}
