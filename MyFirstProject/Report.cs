using System;
using System.Linq;
using Infrastructure;
//using DataAccessLayer.Repositories;
using ObjectRepository.Entities;

namespace MyFirstProject
{
    public class Report
    {
        private readonly Facade _mFacade;

        public Report()
        {
            _mFacade = IocContainer.Container.GetInstance<Facade>();
        }

        public void PrintArticleTitles()
        {
            var articles = _mFacade.GetArticles();

            foreach (var article in articles)
            {
                Console.WriteLine("Title of article {0}: {1}", article.Id, article.Title);
            }

            Console.WriteLine();
        }

        public void PrintAverageRatingForArticle()
        {
            var articles = _mFacade.GetArticles();
            foreach (var article in articles)
            {
                Console.WriteLine("Average rating of article {0}: {1}", article.Id, article.GetAverageRating());
            }

            Console.WriteLine();
        }

        public void PrintListOfPrivilegies()
        {
            var admins = _mFacade.GetAdmins();
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

                    Console.Write("{0}, ", privilegy);
                }
            }

            Console.WriteLine();
        }

        public void PrintListOfCommentsForArticles()
        {
            var articles = _mFacade.GetArticles();
            Console.WriteLine("List of comments for each article:");
            foreach (var article in articles)
            {
                Console.WriteLine("Article {0}: ", article.Title);
                Console.WriteLine();
                var articleComments = _mFacade.FilterCommentsByArticle(article);
                foreach (var comment in articleComments)
                {
                    Console.WriteLine(comment.ToString());
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        public void CreateAticles()
        {
            _mFacade.CreateArticle(5, _mFacade.GetAuthors().First(), "Test 0", "Text 0");
            _mFacade.CreateArticle(6, _mFacade.GetAuthors().First(), "Test 1", "Text 1");
        }

        public void CreateComments()
        {
            _mFacade.CreateComment(1, _mFacade.GetArticles()[1], _mFacade.GetAllUsers()[2], "Test comment 0");
            _mFacade.CreateComment(2, _mFacade.GetArticles()[1], _mFacade.GetAllUsers()[1], "Test comment 1");
            _mFacade.CreateComment(3, _mFacade.GetArticles()[1], _mFacade.GetAllUsers()[0], "Test comment 2");
        }

        public void CreateReviews()
        {
            _mFacade.CreateReview(
                1,
                "Test Review 0",
                new Rating(3),
                _mFacade.GetAllUsers()[1],
                _mFacade.GetArticles().First());

            _mFacade.CreateReview(
                2,
                "Test Review 1",
                new Rating(6),
                _mFacade.GetAllUsers().First(),
                _mFacade.GetArticles()[1]);

            _mFacade.CreateReview(
                3,
                "Test Review 2",
                new Rating(-1),
                _mFacade.GetAllUsers()[2],
                _mFacade.GetArticles()[1]);

            _mFacade.CreateReview(
                4,
                "Test Review 3",
                new Rating(1),
                _mFacade.GetAllUsers().First(),
                _mFacade.GetArticles()[2]);

            _mFacade.CreateReviewText(
                5,
                "Test Review Text 0",
                new Rating(4),
                _mFacade.GetAllUsers()[2],
                _mFacade.GetArticles()[3]);

            _mFacade.CreateReviewText(
                6,
                "Test Review Text 1",
                new Rating(3),
                _mFacade.GetAllUsers()[2],
                _mFacade.GetArticles()[5]);
        }

        public void PrintEntityCodeForEachComment()
        {
            Console.WriteLine("Entity Codes:");

            Console.WriteLine();

            foreach (var comment in _mFacade.GetComments())
            {
                Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                Console.WriteLine();
            }

            var reviews = _mFacade.GetReviews();
            foreach (var comment in reviews.Where(comment => !(comment is ReviewText)))
            {
                Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                Console.WriteLine();
            }

            foreach (var comment in _mFacade.GetReviewTexts())
            {
                Console.WriteLine("{0}: {1}", comment.Content, comment.GetEntityCode());

                Console.WriteLine();
            }
        }

        public void PrintRandomArticleId()
        {
            Console.WriteLine("Random article id = {0}", _mFacade.GetRandomArticle().Id);
        }
    }
}
