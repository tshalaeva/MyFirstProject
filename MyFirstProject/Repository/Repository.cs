using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entities;

namespace MyFirstProject.Repository
{
    public class Repository : IRepository
    {
        private List<IEntity> m_data; 

        public Repository()
        {
            m_data = new List<IEntity>();
        }

        public List<T> Get<T>() where T : IEntity
        {
            return m_data.OfType<T>().ToList();
        }

        public void Save<T>(T entity) where T : IEntity
        {
            m_data.Add(entity);
        }

        public void Delete<T>(T entity) where T : IEntity
        {
            m_data.Remove(entity);
        }
       
        public virtual void Initialize()
        {
            for (var i = 0; i < 3; i++)
            {
                Save(new Author(i + 1));
            }

            for (var i = 0; i < Get<Author>().Count; i++)
            {
                Get<Author>()[i].FirstName = "Author";
                Get<Author>()[i].LastName = (i + 1).ToString();
                Get<Author>()[i].Age = 50 + i;
                Get<Author>()[i].NickName = "Author" + (i + 1).ToString();
                Get<Author>()[i].Popularity = i + 0.5;
            }

            Save(new Admin(4));  
            Get<Admin>()[0].FirstName = "Admin";
            Get<Admin>()[0].LastName = "User";
            Get<Admin>()[0].Age = 58;
            Get<Admin>()[0].Privilegies = new List<string> { "edit", "read", "delete" };

            for (var i = 0; i < 3; i++)
            {
                Save(new User(i + 5));
            }

            for (var i = 0; i < Get<User>().Count; i++)
            {
                Get<User>()[i].FirstName = "User";
                Get<User>()[i].LastName = (i + 1).ToString();
                Get<User>()[i].Age = 30 + i;
            }

            for (var i = 0; i < 4; i++)
            {
                Save(new Article(i + 1));
            }

            for (var i = 0; i < Get<Article>().Count; i++)
            {
                Get<Article>()[i].Content = "Text" + (i + 1).ToString();
                Get<Article>()[i].Title = "Title" + (i + 1).ToString();
                if (i == Get<Article>().Count - 1)
                {
                    Get<Article>()[i].Author = Get<Author>()[0];
                    break;
                }

                Get<Article>()[i].Author = Get<Author>()[i];
            }

            Save(CreateReview(1, "Review 1", new Rating(3), Get<User>()[1], Get<Article>()[0]));
            Save(new Comment(2, "Comment 1", Get<User>()[2], Get<Article>()[1]));
            Save(CreateReview(2, "Review 2", new Rating(6), Get<User>()[0], Get<Article>()[1]));
            Save(CreateReview(3, "Review 3", new Rating(-1), Get<User>()[2], Get<Article>()[1]));
            Save(CreateReview(4, "Review 4", new Rating(1), Get<User>()[0], Get<Article>()[2]));
            Save(CreateReviewText(5, "Review Text 5", new Rating(0), Get<User>()[2], Get<Article>()[3]));
        }

        private void UpdateRating(Rating rating, Article article, User user)
        {
            var flag = false;
            var reviews = (from comment in Get<BaseComment>()
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
