using System;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Report
    {        
        private readonly Facade<Article> m_articleFacade = new Facade<Article>();        
        private readonly Facade<Admin> m_adminFacade = new Facade<Admin>(); 
        private readonly Facade<Author> m_authorFacade = new Facade<Author>(); 
        private readonly Facade<BaseComment> m_commentFacade = new Facade<BaseComment>(); 
        private readonly Facade<User> m_userFacade = new Facade<User>(); 
        
        public void PrintArticleTitles()
        {            
            var articles = m_articleFacade.Get();
            for (var i = 0; i < articles.Count; i++)
            {
                Console.WriteLine(string.Format("Title of article {0}: {1}", i + 1, articles[i].Title));                
            }

            Console.WriteLine();
        }

        public void PrintAverageRatingForArticle()
        {            
            var articles = m_articleFacade.Get();
            for (var i = 0; i < articles.Count; i++)
            {
                Console.WriteLine(string.Format("Average rating of article {0}: {1}",i + 1, articles[i].GetAverageRating()));
            }

            Console.WriteLine();
        }

        public void PrintListOfPrivilegies()
        {            
            var admins = m_adminFacade.Get();
            foreach (var admin in admins)
            {
                Console.Write(string.Format("List of {0} {1} privilegies: ", admin.FirstName, admin.LastName));
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
                Console.WriteLine(string.Format("Artcle {0}: ", article.Title));
                Console.WriteLine();
                var articleComments = m_commentFacade.FilterCommentsByArticle(article);
                foreach (var comment in articleComments)
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
            m_commentFacade.CreateComment(2, m_articleFacade.Get()[1], m_userFacade.Get()[2], "Test comment 0");
            //m_commentFacade.CreateComment(1, m_articleFacade.Get()[1], m_userFacade.Get()[1], "Test comment 1");
            //m_commentFacade.CreateComment(2, m_articleFacade.Get()[1], m_userFacade.Get()[0], "Test comment 2");
        }

        public void CreateReviews()
        {
            m_commentFacade.CreateReview(1, "Test Review 0", new Rating(3), m_userFacade.Get()[1], m_articleFacade.Get()[0]);
            m_commentFacade.CreateReview(2, "Test Review 1", new Rating(6), m_userFacade.Get()[0],
                m_articleFacade.Get()[1]);
            m_commentFacade.CreateReview(3, "Test Review 2", new Rating(-1), m_userFacade.Get()[2],
                m_articleFacade.Get()[1]);
            m_commentFacade.CreateReview(4, "Test Review 3", new Rating(1), m_userFacade.Get()[0],
                m_articleFacade.Get()[2]);
            m_commentFacade.CreateReviewText(5, "Test Review Text 0", new Rating(4), m_userFacade.Get()[2],
                m_articleFacade.Get()[3]);
            m_commentFacade.CreateReviewText(6, "Test Review Text 1", new Rating(3), m_userFacade.Get()[2],
                m_articleFacade.Get()[5]);
        }     
    }
}
