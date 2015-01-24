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

            Author[] authorUser = {new Author(1), new Author(2), new Author(3)};
            for (int i = 0; i < authorUser.Length; i++)
            {
                authorUser[i].FirstName = "Author";
                authorUser[i].LastName = (i + 1).ToString();
                authorUser[i].Age = 50 + i;
                authorUser[i].NickName = "Author" + (i + 1).ToString();
                authorUser[i].Popularity = i + 0.5;
            }

            Admin adminUser = new Admin(4);
            adminUser.FirstName = "Admin";
            adminUser.LastName = "User";
            adminUser.Age = 58;
            adminUser.Privilegies = new List<string>{"edit", "read", "delete"};

            User[] user = {new User(5), new User(6), new User(7)};
            for (int i = 0; i < user.Length; i++)
            {
                user[i].FirstName = "User";
                user[i].LastName = (i + 1).ToString();
                user[i].Age = 30 + i;                
            }

            Article[] article = {new Article(1), new Article(2), new Article(3), new Article(4)};
            for (int i = 0; i < article.Length; i++)
            {
                article[i].Content = "Text" + (i + 1).ToString();
                article[i].Title = "Title" + (i + 1).ToString();                
                if (i == article.Length - 1)
                {
                    article[i].Author = authorUser[0];
                    break;
                }
                article[i].Author = authorUser[i];                
            }

            List<Comment> comments = new List<Comment>();
            comments.Add(article[0].addComment(1, "Comment 1", user[1]));
            comments.Add(article[1].addComment(2, "Comment 2", user[0]));
            comments.Add(article[1].addComment(2, "Comment 3", user[2]));
            comments.Add(article[2].addComment(3, "Comment 4", user[0]));
            comments.Add(article[3].addComment(4, "Comment 5", user[2]));

            article[0].setRating(new Rating(3, user[0]));
            article[0].setRating(new Rating(3, user[1]));
            article[0].setRating(new Rating(2, user[2]));
            article[0].setRating(new Rating(5, authorUser[0]));

            article[1].setRating(new Rating(2, user[0]));
            article[1].setRating(new Rating(1, user[1]));
            article[1].setRating(new Rating(5, user[2]));
            article[1].setRating(new Rating(5, authorUser[0]));
            article[1].setRating(new Rating(3, adminUser));

            article[2].setRating(new Rating(1, user[0]));
            article[2].setRating(new Rating(2, user[1]));
            article[2].setRating(new Rating(3, user[2]));
            article[2].setRating(new Rating(4, adminUser));

            article[3].setRating(new Rating(1, user[0]));
            article[3].setRating(new Rating(5, user[0]));
            article[3].setRating(new Rating(3, user[1]));
            article[3].setRating(new Rating(5, user[2]));

            for (int i = 0; i < article.Length; i++)
            {
                Console.WriteLine("Title of article " + (i + 1).ToString() + ": " + article[i].Title);
            }

            Console.WriteLine();

            for (int i = 0; i < article.Length; i++)
            {
                Console.WriteLine("Average rating of article" + (i + 1).ToString() + ": " + article[i].getAverageRating());
            }

            Console.WriteLine();

            Console.Write("List of admin privilegies: ");
            for (int i = 0; i < adminUser.Privilegies.Count; i++)
            {
                if (i == adminUser.Privilegies.Count - 1)
                {
                    Console.WriteLine(adminUser.Privilegies[i]);
                    break;
                }
                Console.Write(adminUser.Privilegies[i] + ", ");
            }

            Console.WriteLine();

            Console.WriteLine("List of comments for each article:");
            List<Comment> articleComments;
            for (int i = 0; i < article.Length; i++)
            {
                Console.Write("Article " + (i + 1).ToString() + ": ");
                articleComments = new List<Comment>();
                articleComments = report.filterCommentsByArticle(comments, article[i]);
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

            Console.ReadLine();
        }
    }
}
