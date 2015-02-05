using System.Collections.Generic;
using System.Linq;
using MyFirstProject.Entity;

namespace MyFirstProject.Repository
{
    public class Repository : IRepository<Article>, IRepository<User>, IRepository<Admin>, IRepository<Author>,
        IRepository<BaseComment>
    {
        protected List<Article> Articles;
        protected List<User> Users;
        protected List<Admin> Admins;
        protected List<Author> Authors;
        protected List<BaseComment> Comments;

        public Repository()
        {
            Articles = new List<Article>();
            Users = new List<User>();
            Admins = new List<Admin>();
            Authors = new List<Author>();
            Comments = new List<BaseComment>();
        }

        public void Save(Article article)
        {
            Articles.Add(article);
        }

        public void Save(User user)
        {
            Users.Add(user);
        }

        public void Save(Admin admin)
        {
            Admins.Add(admin);
        }

        public void Save(Author author)
        {
            Authors.Add(author);
        }

        public void Save(BaseComment comment)
        {
            Comments.Add(comment);
        }

        public void Delete(Article article)
        {
            Articles.Remove(article);
        }

        public void Delete(User user)
        {
            Users.Remove(user);
        }

        public void Delete(Admin admin)
        {
            Admins.Remove(admin);
        }

        public void Delete(Author author)
        {
            Authors.Remove(author);
        }

        public void Delete(BaseComment comment)
        {
            Comments.Remove(comment);
        }

        List<Article> IRepository<Article>.Get()
        {
            return Articles;
        }

        List<User> IRepository<User>.Get()
        {
            return Users;
        }

        List<Admin> IRepository<Admin>.Get()
        {
            return Admins;
        }

        List<Author> IRepository<Author>.Get()
        {
            return Authors;
        }

        List<BaseComment> IRepository<BaseComment>.Get()
        {
            return Comments;
        }

    public virtual void Initialize()
        {
            for (var i = 0; i < 3; i++)
            {
                Authors.Add(new Author(i + 1));
            }

            for (var i = 0; i < Authors.Count; i++)
            {
                Authors[i].FirstName = "Author";
                Authors[i].LastName = (i + 1).ToString();
                Authors[i].Age = 50 + i;
                Authors[i].NickName = "Author" + (i + 1).ToString();
                Authors[i].Popularity = i + 0.5;
            }

            Admins.Add(new Admin(4));
            Admins[0].FirstName = "Admin";
            Admins[0].LastName = "User";
            Admins[0].Age = 58;
            Admins[0].Privilegies = new List<string> { "edit", "read", "delete" };

            for (var i = 0; i < 3; i++)
            {
                Users.Add(new User(i + 5));
            }

            for (var i = 0; i < Users.Count; i++)
            {
                Users[i].FirstName = "User";
                Users[i].LastName = (i + 1).ToString();
                Users[i].Age = 30 + i;
            }

            for (var i = 0; i < 4; i++)
            {
                Articles.Add(new Article(i + 1));
            }

            for (var i = 0; i < Articles.Count; i++)
            {
                Articles[i].Content = "Text" + (i + 1).ToString();
                Articles[i].Title = "Title" + (i + 1).ToString();
                if (i == Articles.Count - 1)
                {
                    Articles[i].Author = Authors[0];
                    break;
                }

                Articles[i].Author = Authors[i];
            }

            Comments.Add(CreateReview(1, "Review 1", new Rating(3), Users[1], Articles[0]));
            Comments.Add(new Comment(2, "Comment 1", Users[2], Articles[1]));
            Comments.Add(CreateReview(2, "Review 2", new Rating(6), Users[0], Articles[1]));
            Comments.Add(CreateReview(3, "Review 3", new Rating(-1), Users[2], Articles[1]));
            Comments.Add(CreateReview(4, "Review 4", new Rating(1), Users[0], Articles[2]));
            Comments.Add(CreateReviewText(5, "Review Text 5", new Rating(0), Users[2], Articles[3]));
        }

        private void UpdateRating(Rating rating, Article article, User user)
        {
            var flag = false;
            var reviews = (from comment in Comments
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
