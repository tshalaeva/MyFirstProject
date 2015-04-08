

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
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
                        users.Add((User)admin);
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
                cmdText.AppendFormat("INSERT INTO Privilegies(Id,List) VALUES (@Privilegies, '{0}') ", admin.ToString());
                cmdText.AppendLine("SELECT Id FROM [dbo].[User]");
                cmdText.AppendLine("COMMIT");

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    adminId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return adminId;
        }

        private List<string> GetPrivilegies(Guid id)
        {
            var result = new List<string>();
            var privilegiesString = "";
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
            var result = 0;
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
                var cmdText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={3} WHERE Id={4}", newUser.FirstName, newUser.LastName, newUser.Age, oldUser.Id);
                connection.Open();

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public User GetUserById(int? id)
        {
            User user = new User((int)id);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {                
                var cmdText = new SqlCommand(string.Format("SELECT * FROM [dbo].[User] WHERE Id={0}", id));
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[4].ToString() == "NULL")
                    {
                        user.FirstName = reader[1].ToString();
                        user.LastName = reader[2].ToString();
                        user.Age = (int)reader[3];
                    }
                    else
                    {
                        var admin0 = new Admin((int)id);
                        admin0.FirstName = reader[1].ToString();
                        admin0.LastName = reader[2].ToString();
                        admin0.Age = (int)reader[3];
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

        public User GetRandomUser()
        {
            var user = new User();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
            var result = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM [dbo].[Article]", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var article = new Article((int)reader[0]);
                    article.Title = reader[1].ToString();
                    article.Content = reader[2].ToString();
                    article.Author = (Author)GetUserById((int)reader[3]);
                    articles.Add(article);
                }
                reader.Close();
                var getAuthorCommand = new SqlCommand("SELECT * FROM [dbo].[User]", connection);
                DataTable table = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                getAuthorCommand.CommandType = CommandType.Text;
                getAuthorCommand.Connection = connection;
                sda.SelectCommand = getAuthorCommand;
                sda.Fill(table);
                for (int i = 0; i < articles.Count; i++)
                {
                    var author = new Author();
                    author.Id = (int)table.Rows[i]["Id"];
                    author.FirstName = table.Rows[i]["FirstName"].ToString();
                    author.LastName = table.Rows[i]["LastName"].ToString();
                    author.Age = (int)table.Rows[i]["Age"];
                    articles[i].Author = author;
                }
                connection.Close();
            }
            return articles;
        }

        public int SaveArticle(Article article)
        {
            int articleId;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = new StringBuilder();
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Article](Title,Content,AuthorId) VALUES('{0}','{1}',{2})",
                    article.Title, article.Content, article.Author.Id);

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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = string.Format("UPDATE [dbo].[Article] SET Title='{0}',Content='{1}',Author={3} WHERE Id={4}", newArticle.Title, newArticle.Content, newArticle.Author.Id, oldArticle.Id);
                connection.Open();

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public Article GetArticleById(int? id)
        {
            Article article = new Article((int)id);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT * FROM [dbo].[Article] WHERE Id={0}", id));
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    article.Title = reader[1].ToString();
                    article.Content = reader[2].ToString();
                    var author = GetUserById((int)reader[3]);
                    article.Author = (Author)author;
                }
                reader.Close();
                connection.Close();
            }
            return article;
        }

        public Article GetRandomArticle()
        {
            Article article = new Article();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand("SELECT * FROM [dbo].[Article] ORDER BY RAND() LIMIT 1");
                connection.Open();

                var reader = cmdText.ExecuteReader();
                while (reader.Read())
                {
                    article = new Article((int)reader[0]);
                    article.Title = reader[1].ToString();
                    article.Content = reader[2].ToString();
                    article.Author = (Author)GetUserById((int)reader[3]);
                }
                reader.Close();
                connection.Close();
            }
            return article;
        }
    }
}
