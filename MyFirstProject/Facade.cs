using System;
using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Facade
    {
        private readonly Repository.IRepository<User> _mUserRepository;
        private readonly Repository.IRepository<Article> _mArticleRepository;
        private readonly Repository.IRepository<BaseComment> _mCommentRepository;

        private void Initialize()
        {
            var authors = new List<Author>();
            for (var i = 0; i < 3; i++)
            {
                authors.Add(new Author(i + 1));
            }

            foreach (var author in authors)
            {
                author.FirstName = "Author";
                author.LastName = string.Format("{0}", author.Id + 1);
                author.Age = 50 + author.Id;
                author.NickName = string.Format("Author{0}", author.Id);
                author.Popularity = Convert.ToDecimal(author.Id + 0.5);
            }

            foreach (var author in authors)
            {
                author.Id = _mUserRepository.Save(author);
            }

            var users = new List<User>();

            for (var i = 0; i < 3; i++)
            {
                users.Add(new User(i));
            }

            var j = 0;
            foreach (var user in users)
            {
                user.FirstName = "User";
                user.LastName = string.Format("{0}", j++);
                user.Age = 30 + j;
                user.Id = _mUserRepository.Save(user);
            }

            var admin = new Admin(4)
            {
                FirstName = "Admin",
                LastName = "User",
                Age = 58,
                Privilegies = new List<string> {"edit", "read", "delete"}
            };

            admin.Id = _mUserRepository.Save(admin);

            //var updatedAdmin = admin;
            //updatedAdmin.FirstName = "Updated Admin";
            //updatedAdmin.Privilegies = new List<string> {"Edit", "Read"};
            //_mUserRepository.Save(updatedAdmin);

            var articles = new List<Article>();

            for (var i = 0; i < 4; i++)
            {
                var article = new Article
                {
                    Content = string.Format("Text {0}", i + 1),
                    Title = string.Format("Title {0}", i + 1)
                };
                if (i == 3)
                {
                    article.Author = authors[0];
                    article.Id = _mArticleRepository.Save(article);
                    articles.Add(article);
                    break;
                }

                article.Author = authors[i];
                article.Id = _mArticleRepository.Save(article);
                articles.Add(article);
            }

            _mCommentRepository.Save(new Comment(0, "Content 0", users[0], articles[0]));
            _mCommentRepository.Save(new Review(1, "Review Content 1", users[0], articles[0], new Rating(4)));
            _mCommentRepository.Save(new ReviewText(2, "Review text 2", users[1], articles[0], new Rating(4)));
            _mCommentRepository.Save(new Review(1, "Updated review", users[0], articles[1], new Rating(3)));
        }

        public Facade(Repository.IRepository<User> userRepository, Repository.IRepository<Article> articleRepository, Repository.IRepository<BaseComment> commentRepository)
        {
            _mUserRepository = userRepository;
            _mArticleRepository = articleRepository;
            _mCommentRepository = commentRepository;
            if (!(_mCommentRepository.Initialized) && !(_mArticleRepository.Initialized) && !(_mUserRepository.Initialized))
            {
                Initialize();
            }
        }

        public List<User> GetAllUsers()
        {
            return _mUserRepository.Get();
        }

        public List<Author> GetAuthors()
        {
            return _mUserRepository.Get().OfType<Author>().ToList();
        }

        public List<Admin> GetAdmins()
        {
            return _mUserRepository.Get().OfType<Admin>().ToList();
        }

        public void DeleteUser(User entity)
        {
            _mUserRepository.Delete(entity);
        }

        public void SaveUser(User entity)
        {
            _mUserRepository.Save(entity);
        }

        public List<Article> GetArticles()
        {
            return _mArticleRepository.Get();
        }

        public List<BaseComment> GetAllComments()
        {
            return _mCommentRepository.Get();
        }

        public List<Comment> GetComments()
        {
            return _mCommentRepository.Get().OfType<Comment>().ToList();
        }

        public List<Review> GetReviews()
        {
            return _mCommentRepository.Get().OfType<Review>().ToList();
        }

        public List<ReviewText> GetReviewTexts()
        {
            return _mCommentRepository.Get().OfType<ReviewText>().ToList();
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
            _mArticleRepository.Save(article);
        }

        public void CreateComment(int id, Article article, User user, string content)
        {
            var comment = new Comment(id, content, user, article);
            _mCommentRepository.Save(comment);
        }

        public void CreateReview(int id, string content, Rating rating, User user, Article article)
        {
            var review = new Review(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            _mCommentRepository.Save(review);
        }

        public void CreateReviewText(int id, string content, Rating rating, User user, Article article)
        {
            var review = new ReviewText(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            _mCommentRepository.Save(review);
        }

        public void UpdateArticle(int oldEntity, Article newEntity)
        {
            _mArticleRepository.Update(oldEntity, newEntity);
        }

        public Article GetArticleById(int? id)
        {
            return _mArticleRepository.GetById(id);
        }

        public BaseComment GetCommentById(int? id)
        {
            return _mCommentRepository.GetById(id);
        }

        public Article GetRandomArticle()
        {
            return _mArticleRepository.GetRandom();
        }

        public bool ArticleExists(int? id)
        {
            var elements = _mArticleRepository.Get();
            return elements.Any(element => element.Id == id);
        }

        public void DeleteArticle(Article entity)
        {
            _mArticleRepository.Delete(entity);
        }

        public void DeleteComment(BaseComment comment)
        {
            _mCommentRepository.Delete(comment);
        }

        public void SaveArticle(Article entity)
        {
            _mArticleRepository.Save(entity);
        }

        public void SaveComment(BaseComment comment)
        {
            _mCommentRepository.Save(comment);
        }

        private void UpdateRating(Rating rating, Article article, User user)
        {
            var flag = false;

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
