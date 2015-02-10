using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Report
    {        
        private readonly Facade<Article> m_articleFacade = new Facade<Article>(new Repository.Repository());
        private readonly Facade<Admin> m_adminFacade = new Facade<Admin>(new Repository.Repository());
        private readonly Facade<Author> m_authorFacade = new Facade<Author>(new Repository.Repository());
        private readonly Facade<Comment> m_commentFacade = new Facade<Comment>(new Repository.Repository());
        private readonly Facade<ReviewText> m_reviewTextFacade = new Facade<ReviewText>(new Repository.Repository());
        private readonly Facade<Review> m_reviewFacade = new Facade<Review>(new Repository.Repository());
        private readonly Facade<User> m_userFacade = new Facade<User>(new Repository.Repository());

        public void PrintArticleTitles()
        {
            var articles = m_articleFacade.Get();
            for (var i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Title of article {0}: {1}", i + 1, articles[i].Title);
            }

            Console.WriteLine();
        }

        public void PrintAverageRatingForArticle()
        {
            var articles = m_articleFacade.Get();
            for (var i = 0; i < articles.Count; i++)
            {
                Console.WriteLine("Average rating of article {0}: {1}", i + 1, articles[i].GetAverageRating());
            }

            Console.WriteLine();
        }

        public void PrintListOfPrivilegies()
        {
            var admins = m_adminFacade.Get();
            foreach (var admin in admins)
            {
                Console.Write("List of {0} {1} privilegies: ", admin.FirstName, admin.LastName);
                foreach (var privilegy in admin.Privilegies)
                {
                    if (privilegy == admin.Privilegies.Last())
                    {
                        Console.WriteLine(privilegy);
                        break;
                    }

                    Console.Write(string.Format("{0}, ", privilegy));
                }
            }

            Console.WriteLine();
        }

        public void PrintListOfCommentsForArticles()
        {
            var articles = m_articleFacade.Get();
            Console.WriteLine("List of comments for each article:");
            foreach (var article in articles)
            {
                Console.WriteLine("Article {0}: ", article.Title);
                Console.WriteLine();
                var articleComments = m_commentFacade.FilterCommentsByArticle(article);
                var articleReviews = m_reviewFacade.FilterCommentsByArticle(article);
                var articleReviewTexts = m_reviewTextFacade.FilterCommentsByArticle(article);
                var allArticleComments = new List<BaseComment>(articleComments.Count + articleReviews.Count + articleReviewTexts.Count);
                allArticleComments.AddRange(articleComments);
                allArticleComments.AddRange(articleReviews);
                allArticleComments.AddRange(articleReviewTexts);
                foreach (var comment in allArticleComments)
                {
                    comment.Display();
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        public void CreateAticles()
        {
            m_articleFacade.CreateArticle(5, m_authorFacade.Get()[0], "Test 0", "Text 0");
            m_articleFacade.CreateArticle(6, m_authorFacade.Get()[0], "Test 1", "Text 1");
        }

        public void CreateComments()
        {
            m_commentFacade.CreateComment(1, m_articleFacade.Get()[1], m_userFacade.Get()[2], "Test comment 0");
            m_commentFacade.CreateComment(2, m_articleFacade.Get()[1], m_userFacade.Get()[1], "Test comment 1");
            m_commentFacade.CreateComment(3, m_articleFacade.Get()[1], m_userFacade.Get()[0], "Test comment 2");
        }

        public void CreateReviews()
        {
            m_reviewFacade.CreateReview(
                1,
                "Test Review 0",
                new Rating(3),
                m_userFacade.Get()[1],
                m_articleFacade.Get()[0]);

            m_reviewFacade.CreateReview(
                2,
                "Test Review 1",
                new Rating(6),
                m_userFacade.Get()[0],
                m_articleFacade.Get()[1]);

            m_reviewFacade.CreateReview(
                3,
                "Test Review 2",
                new Rating(-1),
                m_userFacade.Get()[2],
                m_articleFacade.Get()[1]);

            m_reviewFacade.CreateReview(
                4,
                "Test Review 3",
                new Rating(1),
                m_userFacade.Get()[0],
                m_articleFacade.Get()[2]);

            m_reviewTextFacade.CreateReviewText(
                5,
                "Test Review Text 0",
                new Rating(4),
                m_userFacade.Get()[2],
                m_articleFacade.Get()[3]);

            m_reviewTextFacade.CreateReviewText(
                6,
                "Test Review Text 1",
                new Rating(3),
                m_userFacade.Get()[2],
                m_articleFacade.Get()[5]);
        }

        public void PrintEntityCodeForEachComment()
        {
            Console.WriteLine("Entity Codes:");

            Console.WriteLine();

            foreach (var comment in m_commentFacade.Get())
            {
                Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                Console.WriteLine();
            }

            foreach (var comment in m_reviewFacade.Get())
            {
                Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                Console.WriteLine();
            }

            foreach (var comment in m_reviewTextFacade.Get())
            {
                Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                Console.WriteLine();
            }
        }
    }
}
