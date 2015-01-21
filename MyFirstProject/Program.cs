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
            Author[] authorUser = {new Author(1, "Author", "1", 50, "Author1", 3.4), new Author(2, "Author", "2", 50, "Author2", 5.0), new Author(3, "Author", "3", 52, "Author3", 4.5)};
            Admin adminUser = new Admin(4, "Admin", "User", 58, new string[]{"edit", "read", "delete"});
            User[] user = {new User(5, "User", "1", 80), new User(6, "User", "2", 25), new User(7, "User", "3", 38)};
            Article[] article = {new Article(1, authorUser[0], "Title 1", "Text 1"), new Article(2, authorUser[1], "Title 2", "Text 2"),
                                       new Article(3, authorUser[1], "Title 3", "Text 3"), new Article(4, authorUser[2], "Title 4", "Text 4")};
            
            article[0].addComment(new Comment(1, "Comment 1", user[1]));
            article[1].addComment(new Comment(2, "Comment 2", user[0]));
            article[1].addComment(new Comment(2, "Comment 3", user[2]));
            article[2].addComment(new Comment(3, "Comment 4", user[0]));
            article[3].addComment(new Comment(4, "Comment 5", user[2]));

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
                Console.WriteLine("Title of article " + (i + 1).ToString() + ": " + article[i].getTitle());
            }

            Console.WriteLine();

            for (int i = 0; i < article.Length; i++)
            {
                Console.WriteLine("Average rating of article" + (i + 1).ToString() + ": " + article[i].getAverageRating());
            }

            Console.WriteLine();

            Console.Write("List of admin privilegies: ");
            for (int i = 0; i < adminUser.getPrivilegies().Length; i++)
            {
                if (i == adminUser.getPrivilegies().Length - 1)
                {
                    Console.WriteLine(adminUser.getPrivilegies()[i]);
                    break;
                }
                Console.Write(adminUser.getPrivilegies()[i] + ", ");
            }

            Console.WriteLine();

            Console.WriteLine("List of comments for each article:");
            for (int i = 0; i < article.Length; i++)
            {
                Console.Write("Article " + (i + 1).ToString() + ": ");
                for (int j = 0; j < article[i].getComments().Count; j++)
                {
                    if (j == article[i].getComments().Count - 1)
                    {
                        Console.WriteLine(article[i].getComments()[j].getContent());
                        break;
                    }
                    Console.Write(article[i].getComments()[j].getContent() + ", ");
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
