using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Facade<T> where T : IEntity
    {
        private readonly Repository.Repository m_repository;

        private void Initialize()
        {
            for (var i = 0; i < 3; i++)
            {
                m_repository.Save(new Author(i + 1));
            }

            var authors = m_repository.Get<Author>();

            for (var i = 0; i < authors.Count; i++)
            {
                authors[i].FirstName = "Author";
                authors[i].LastName = (i + 1).ToString();
                authors[i].Age = 50 + i;
                authors[i].NickName = "Author" + (i + 1).ToString();
                authors[i].Popularity = i + 0.5;
            }

            for (var i = 0; i < 3; i++)
            {
                m_repository.Save(new User(i + 5));
            }

            var users = m_repository.Get<User>();

            for (var i = 0; i < users.Count; i++)
            {
                users[i].FirstName = "User";
                users[i].LastName = string.Format("{0}", i + 1);
                users[i].Age = 30 + i;
            }

            m_repository.Save(new Admin(4));
            m_repository.Get<Admin>()[0].FirstName = "Admin";
            m_repository.Get<Admin>()[0].LastName = "User";
            m_repository.Get<Admin>()[0].Age = 58;
            m_repository.Get<Admin>()[0].Privilegies = new List<string> { "edit", "read", "delete" };

            for (var i = 0; i < 4; i++)
            {
                m_repository.Save(new Article(i + 1));
            }

            var articles = m_repository.Get<Article>();

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
        }

        public Facade(Repository.Repository repository)
        {
            m_repository = new Repository.Repository();
            if (!repository.Initialized)
            {
                Initialize();
            }
            else
            {
                m_repository = repository;   
            }            
        }

        public void Save(T entity)
        {
            m_repository.Save(entity);
        }

        public List<T> Get()
        {
            return m_repository.Get<T>();
        }

        public void Delete(T entity)
        {
            m_repository.Delete(entity);
        }

        public List<BaseComment> FilterCommentsByArticle(Article article)
        {
            var comments = m_repository.Get<BaseComment>();
            var result = (from comment in comments
                          where comment.Article.Id == article.Id
                          select comment).ToList();
            return result;
        }

        public Article CreateArticle(int id, Author author, string title, string content)
        {
            var article = new Article(id) { Author = author, Title = title, Content = content };
            m_repository.Save(article);
            return article;
        }

        public Comment CreateComment(int id, Article article, User user, string content)
        {
            Comment comment = new Comment(id, content, user, article);
            m_repository.Save(comment);
            return comment;
        }

        public Review CreateReview(int id, string content, Rating rating, User user, Article article)
        {
            var review = new Review(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            m_repository.Save(review);
            return review;
        }

        public ReviewText CreateReviewText(int id, string content, Rating rating, User user, Article article)
        {
            var review = new ReviewText(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            m_repository.Save(review);
            return review;
        }

        public void Update(T oldEntity, T newEntity)
        {
            m_repository.Update(oldEntity, newEntity);
        }

        private void UpdateRating(Rating rating, Article article, User user)
        {
            var flag = false;
            var reviews = (from comment in m_repository.Get<BaseComment>()
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
