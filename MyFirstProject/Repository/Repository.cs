using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entity;

namespace MyFirstProject.Repository
{
    public class Repository : IRepository
    {
        private readonly List<Article> m_articles;
        private readonly List<User> m_users;
        private readonly List<Admin> m_admins;
        private readonly List<Author> m_authors;
        private readonly List<BaseComment> m_comments;        

        public Repository()
        {
            m_articles = new List<Article>();
            m_users = new List<User>();
            m_admins = new List<Admin>();
            m_authors = new List<Author>();
            m_comments = new List<BaseComment>();            
        }

        public void SaveArticle(Article article)
        {
            m_articles.Add(article);
        }

        public void SaveUser(User user)
        {
            m_users.Add(user);
        }

        public void SaveAdmin(Admin admin)
        {
            m_admins.Add(admin);
        }

        public void SaveAuthor(Author author)
        {
            m_authors.Add(author);
        }

        public void SaveComment(Comment comment)
        {
            m_comments.Add(comment);
        }

        public void SaveReview(Review review)
        {
            m_comments.Add(review);
        }

        public List<Article> GetArticles()
        {
            return m_articles;
        }

        public List<User> GetUsers()
        {
            return m_users;
        }

        public List<Admin> GetAdmins()
        {
            return m_admins;
        }

        public List<Author> GetAuthors()
        {
            return m_authors;
        }

        public List<BaseComment> GetComments()
        {
            return m_comments;
        }

        public void DeleteArticle(Article article)
        {
            m_articles.Remove(article);
        }

        public void DeleteUser(User user)
        {
            m_users.Remove(user);
        }

        public void DeleteAdmin(Admin admin)
        {
            m_admins.Remove(admin);
        }

        public void DeleteAuthor(Author author)
        {
            m_authors.Remove(author);
        }

        public void DeleteComment(BaseComment comment)
        {
            m_comments.Remove(comment);
        }

        public void Initialize()
        {
            for (var i = 0; i < 3; i++)
            {
                m_authors.Add(new Author(i + 1));
            }

            for (var i = 0; i < m_authors.Count; i++)
            {
                m_authors[i].FirstName = "Author";
                m_authors[i].LastName = (i + 1).ToString();
                m_authors[i].Age = 50 + i;
                m_authors[i].NickName = "Author" + (i + 1).ToString();
                m_authors[i].Popularity = i + 0.5;
            }

            m_admins.Add(new Admin(4));
            m_admins[0].FirstName = "Admin";
            m_admins[0].LastName = "User";
            m_admins[0].Age = 58;
            m_admins[0].Privilegies = new List<string> { "edit", "read", "delete" };

            for (var i = 0; i < 3; i++)
            {
                m_users.Add(new User(i + 5));
            }

            for (var i = 0; i < m_users.Count; i++)
            {
                m_users[i].FirstName = "User";
                m_users[i].LastName = (i + 1).ToString();
                m_users[i].Age = 30 + i;
            }

            for (var i = 0; i < 4; i++)
            {
                m_articles.Add(new Article(i + 1));
            }

            for (var i = 0; i < m_articles.Count; i++)
            {
                m_articles[i].Content = "Text" + (i + 1).ToString();
                m_articles[i].Title = "Title" + (i + 1).ToString();
                if (i == m_articles.Count - 1)
                {
                    m_articles[i].Author = m_authors[0];
                    break;
                }

                m_articles[i].Author = m_authors[i];
            }

            m_comments.Add(CreateReview(1, "Review 1", new Rating(3), m_users[1], m_articles[0]));
            m_comments.Add(new Comment(2, "Comment 1", m_users[2], m_articles[1]));
            m_comments.Add(CreateReview(2, "Review 2", new Rating(6), m_users[0], m_articles[1]));
            m_comments.Add(CreateReview(3, "Review 3", new Rating(-1), m_users[2], m_articles[1]));
            m_comments.Add(CreateReview(4, "Review 4", new Rating(1), m_users[0], m_articles[2]));
            m_comments.Add(CreateReviewText(5, "Review Text 5", new Rating(0), m_users[2], m_articles[3]));
        }

        private void UpdateRating(Rating rating, Article article, User user)
        {
            var flag = false;
            var reviews = (from comment in GetComments()
                           where comment is Review
                           select comment).ToList();
            foreach (Review mreview in reviews)
            {
                if (mreview.Article == article)
                {
                    foreach (var mrating in article.Rating)
                    {
                        if (mreview.User.Id == user.Id)
                        {
                            mrating.SetRating(rating.Value);
                            flag = true;
                            break;
                        }
                    }
                }
            }

            if (flag == false)
            {
                article.AddRating(rating);
            }
        }

        private Review CreateReview(int id, string content, Rating rating, User user, Article article)
        {
            var review = new Review(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            return review;
        }

        private ReviewText CreateReviewText(int id, string content, Rating rating, User user, Article article)
        {
            var review = new ReviewText(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            return review;
        }
    }
}
