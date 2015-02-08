using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class Facade<T> where T : IEntity
    {
        public Facade()
        {
            Repository = new Repository.Repository();
            Repository.Initialize();
        }

        public Facade(Repository.Repository repository)
        {
            Repository = repository;
            Repository.Initialize();
        }

        private Repository.Repository Repository { get; set; }

        public void Save(T entity)
        {
            Repository.Save(entity);
        }

        public List<T> Get()
        {
            return Repository.Get<T>();
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public List<BaseComment> FilterCommentsByArticle(Article article)
        {            
            var comments = Repository.Get<BaseComment>();
            var result = (from comment in comments
                where comment.Article.Id == article.Id
                select comment).ToList();
            return result;
        }

        public Article CreateArticle(int id, Author author, string title, string content)
        {
            var article = new Article(id) {Author = author, Title = title, Content = content};
            Repository.Save(article);
            return article;
        }

        public Comment CreateComment(int id, Article article, User user, string content)
        {
            Comment comment = new Comment(id, content, user, article);
            Repository.Save(comment);
            return comment;
        }

        private void UpdateRating(Rating rating, Article article, User user)
        {
            var flag = false;
            var reviews = (from comment in Repository.Get<BaseComment>()
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

        public Review CreateReview(int id, string content, Rating rating, User user, Article article)
        {
            var review = new Review(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            Repository.Save(review);
            return review;
        }

        public ReviewText CreateReviewText(int id, string content, Rating rating, User user, Article article)
        {
            var review = new ReviewText(id, content, user, article, rating);
            UpdateRating(rating, article, user);
            Repository.Save(review);
            return review;
        }
    }
}
