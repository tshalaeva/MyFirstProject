

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
                    if(!(reader[4] is Guid))
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

        public void SaveUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var cmdText =
                    string.Format(
                        "INSERT INTO [dbo].[User](FirstName,LastName,Age) VALUES('{0}','{1}',{2})",
                        user.FirstName, user.LastName, user.Age);

                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void SaveAdmin(Admin admin)
        {
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
                cmdText.AppendLine("COMMIT");

                using (var command = new SqlCommand(cmdText.ToString(), connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
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
                    cmdText.AppendLine("BEGIN TRANSACTION");
                    cmdText.AppendLine("DECLARE @Privilegies uniqueidentifier");
                    cmdText.AppendFormat("SET @Privilegies='{0}' ", user.Id);
                    cmdText.AppendLine(
                        "DELETE FROM [dbo].[User] WHERE Id=@Privilegies)");
                    cmdText.AppendLine("DELETE FROM [dbo].[Privilegies] WHERE Id=@Privilegies");
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
    }
}
