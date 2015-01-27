using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Repository
    {
        private List<Article> articles;
        private List<User> users;
        private List<Admin> admins;
        private List<Author> authors;
        private List<Comment> comments;

        public Repository()
        {
            articles = new List<Article>();
            users = new List<User>();
            admins = new List<Admin>();
            authors = new List<Author>();
            comments = new List<Comment>();
        }

        public void saveArticle(Article article)
        {
            articles.Add(article);
        }

        public void saveUser(User user)
        {
            users.Add(user);
        }

        public void saveAdmin(Admin admin)
        {
            admins.Add(admin);
        }

        public void saveAuthor(Author author)
        {
            authors.Add(author);
        }

        public void saveComment(Comment comment)
        {
            comments.Add(comment);
        }

        public List<Article> getArticles()
        {
            return articles;
        }

        public List<User> getUsers()
        {
            return users;
        }

        public List<Admin> getAdmins()
        {
            return admins;
        }

        public List<Author> getAuthors()
        {
            return authors;
        }

        public List<Comment> getComments()
        {
            return comments;
        }

        public void deleteArticle(Article article)
        {
            articles.Remove(article);
        }

        public void deleteUser(User user)
        {
            users.Remove(user);
        }

        public void deleteAdmin(Admin admin)
        {
            admins.Remove(admin);
        }

        public void deleteAuthor(Author author)
        {
            authors.Remove(author);
        }

        public void deleteComment(Comment comment)
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
            
            comments.Add(articles[0].addComment(1, "Comment 1", users[1]));
            comments.Add(articles[1].addComment(2, "Comment 2", users[0]));
            comments.Add(articles[1].addComment(2, "Comment 3", users[2]));
            comments.Add(articles[2].addComment(3, "Comment 4", users[0]));
            comments.Add(articles[3].addComment(4, "Comment 5", users[2]));

            articles[0].setRating(new Rating(3, users[0]));
            articles[0].setRating(new Rating(3, users[1]));
            articles[0].setRating(new Rating(2, users[2]));
            articles[0].setRating(new Rating(5, authors[0]));

            articles[1].setRating(new Rating(2, users[0]));
            articles[1].setRating(new Rating(1, users[1]));
            articles[1].setRating(new Rating(5, users[2]));
            articles[1].setRating(new Rating(5, authors[0]));
            articles[1].setRating(new Rating(3, admins[0]));

            articles[2].setRating(new Rating(1, users[0]));
            articles[2].setRating(new Rating(2, users[1]));
            articles[2].setRating(new Rating(3, users[2]));
            articles[2].setRating(new Rating(4, admins[0]));

            articles[3].setRating(new Rating(1, users[0]));
            articles[3].setRating(new Rating(5, users[0]));
            articles[3].setRating(new Rating(3, users[1]));
            articles[3].setRating(new Rating(5, users[2]));
        }
    }
}
