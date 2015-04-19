using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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
            using (var connection = new SqlConnection(ConnectionString))
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

        public object CRUDOperation(string request, string tableName)
        {
            object elementId;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(request, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                var getIdCmd = string.Format("SELECT TOP 1 Id FROM [dbo].[{0}] ORDER BY Id DESC", tableName);
                using (var command = new SqlCommand(getIdCmd, connection))
                {
                    command.CommandType = CommandType.Text;
                    elementId = command.ExecuteScalar();
                }
                connection.Close();
            }
            return elementId;
        }

        public object GetCellValue(string tableName, string columnName, int id)
        {
            object result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT {0} FROM [dbo].[{1}] WHERE Id={2}", columnName, tableName, id), connection);
                connection.Open();
                result = cmdText.ExecuteScalar();
                connection.Close();
            }
            return result;
        }

        public object GetCellValue(string tableName, string columnName, string filterColumn, object filterValue)
        {
            object result;
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT {0} FROM [dbo].[{1}] WHERE {2}='{3}'", columnName, tableName, filterColumn, filterValue), connection);
                connection.Open();
                result = cmdText.ExecuteScalar();
                connection.Close();
            }
            return result;
        }

        public DataTable GetData(string tableName)
        {
            var table = new DataTable();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT * FROM [dbo].[{0}]", tableName), connection);
                connection.Open();
                var dataAdapter = new SqlDataAdapter(cmdText);
                dataAdapter.Fill(table);
                connection.Close();
                dataAdapter.Dispose();
            }
            return table;
        }

        public DataTable GetData(string tableName, object id)
        {
            var table = new DataTable();
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmdText = new SqlCommand(string.Format("SELECT * FROM [dbo].[{0}] WHERE Id={1}", tableName,id), connection);
                connection.Open();
                var dataAdapter = new SqlDataAdapter(cmdText);
                dataAdapter.Fill(table);
                connection.Close();
                dataAdapter.Dispose();
            }
            return table;
        }
    }
}
