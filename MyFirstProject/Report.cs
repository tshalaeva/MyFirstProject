using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;
using MyFirstProject.Repository;
using StructureMap;
using StructureMap.Configuration;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace MyFirstProject
{
    public class Report
    {                
        private readonly Facade m_facade;

        public Report()
        {           
            m_facade = IocContainer.Container.GetInstance<Facade>();
        }

        public void PrintArticleTitles()
        {
            var articles = m_facade.Get<Article>();

            foreach (var article in articles)
            {
                Console.WriteLine("Title of article {0}: {1}", article.Id, article.Title);
            }

            Console.WriteLine();
        }

        public void PrintAverageRatingForArticle()
        {
            var articles = m_facade.Get<Article>();
            foreach (var article in articles)
            {
                Console.WriteLine("Average rating of article {0}: {1}", article.Id, article.GetAverageRating());
            }

            Console.WriteLine();
        }

        public void PrintListOfPrivilegies()
        {
            var admins = m_facade.Get<Admin>();
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
            var articles = m_facade.Get<Article>();
            Console.WriteLine("List of comments for each article:");
            foreach (var article in articles)
            {
                Console.WriteLine("Article {0}: ", article.Title);
                Console.WriteLine();
                var articleComments = m_facade.FilterCommentsByArticle<Comment>(article);
                var articleReviews = m_facade.FilterCommentsByArticle<Review>(article);                
                var articleReviewTexts = m_facade.FilterCommentsByArticle<ReviewText>(article);
                var allArticleComments = new List<BaseComment>(articleComments.Count + articleReviews.Count + articleReviewTexts.Count);
                allArticleComments.AddRange(articleComments);
                allArticleComments.AddRange(articleReviews);
                allArticleComments.AddRange(articleReviewTexts);
                foreach (var comment in allArticleComments)
                {
                    Console.WriteLine(comment.ToString());
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        public void CreateAticles()
        {
            m_facade.CreateArticle(5, m_facade.Get<Author>().First(), "Test 0", "Text 0");
            m_facade.CreateArticle(6, m_facade.Get<Author>().First(), "Test 1", "Text 1");
        }

        public void CreateComments()
        {
            m_facade.CreateComment(1, m_facade.Get<Article>()[1], m_facade.Get<User>()[2], "Test comment 0");
            m_facade.CreateComment(2, m_facade.Get<Article>()[1], m_facade.Get<User>()[1], "Test comment 1");
            m_facade.CreateComment(3, m_facade.Get<Article>()[1], m_facade.Get<User>()[0], "Test comment 2");
        }

        public void CreateReviews()
        {
            m_facade.CreateReview(
                1,
                "Test Review 0",
                new Rating(3),
                m_facade.Get<User>()[1],
                m_facade.Get<Article>().First());

            m_facade.CreateReview(
                2,
                "Test Review 1",
                new Rating(6),
                m_facade.Get<User>().First(),
                m_facade.Get<Article>()[1]);

            m_facade.CreateReview(
                3,
                "Test Review 2",
                new Rating(-1),
                m_facade.Get<User>()[2],
                m_facade.Get<Article>()[1]);

            m_facade.CreateReview(
                4,
                "Test Review 3",
                new Rating(1),
                m_facade.Get<User>().First(),
                m_facade.Get<Article>()[2]);

            m_facade.CreateReviewText(
                5,
                "Test Review Text 0",
                new Rating(4),
                m_facade.Get<User>()[2],
                m_facade.Get<Article>()[3]);

            m_facade.CreateReviewText(
                6,
                "Test Review Text 1",
                new Rating(3),
                m_facade.Get<User>()[2],
                m_facade.Get<Article>()[5]);
        }

        public void PrintEntityCodeForEachComment()
        {
            Console.WriteLine("Entity Codes:");

            Console.WriteLine();

            foreach (var comment in m_facade.Get<Comment>())
            {
                Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                Console.WriteLine();
            }

            var reviews = m_facade.Get<Review>();
            foreach (var comment in reviews)
            {
                if (!(comment is ReviewText))
                {
                    Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                    Console.WriteLine();
                }
            }

            foreach (var comment in m_facade.Get<ReviewText>())
            {
                Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                Console.WriteLine();
            }
        }

        public void PrintRandomArticleId()
        {
            Console.WriteLine("Random article id = {0}", m_facade.GetRandom<Article>().Id);
        }
    }
}
