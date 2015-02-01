using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Repository : IRepository
    {
        private List<Article> articles;
        private List<User> users;
        private List<Admin> admins;
        private List<Author> authors;
        private List<IComment> comments;        

        public Repository()
        {
            articles = new List<Article>();
            users = new List<User>();
            admins = new List<Admin>();
            authors = new List<Author>();
            comments = new List<IComment>();            
        }

        public void SaveArticle(Article article)
        {
            articles.Add(article);
        }

        public void SaveUser(User user)
        {
            users.Add(user);
        }

        public void SaveAdmin(Admin admin)
        {
            admins.Add(admin);
        }

        public void SaveAuthor(Author author)
        {
            authors.Add(author);
        }

        public void SaveComment(Comment comment)
        {
            comments.Add(comment);
        }

        public void SaveReview(Review review)
        {
            comments.Add(review);
        }

        public List<Article> GetArticles()
        {
            return articles;
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public List<Admin> GetAdmins()
        {
            return admins;
        }

        public List<Author> GetAuthors()
        {
            return authors;
        }

        public List<IComment> GetComments()
        {
            return comments;
        }

        public void DeleteArticle(Article article)
        {
            articles.Remove(article);
        }

        public void DeleteUser(User user)
        {
            users.Remove(user);
        }

        public void DeleteAdmin(Admin admin)
        {
            admins.Remove(admin);
        }

        public void DeleteAuthor(Author author)
        {
            authors.Remove(author);
        }

        public void DeleteComment(IComment comment)
        {
            comments.Remove(comment);
        }

        public void Initialize()
        {
            for(int i = 0; i < 3; i++)
            {
                authors.Add(new Author(i + 1));
            }
            for (int i = 0; i < authors.Count; i++)
            {
                authors[i].FirstName = "Author";
                authors[i].LastName = (i + 1).ToString();
                authors[i].Age = 50 + i;
                authors[i].NickName = "Author" + (i + 1).ToString();
                authors[i].Popularity = i + 0.5;
            }

            admins.Add(new Admin(4));
            admins[0].FirstName = "Admin";
            admins[0].LastName = "User";
            admins[0].Age = 58;
            admins[0].Privilegies = new List<string> { "edit", "read", "delete" };

            for(int i = 0; i < 3; i++)
            {
                users.Add(new User(i + 5));
            }
            for (int i = 0; i < users.Count; i++)
            {
                users[i].FirstName = "User";
                users[i].LastName = (i + 1).ToString();
                users[i].Age = 30 + i;
            }

            for(int i = 0; i < 4; i++)
            {
                articles.Add(new Article(i + 1));
            }
            for (int i = 0; i < articles.Count; i++)
            {
                articles[i].Content = "Text" + (i + 1).ToString();
                articles[i].Title = "Title" + (i + 1).ToString();
                if (i == articles.Count - 1)
                {
                    articles[i].Author = authors[0];
                    break;
                }
                articles[i].Author = authors[i];
            }

            comments.Add(CreateReview(1, "Review 1", new Rating(3), users[1], articles[0]));
            comments.Add(CreateComment(2, "Comment 1", users[2], articles[1]));
            comments.Add(CreateReview(2, "Review 2", new Rating(6), users[0], articles[1]));
            comments.Add(CreateReview(3, "Review 3", new Rating(-1), users[2], articles[1]));
            comments.Add(CreateReview(4, "Review 4", new Rating(1), users[0], articles[2]));
            comments.Add(CreateReview(5, "Review 5", new Rating(0), users[2], articles[3]));
        }


        private Comment CreateComment(int id, string content, User user, Article article)
        {
            Comment comment = new Comment(id);
            comment.Article = article;
            comment.User = user;
            comment.Content = content;
            return comment;
        }

        private Review CreateReview(int id, string content, Rating rating, User user, Article article)
        {
            Review review = new Review(id);
            review.Article = article;
            review.User = user;                        
            bool flag = false;
            List<IComment> reviews = (from comment in GetComments()
                                        where comment.IsReview()
                                    select comment).ToList();
            foreach (Review mreview in reviews)
            {
                if (mreview.Article == article)
                {
                    foreach (Rating mrating in article.Rating)
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
            review.Content = content;
            review.Rating = rating;
            return review;
        }
    }
}
