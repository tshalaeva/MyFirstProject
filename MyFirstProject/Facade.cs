using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Facade
    {
        //private readonly Repository.Repository m_repository;
        //private readonly Repository.ArticleRepository m_articleRepository;
        //private readonly Repository.UserRepository m_userRepository;
        //private readonly Repository.CommentRepository m_commentRepository;

        private readonly Repository.Repository m_repository;

        //private void Initialize()
        //{
        //    for (var i = 0; i < 3; i++)
        //    {
        //        m_repository.Save(new Author(i + 1));
        //    }

        //    var authors = m_repository.Get<Author>();

        //    foreach (var author in authors)
        //    {
        //        author.FirstName = "Author";
        //        author.LastName = string.Format("{0}", author.Id + 1);
        //        author.Age = 50 + author.Id;
        //        author.NickName = string.Format("Author{0}", author.Id);
        //        author.Popularity = author.Id + 0.5;
        //    }

        //    for (var i = 0; i < 3; i++)
        //    {
        //        m_repository.Save(new User(i + 5));
        //    }

        //    var users = m_repository.Get<User>();

        //    for (var i = 0; i < users.Count; i++)
        //    {
        //        users[i].FirstName = "User";
        //        users[i].LastName = string.Format("{0}", i + 1);
        //        users[i].Age = 30 + i;
        //    }

        //    m_repository.Save(new Admin(4));
        //    m_repository.Get<Admin>()[0].FirstName = "Admin";
        //    m_repository.Get<Admin>()[0].LastName = "User";
        //    m_repository.Get<Admin>()[0].Age = 58;
        //    m_repository.Get<Admin>()[0].Privilegies = new List<string> { "edit", "read", "delete" };

        //    for (var i = 0; i < 4; i++)
        //    {
        //        m_repository.Save(new Article(i + 1));
        //    }

        //    var articles = m_repository.Get<Article>();

        //    for (var i = 0; i < articles.Count; i++)
        //    {
        //        articles[i].Content = string.Format("Text {0}", i + 1);
        //        articles[i].Title = string.Format("Title {0}", i + 1);
        //        if (i == articles.Count - 1)
        //        {
        //            articles[i].Author = authors[0];
        //            break;
        //        }

        //        articles[i].Author = authors[i];
        //    }

        //    m_repository.Save(new Comment(0, "Content 0", users[0], articles[0]));
        //    m_repository.Save(new Review(1, "Review Content 1", users[0], articles[0], new Rating(4)));
        //    m_repository.Save(new ReviewText(2, "Review text 2", users[1], articles[0], new Rating(4)));
        //}

        private void Initialize()
        {
            for (var i = 0; i < 3; i++)
            {
                m_repository.UserRepository.Save(new Author(i + 1));
            }

            var authors = m_repository.UserRepository.GetAuthors();

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
                m_repository.UserRepository.Save(new User(i + 5));
            }

            var users = m_repository.UserRepository.GetAllUsers();

            for (var i = 0; i < users.Count; i++)
            {
                if (users[i] is Author) continue;
                users[i].FirstName = "User";
                users[i].LastName = string.Format("{0}", i + 1);
                users[i].Age = 30 + i;
            }

            m_repository.UserRepository.Save(new Admin(4));
            m_repository.UserRepository.GetAdmins().First().FirstName = "Admin";
            m_repository.UserRepository.GetAdmins().First().LastName = "User";
            m_repository.UserRepository.GetAdmins().First().Age = 58;
            m_repository.UserRepository.GetAdmins().First().Privilegies = new List<string> { "edit", "read", "delete" };

            for (var i = 0; i < 4; i++)
            {
                m_repository.ArticleRepository.Save(new Article(i + 1));
            }

            var articles = m_repository.ArticleRepository.GetArticles();

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

            m_repository.CommentRepository.Save(new Comment(0, "Content 0", users[0], articles[0]));
            m_repository.CommentRepository.Save(new Review(1, "Review Content 1", users[0], articles[0], new Rating(4)));
            m_repository.CommentRepository.Save(new ReviewText(2, "Review text 2", users[1], articles[0], new Rating(4)));
        }

        //public Facade(Repository.ArticleRepository articleRepository, Repository.UserRepository userRepository, Repository.CommentRepository commentRepository)
        public Facade(Repository.Repository repository)
        {
            //m_repository = repository;
            //if (!m_repository.Initialized)
            //{
            //    Initialize();
            //}
            //m_repository.ArticleRepository = repository.ArticleRepository;
            //m_repository.CommentRepository = repository.CommentRepository;
            //m_repository.UserRepository = repository.UserRepository;            
            m_repository = repository;
            if (!m_repository.Initialized)
            {
                Initialize();
            }
        }

        public void SaveArticle(Article entity)
        {
            m_repository.Save(entity);
        }

        public void SaveUser(User entity)
        {
            m_repository.UserRepository.Save(entity);
        }

        public void SaveComment(BaseComment entity)
        {
            m_repository.CommentRepository.Save(entity);
        }

        public List<Article> GetArticles()
        {
            return m_repository.ArticleRepository.GetArticles();
        }

        public List<User> GetAllUsers()
        {
            return m_repository.UserRepository.GetAllUsers();
        }

        public List<Author> GetAuthors()
        {
            return m_repository.UserRepository.GetAuthors();
        }

        public List<Admin> GetAdmins()
        {
            return m_repository.UserRepository.GetAdmins();
        }

        public List<Comment> GetComments()
        {
            return m_repository.CommentRepository.GetComments().OfType<Comment>().ToList();
        }

        public List<Review> GetReviews()
        {
            return m_repository.CommentRepository.GetComments().OfType<Review>().ToList();
        }

        public List<ReviewText> GetReviewTexts()
        {
            return m_repository.CommentRepository.GetComments().OfType<ReviewText>().ToList();
        }

        public void DeleteArticle(Article entity)
        {
            m_repository.CommentRepository.Delete(entity);
        }

        public List<BaseComment> FilterCommentsByArticle(Article article)
        {
            var comments = m_repository.CommentRepository.GetComments();
            var result = (from comment in comments
                          where comment.Article.Id == article.Id
                          select comment).ToList();
            return result;
        }

        public void CreateArticle(int id, Author author, string title, string content)
        {
            var article = new Article(id) { Author = author, Title = title, Content = content };            
            SaveArticle(article);
        }

        public void CreateComment(int id, Article article, User user, string content)
        {
            var comment = new Comment(id, content, user, article);
            m_repository.CommentRepository.Save(comment);
        }

        public void CreateReview(int id, string content, Rating rating, User user, Article article)
        {
            var review = new Review(id, content, user, article, rating);
            UpdateRating(rating, article, user);            
            SaveComment(review);
        }

        public void CreateReviewText(int id, string content, Rating rating, User user, Article article)
        {
            var review = new ReviewText(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            m_repository.CommentRepository.Save(review);
        }

        public void UpdateArticle(Article oldEntity, Article newEntity)
        {
            m_repository.ArticleRepository.Update(oldEntity, newEntity);
        }

        public Article GetArticleById(int? id)
        {
            return (Article)m_repository.ArticleRepository.GetById(id);
        }

        public BaseComment GetCommentById(int? id)
        {
            return (BaseComment)m_repository.CommentRepository.GetById(id);
        }

        public Article GetRandomArticle()
        {
            return (Article)m_repository.ArticleRepository.GetRandom();
        }

        public bool ArticleExists(int? id)
        {
            var elements = m_repository.ArticleRepository.GetArticles();
            return elements.Any(element => element.Id == id);
        }

        private void UpdateRating(Rating rating, Article article, User user)
        {
            var flag = false;
            var reviews = (from comment in m_repository.CommentRepository.GetComments()
                           where comment is Review
                           select comment).ToList();

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
