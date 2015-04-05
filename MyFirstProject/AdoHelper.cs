

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
        //private readonly SqlConnection _connection = new SqlConnection(@"Data Source=(local)\..\MyDatabase#1.sdf");
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
                    users.Add(new User((int)reader[0]));
                    users.Last().FirstName = reader[1].ToString();
                    users.Last().LastName = reader[2].ToString();
                    users.Last().Age = (int)reader[3];
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
                        "INSERT INTO [dbo].[User](Id,FirstName,LastName,Age) VALUES(NEWID(),'{0}','{1}',{2})",
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
                    "INSERT INTO [dbo].[User](Id,FirstName,LastName,Age,PrivilegiesId) VALUES (NEWID(),'{0}','{1}',{2},@Privilegies) ",
                    admin.FirstName, admin.LastName, admin.Age);
                cmdText.AppendFormat("INSERT INTO Privilegies(Id,List) VALUES (@Privilegies, '{0}') ", admin.Privilegies.ToString());
                cmdText.AppendLine("COMMIT");

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
