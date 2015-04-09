using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    class AdoHelper
    {
        protected virtual string ConnectionString
        {
            get
            {
                return ConfigurationManager.
    ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }

        public void Initialize()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                const string commandText = "INSERT INTO User(Id,FirstName,LastName,Age) VALUES(@id,@firstName,@lastName,@age)";
                var command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", 0);
                command.Parameters.AddWithValue("@firstName", "Test");
                command.Parameters.AddWithValue("@lastName", "User");
                command.Parameters.AddWithValue("@age", 30);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM [dbo].[User]", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!(reader[4] is Guid))
                    {
                        users.Add(new User());
                        users.Last().FirstName = reader[1].ToString();
                        users.Last().LastName = reader[2].ToString();
                        users.Last().Age = (int)reader[3];
                    }
                    else
                    {
                        var admin = new Admin();
                        admin.FirstName = reader[1].ToString();
                        admin.LastName = reader[2].ToString();
                        admin.Age = (int)reader[3];
                        admin.Privilegies = GetPrivilegies((Guid)reader[4]);
                        users.Add(admin);
                    }
                }
                reader.Close();
                connection.Close();
            }
            return users;
        }

        public int SaveUser(User user)
        {
            int userId;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = new StringBuilder();
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[User](FirstName,LastName,Age) VALUES('{0}','{1}',{2})",
                    user.FirstName, user.LastName, user.Age);

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                var getIdCmd = "SELECT TOP 1 Id FROM [dbo].[User] ORDER BY Id DESC";
                using (var command = new SqlCommand(getIdCmd, connection))
                {
                    command.CommandType = CommandType.Text;
                    userId = (int)command.ExecuteScalar();
                }

                connection.Close();
            }
            return userId;
        }

        public int SaveAdmin(Admin admin)
        {
            int adminId;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = new StringBuilder();
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendLine("DECLARE @Privilegies uniqueidentifier");
                cmdText.AppendLine("SET @Privilegies=NEWID()");
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[User](FirstName,LastName,Age,PrivilegiesId) VALUES ('{0}','{1}',{2},@Privilegies) ",
                    admin.FirstName, admin.LastName, admin.Age);
                cmdText.AppendFormat("INSERT INTO Privilegies(Id,List) VALUES (@Privilegies, '{0}') ", admin);
                //cmdText.AppendLine("SELECT Id FROM [dbo].[User]");
                cmdText.AppendLine("COMMIT");

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                var getIdCmd = "SELECT TOP 1 Id FROM [dbo].[User] ORDER BY Id DESC";
                using (var command = new SqlCommand(getIdCmd, connection))
                {
                    command.CommandType = CommandType.Text;
                    adminId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return adminId;
        }

        public int SaveAuthor(Author author)
        {
            int authorId;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = new StringBuilder();
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendLine("DECLARE @AuthorID uniqueidentifier");
                cmdText.AppendLine("SET @AuthorID=NEWID()");
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[User](FirstName,LastName,Age,AuthorId) VALUES ('{0}','{1}',{2},@AuthorID); ",
                    author.FirstName, author.LastName, author.Age);
                cmdText.AppendFormat("INSERT INTO [dbo].[Author](Id,NickName,Popularity) VALUES (@AuthorID, '{0}', '{1}'); ", author.NickName, author.Popularity);
                cmdText.AppendLine("SELECT Id FROM [dbo].[User]");
                cmdText.AppendLine("COMMIT");

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                var getIdCmd = "SELECT TOP 1 Id FROM [dbo].[User] ORDER BY Id DESC";
                using (var command = new SqlCommand(getIdCmd, connection))
                {
                    command.CommandType = CommandType.Text;
                    authorId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return authorId;
        }

        private List<string> GetPrivilegies(Guid id)
        {
            var result = new List<string>();
            string privilegiesString;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(string.Format("SELECT List FROM [dbo].[Privilegies] WHERE Id='{0}'", id), connection);
                var reader = command.ExecuteReader();
                reader.Read();
                privilegiesString = reader[0].ToString();
                reader.Close();
                connection.Close();
            }

            var privilegiesArray = privilegiesString.Split(',');
            foreach (var privilegy in privilegiesArray)
            {
                result.Add(privilegy);
            }

            return result;
        }

        public int GetUsersCount()
        {
            int result;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(Id) FROM [dbo].[User]", connection);
                result = (int)command.ExecuteScalar();

                connection.Close();
            }
            return result;
        }

        public void DeleteUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new StringBuilder();
                connection.Open();
                if (!(user is Admin))
                {
                    cmdText.AppendFormat(
                            "DELETE FROM [dbo].[User] WHERE Id='{0}'",
                            user.Id);
                }
                else
                {
                    var getIdCommand = string.Format("SELECT PrivilegyId FROM [dbo].[User] WHERE Id={0}", user.Id);
                    Guid privilegyId;
                    using (var command = new SqlCommand(getIdCommand, connection))
                    {
                        command.CommandType = CommandType.Text;
                        privilegyId = (Guid)command.ExecuteScalar();
                    }

                    cmdText.AppendLine("BEGIN TRANSACTION");
                    cmdText.AppendFormat(
                        "DELETE FROM [dbo].[User] WHERE Id={0}) ", user.Id);
                    cmdText.AppendFormat("DELETE FROM [dbo].[Privilegies] WHERE Id={0}", privilegyId);
                    cmdText.AppendLine("COMMIT");
                }

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdateUser(User oldUser, User newUser)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newUser.FirstName, newUser.LastName, newUser.Age, oldUser.Id);
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public User GetUserById(int? id)
        {
            var user = new User((int)id);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT * FROM [dbo].[User] WHERE Id={0}", id), connection);
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[4].Equals(DBNull.Value) && reader[5].Equals(DBNull.Value))
                    {
                        user.FirstName = reader[1].ToString();
                        user.LastName = reader[2].ToString();
                        user.Age = (int)reader[3];
                    }
                    else
                    {
                        var admin0 = new Admin((int)id)
                             {
                                 FirstName = reader[1].ToString(),
                                 LastName = reader[2].ToString(),
                                 Age = (int)reader[3]
                             };
                        foreach (var privilegy in GetPrivilegies((Guid)reader[4]))
                        {
                            admin0.Privilegies.Add(privilegy);
                        }
                        user = admin0;

                    }
                }
                reader.Close();
                connection.Close();
            }
            return user;
        }

        public Author GetAuthorById(int? id)
        {
            var author = new Author();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT * FROM [dbo].[User] WHERE Id={0}", id), connection);
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    author.Id = (int)reader[0];
                    author.FirstName = reader[1].ToString();
                    author.LastName = reader[2].ToString();
                    author.Age = (int)reader[3];
                    author.NickName = GetNickName((Guid)reader[5]);
                    author.Popularity = GetPopularity((Guid)reader[5]);
                }
                reader.Close();
                connection.Close();
            }
            return author;
        }

        private string GetNickName(Guid id)
        {
            string result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(string.Format("SELECT NickName FROM [dbo].[Author] WHERE Id='{0}'", id), connection);
                var reader = command.ExecuteReader();
                reader.Read();
                result = reader[0].ToString();
                reader.Close();
                connection.Close();
            }

            return result;
        }

        private decimal GetPopularity(Guid id)
        {
            string result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(string.Format("SELECT Popularity FROM [dbo].[Author] WHERE Id='{0}'", id), connection);
                using (command)
                {
                    command.CommandType = CommandType.Text;
                    result = command.ExecuteScalar().ToString();
                }
                connection.Close();
            }

            return Convert.ToDecimal(result);
        }

        public User GetRandomUser()
        {
            var user = new User();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand("SELECT * FROM [dbo].[User] ORDER BY RAND() LIMIT 1");
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    user.Id = (int)reader[0];
                    user.FirstName = reader[1].ToString();
                    user.LastName = reader[2].ToString();
                    user.Age = (int)reader[3];
                }
                reader.Close();
                connection.Close();
            }
            return user;
        }

        public int GetArticlesCount()
        {
            int result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(Id) FROM [dbo].[Article]", connection);
                result = (int)command.ExecuteScalar();

                connection.Close();
            }
            return result;
        }

        public List<Article> GetArticles()
        {
            var articles = new List<Article>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM [dbo].[Article]", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var article = new Article((int) reader[0])
                    {
                        Title = reader[1].ToString(),
                        Content = reader[2].ToString(),
                        Author = GetAuthorById(GetAuthorId((Guid) reader[3])),
                        Ratings = GetArticleRatings((int) reader[0])
                    };
                    articles.Add(article);
                }
                reader.Close();
                connection.Close();
            }
            return articles;
        }

        private List<Rating> GetArticleRatings(int id)
        {
            var ratings = new List<Rating>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(string.Format("SELECT Value FROM [dbo].[Rating] WHERE ArticleId={0}", id), connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var rating = new Rating((int)reader[0]);
                    ratings.Add(rating);
                }
                reader.Close();
                connection.Close();
            }
            return ratings;
        }

        private int GetAuthorId(Guid authorGuid)
        {
            int result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(string.Format("SELECT Id FROM [dbo].[User] WHERE AuthorId='{0}'", authorGuid), connection);
                using (command)
                {
                    command.CommandType = CommandType.Text;
                    result = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return result;
        }

        private Guid GetAuthorGuid(int id)
        {
            Guid result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(string.Format("SELECT AuthorId FROM [dbo].[User] WHERE Id={0}", id), connection);
                using (command)
                {
                    command.CommandType = CommandType.Text;
                    result = (Guid)command.ExecuteScalar();
                }
                connection.Close();
            }
            return result;
        }

        public int SaveArticle(Article article)
        {
            int articleId;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = new StringBuilder();
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendLine("DECLARE @authorId uniqueidentifier");
                cmdText.AppendFormat("SET @authorId='{0}' ", GetAuthorGuid(article.Author.Id));
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Article](Title,Content,AuthorId) VALUES('{0}','{1}','{2}') ",
                    article.Title, article.Content, GetAuthorGuid(article.Author.Id));
                cmdText.AppendLine("COMMIT");

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                var getIdCmd = "SELECT TOP 1 Id FROM [dbo].[Article] ORDER BY Id DESC";
                using (var command = new SqlCommand(getIdCmd, connection))
                {
                    command.CommandType = CommandType.Text;
                    articleId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return articleId;
        }

        public void DeleteArticle(Article article)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = string.Format("DELETE FROM [dbo].[Article] WHERE Id={0}", article.Id);

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdateArticle(Article oldArticle, Article newArticle)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = string.Format("UPDATE [dbo].[Article] SET Title='{0}',Content='{1}',Author={2} WHERE Id={3}", newArticle.Title, newArticle.Content, newArticle.Author.Id, oldArticle.Id);
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public Article GetArticleById(int? id)
        {
            var article = new Article((int)id);
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT * FROM [dbo].[Article] WHERE Id={0}", id), connection);
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    article.Title = reader[1].ToString();
                    article.Content = reader[2].ToString();
                    var author = GetAuthorById(GetAuthorId((Guid)reader[3]));
                    article.Author = author;
                    article.Ratings = GetArticleRatings((int) reader[0]);
                }
                reader.Close();
                connection.Close();
            }
            return article;
        }

        public Article GetRandomArticle()
        {
            var article = new Article();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand("SELECT TOP 1 * FROM [dbo].[Article] ORDER BY RAND()", connection);
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    article = new Article((int)reader[0]);
                    article.Title = reader[1].ToString();
                    article.Content = reader[2].ToString();
                    article.Author = GetAuthorById(GetAuthorId((Guid)reader[3]));
                    article.Ratings = GetArticleRatings((int)reader[0]);
                }
                reader.Close();
                connection.Close();
            }
            return article;
        }

        public int GetCommentsCount()
        {
            int result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT COUNT(Id) FROM [dbo].[Comments]", connection);
                result = (int)command.ExecuteScalar();

                connection.Close();
            }
            return result;
        }

        public int SaveComment(Comment comment)
        {
            int commentId;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = new StringBuilder();
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) VALUES('{0}','{1}','{2}') ",
                    comment.User.Id, comment.Article.Id, comment.Content);

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                var getIdCmd = "SELECT TOP 1 Id FROM [dbo].[Comments] ORDER BY Id DESC";
                using (var command = new SqlCommand(getIdCmd, connection))
                {
                    command.CommandType = CommandType.Text;
                    commentId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return commentId;
        }

        public int SaveReview(Review comment)
        {
            int commentId;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = new StringBuilder();
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content,Rating) VALUES({0},{1},'{2}',{3}) ",
                    comment.User.Id, comment.Article.Id, comment.Content, comment.Rating.Value);
                cmdText.AppendFormat("INSERT INTO [dbo].[Rating](UserId,ArticleId,Value) VALUES({0},{1},{2}) ",
                    comment.User.Id, comment.Article.Id, comment.Rating.Value);
                cmdText.AppendLine("COMMIT");

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                var getIdCmd = "SELECT TOP 1 Id FROM [dbo].[Comments] ORDER BY Id DESC";
                using (var command = new SqlCommand(getIdCmd, connection))
                {
                    command.CommandType = CommandType.Text;
                    commentId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return commentId;
        }

        public int SaveReviewText(ReviewText comment)
        {
            int commentId;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = new StringBuilder();
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content,Rating,RatingText) VALUES('{0}','{1}','{2}',{3},'{4}') ",
                    comment.User.Id, comment.Article.Id, comment.Content, comment.Rating.Value, comment.GetRatingValue());
                cmdText.AppendFormat("INSERT INTO [dbo].[Rating](UserId,ArticleId,Value) VALUES('{0}','{1}',{2}) ",
                    comment.User.Id, comment.Article.Id, comment.Rating.Value);
                cmdText.AppendLine("COMMIT");

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                var getIdCmd = "SELECT TOP 1 Id FROM [dbo].[Comments] ORDER BY Id DESC";
                using (var command = new SqlCommand(getIdCmd, connection))
                {
                    command.CommandType = CommandType.Text;
                    commentId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return commentId;
        }

        public BaseComment GetRandomComment()
        {
            BaseComment comment = new Comment();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand("SELECT TOP 1 * FROM [dbo].[Comments] ORDER BY RAND()", connection);
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    if ((reader[4].ToString() == "NULL") && (reader[5].ToString() == "NULL"))
                    {
                        comment = new Comment((int)reader[0]);
                        comment.Content = reader[3].ToString();
                        comment.User = GetUserById((int)reader[1]);
                        comment.Article = GetArticleById((int)reader[2]);
                    }
                    else
                    {
                        if (reader[5].ToString() == "NULL")
                        {
                            var review = new Review((int)reader[0], reader[3].ToString(), GetUserById((int)reader[1]), GetArticleById((int)reader[2]), new Rating((int)reader[4]));
                            comment = review;
                        }
                        else
                        {
                            var review = new ReviewText((int)reader[0], reader[3].ToString(), GetUserById((int)reader[1]), GetArticleById((int)reader[2]), new Rating((int)reader[4]));
                            comment = review;
                        }
                    }
                }
                reader.Close();
                connection.Close();
            }
            return comment;
        }

        public BaseComment GetCommentById(int? id)
        {
            BaseComment comment = new Comment();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT * FROM [dbo].[Comments] WHERE Id={0}", id), connection);
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[4].ToString() == "NULL")
                    {
                        comment = new Comment((int)id);
                        comment.User = GetUserById((int)reader[1]);
                        comment.Article = GetArticleById((int)reader[2]);
                        comment.Content = reader[3].ToString();
                    }
                    else
                    {
                        if (reader[5].ToString() == "NULL")
                        {
                            comment = new Review((int)reader[0], reader[3].ToString(), GetUserById((int)reader[1]), GetArticleById((int)reader[2]), new Rating((int)reader[4]));
                        }
                        else
                        {
                            comment = new ReviewText((int)reader[0], reader[3].ToString(), GetUserById((int)reader[1]), GetArticleById((int)reader[2]), new Rating((int)reader[4]));
                        }
                    }
                }
                reader.Close();
                connection.Close();
            }
            return comment;
        }

        public List<BaseComment> GetComments()
        {
            var comments = new List<BaseComment>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand("SELECT * FROM [dbo].[Comments]", connection);
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[4].Equals(DBNull.Value))
                    {
                        var comment = new Comment((int)reader[0]);
                        comment.User = GetUserById((int)reader[1]);
                        comment.Article = GetArticleById((int)reader[2]);
                        comment.Content = reader[3].ToString();
                        comments.Add(comment);
                    }
                    else
                    {
                        if (reader[5].Equals(DBNull.Value))
                        {
                            var comment = new Review((int)reader[0], reader[3].ToString(), GetUserById((int)reader[1]), GetArticleById((int)reader[2]), new Rating((int)reader[4]));
                            comments.Add(comment);
                        }
                        else
                        {
                            var comment = new ReviewText((int)reader[0], reader[3].ToString(), GetUserById((int)reader[1]), GetArticleById((int)reader[2]), new Rating((int)reader[4]));
                            comments.Add(comment);
                        }
                    }
                }
                reader.Close();
                connection.Close();
            }
            return comments;
        }

        public void DeleteComment(BaseComment comment)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = string.Format("DELETE FROM [dbo].[Comments] WHERE Id={0}", comment.Id);

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdateComment(Comment oldComment, Comment newComment)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = string.Format("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1}", newComment.Content, oldComment.Id);
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdateReview(Review oldReview, Review newReview)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = string.Format("UPDATE [dbo].[Comments] SET Content='{0}',Rating={1} WHERE Id={2}", newReview.Content, newReview.Rating.Value, oldReview.Id);
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdateReviewText(ReviewText oldReviewText, ReviewText newReviewText)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = string.Format("UPDATE [dbo].[Comments] SET Content='{0}',Rating={1},RatingText='{2}' WHERE Id={3}", newReviewText.Content, newReviewText.Rating.Value, newReviewText.GetRatingValue(), oldReviewText.Id);
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

    }
}
