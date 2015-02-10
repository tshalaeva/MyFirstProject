using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Facade<T> where T : IEntity
    {
        private readonly Repository.Repository m_repository;

        public Facade(Repository.Repository repository)
        {
            this.m_repository = repository;           
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
