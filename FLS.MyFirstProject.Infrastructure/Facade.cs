using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Repositories;
using ObjectRepository.Entities;
using StructureMap;

namespace FLS.MyFirstProject.Infrastructure
{
    public class Facade
    {
        private readonly IRepository<User> m_userRepository;
        private readonly IRepository<Article> m_articleRepository;
        private readonly IRepository<BaseComment> m_commentRepository;

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
                author.Id = m_userRepository.Save(author);
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
                user.Id = m_userRepository.Save(user);
            }

            var updatedUser = users[0];
            updatedUser.FirstName = "Updated";
            m_userRepository.Save(updatedUser);

            var admin = new Admin(4)
            {
                FirstName = "Admin",
                LastName = "User",
                Age = 58,
                Privilegies = new List<string> { "edit", "read", "delete" }
            };

            admin.Id = m_userRepository.Save(admin);

            var updatedAdmin = admin;
            updatedAdmin.FirstName = "Updated Admin";
            updatedAdmin.Privilegies = new List<string> { "Edit", "Read" };
            m_userRepository.Save(updatedAdmin);

            var updatedAuthor = authors[0];
            updatedAuthor.NickName = "Updated Nick";
            m_userRepository.Save(updatedAuthor);

            var articles = new List<Article>();

            for (var i = 0; i < 3; i++)
            {
                var article = new Article(i)
                {
                    Content = string.Format("Text {0}", i + 1),
                    Title = string.Format("Title {0}", i + 1),
                    Author = new Author()
                };

                article.Author = authors[i];
                article.Id = m_articleRepository.Save(article);
                articles.Add(article);
            }

            var updatedArticle = articles[1];
            updatedArticle.Title = "Updated Article";
            m_articleRepository.Save(updatedArticle);

            m_commentRepository.Save(new Comment(0, "Content 0", users[0], articles[0]));
            m_commentRepository.Save(new Review(1, "Review Content 1", users[0], articles[0], new Rating(4)));
            m_commentRepository.Save(new ReviewText(2, "Review text 2", users[1], articles[0], new Rating(4)));
        }

        [DefaultConstructor]
        public Facade(IRepository<User> userRepository, IRepository<Article> articleRepository, IRepository<BaseComment> commentRepository)
        {
            m_userRepository = userRepository;
            m_articleRepository = articleRepository;
            m_commentRepository = commentRepository;
            if (!(m_commentRepository.Initialized) && !(m_articleRepository.Initialized) && !(m_userRepository.Initialized))
            {
                Initialize();
            }
        }

        public List<Article> GetArticles(int from, int count)
        {
            return m_articleRepository.Get(from, count);
        }

        public int GetArticlesCount()
        {
            return m_articleRepository.GetCount();
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

        public void DeleteUser(int entityId)
        {
            var comments = m_commentRepository.Get(entityId);
            foreach (var comment in comments.Where(comment => comment.User.Id == entityId))
            {
                m_commentRepository.Delete(comment.Id);
            }
            m_userRepository.Delete(entityId);
        }

        public int CreateUser(string firstName, string lastName, int age)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            };
            return SaveUser(user);
        }

        public int UpdateUser(int id, string firstName, string lastName, int age)
        {
            var user = m_userRepository.GetById(id);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Age = age;
            return m_userRepository.Update(id, user);
        }

        public int SaveUser(User entity)
        {
            return m_userRepository.Save(entity);
        }

        public User GetUserById(int id)
        {
            return m_userRepository.GetById(id);
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

        public int CreateArticle(int id, Author author, string title, string content)
        {
            var article = new Article(id) { Author = author, Title = title, Content = content };
            return m_articleRepository.Save(article);
        }

        public int CreateComment(int articleId, User user, string content)
        {
            //var comment = new Comment(id, content, user, m_articleRepository.GetById(articleId));
            var comment = new Comment()
            {
                User = user,
                Article = m_articleRepository.GetById(articleId),
                Content = content
            };
            return m_commentRepository.Save(comment);
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

        public int UpdateComment(int id, string content)
        {
            var oldComment = m_commentRepository.GetById(id);
            var newComment = new Comment(id, content, oldComment.User, oldComment.Article);
            return m_commentRepository.Update(id, newComment);
        }        

        //public void UpdateArticle(int oldEntity, Article newEntity)
        public int UpdateArticle(int oldEntity, string articleTitle, string articleContent)
        {
            var newEntity = new Article(oldEntity)
            {
                Author = GetArticleById(oldEntity).Author,
                Content = articleContent,
                Ratings = GetArticleById(oldEntity).Ratings,
                Title = articleTitle
            };
            return m_articleRepository.Update(oldEntity, newEntity);
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

        public void DeleteArticle(int entityId)
        {
            m_articleRepository.Delete(entityId);
            var comments = m_commentRepository.Get();
            foreach (var comment in comments.Where(comment => comment.Article.Id == entityId))
            {
                m_commentRepository.Delete(comment.Id);
            }
        }

        public void DeleteComment(int commentId)
        {
            m_commentRepository.Delete(commentId);
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
