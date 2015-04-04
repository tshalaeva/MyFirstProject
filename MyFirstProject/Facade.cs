using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Facade
    {
        private readonly Repository.IRepository<User> m_userRepository;
        private readonly Repository.IRepository<Article> m_articleRepository;
        private readonly Repository.IRepository<BaseComment> m_commentRepository;

        private void Initialize()
        {
            for (var i = 0; i < 3; i++)
            {
                m_userRepository.Save(new Author(i + 1));
            }

            var authors = m_userRepository.Get().OfType<Author>().ToList();

            foreach (var author in authors)
            {
                author.FirstName = "Author";
                author.LastName = string.Format("{0}", author.Id + 1);
                author.Age = 50 + author.Id;
                author.NickName = string.Format("Author{0}", author.Id);
                author.Popularity = author.Id + 0.5;
            }

            for (var i = 0; i < 3; i++)
            {
                m_userRepository.Save(new User(i + 5));
            }

            var users = m_userRepository.Get();

            for (var i = 0; i < users.Count; i++)
            {
                users[i].FirstName = "User";
                users[i].LastName = string.Format("{0}", i + 1);
                users[i].Age = 30 + i;
            }

            m_userRepository.Save(new Admin(4));
            m_userRepository.Get().OfType<Admin>().ToList()[0].FirstName = "Admin";
            m_userRepository.Get().OfType<Admin>().ToList()[0].LastName = "User";
            m_userRepository.Get().OfType<Admin>().ToList()[0].Age = 58;
            m_userRepository.Get().OfType<Admin>().ToList()[0].Privilegies = new List<string> { "edit", "read", "delete" };

            for (var i = 0; i < 4; i++)
            {
                m_articleRepository.Save(new Article(i + 1));
            }

            var articles = m_articleRepository.Get();

            for (var i = 0; i < articles.Count; i++)
            {
                articles[i].Content = string.Format("Text {0}", i + 1);
                articles[i].Title = string.Format("Title {0}", i + 1);
                if (i == articles.Count - 1)
                {
                    articles[i].Author = authors[0];
                    break;
                }

                articles[i].Author = authors[i];
            }

            m_commentRepository.Save(new Comment(0, "Content 0", users[0], articles[0]));
            m_commentRepository.Save(new Review(1, "Review Content 1", users[0], articles[0], new Rating(4)));
            m_commentRepository.Save(new ReviewText(2, "Review text 2", users[1], articles[0], new Rating(4)));            
        }        

        public Facade(Repository.IRepository<User> userRepository, Repository.IRepository<Article> articleRepository, Repository.IRepository<BaseComment> commentRepository)
        {
            m_userRepository = userRepository;
            m_articleRepository = articleRepository;
            m_commentRepository = commentRepository;
            if (!m_commentRepository.Initialized)
            {
                Initialize();
            }
        }

        public List<User> GetAllUsers()
        {
            return m_userRepository.Get();
        }                
        
        public List<Author> GetAuthors()
        {
            return m_userRepository.Get().OfType<Author>().ToList();
        }

        public List<Admin> GetAdmins()
        {
            return m_userRepository.Get().OfType<Admin>().ToList();
        }

        public void DeleteUser(User entity)
        {
            m_userRepository.Delete(entity);
        }

        public void SaveUser(User entity)
        {
            m_userRepository.Save(entity);
        }    
    
        public List<Article> GetArticles()
        {
            return m_articleRepository.Get();
        }

        public List<BaseComment> GetAllComments()
        {
            return m_commentRepository.Get();
        }

        public List<Comment> GetComments()
        {
            return m_commentRepository.Get().OfType<Comment>().ToList();
        }

        public List<Review> GetReviews()
        {
            return m_commentRepository.Get().OfType<Review>().ToList();
        }

        public List<ReviewText> GetReviewTexts()
        {
            return m_commentRepository.Get().OfType<ReviewText>().ToList();
        }

        public List<BaseComment> FilterCommentsByArticle(Article article)
        {
            var comments = GetAllComments();
            var result = (from comment in comments
                          where comment.Article.Id == article.Id
                          select comment).ToList();
            return result;
        }

        public void CreateArticle(int id, Author author, string title, string content)
        {
            var article = new Article(id) { Author = author, Title = title, Content = content };
            m_articleRepository.Save(article);
        }

        public void CreateComment(int id, Article article, User user, string content)
        {
            var comment = new Comment(id, content, user, article);
            m_commentRepository.Save(comment);
        }

        public void CreateReview(int id, string content, Rating rating, User user, Article article)
        {
            var review = new Review(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            m_commentRepository.Save(review);
        }

        public void CreateReviewText(int id, string content, Rating rating, User user, Article article)
        {
            var review = new ReviewText(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            m_commentRepository.Save(review);
        }

        public void UpdateArticle(Article oldEntity, Article newEntity)
        {
            m_articleRepository.Update(oldEntity, newEntity);
        }

        public Article GetArticleById(int? id)
        {
            return m_articleRepository.GetById(id);
        }

        public BaseComment GetCommentById(int? id)
        {
            return m_commentRepository.GetById(id);
        }

        public Article GetRandomArticle()
        {
            return m_articleRepository.GetRandom();
        }

        public bool ArticleExists(int? id)
        {
            var elements = m_articleRepository.Get();
            return elements.Any(element => element.Id == id);
        }

        public void DeleteArticle(Article entity)
        {
            m_articleRepository.Delete(entity);
        }

        public void DeleteComment(BaseComment comment)
        {
            m_commentRepository.Delete(comment);
        }

        public void SaveArticle(Article entity)
        {
            m_articleRepository.Save(entity);
        }

        public void SaveComment(BaseComment comment)
        {
            m_commentRepository.Save(comment);
        }

        private void UpdateRating(Rating rating, Article article, User user)
        {
            var flag = false;
            //var reviews = (from comment in m_repository.Get<Review>()
            //               select comment).ToList();

            var reviews = GetReviews();

            foreach (var mreview in reviews)
            {
                if (mreview.Article == article)
                {
                    foreach (var mrating in article.Ratings)
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
    }        
}
