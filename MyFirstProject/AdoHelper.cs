using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    class AdoHelper
    {
        private readonly SqlConnection _connection = new SqlConnection("Data Source=(local); Initial Catalog=MyDatabase#1;Password=test");

        public List<User> GetUsers()
        {        
            _connection.Open();
            var users = new List<User>();
            var command = new SqlCommand("SELECT * FROM User");
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User((int)reader[0]));
                users.Last().FirstName = reader[1].ToString();
                users.Last().LastName = reader[2].ToString();
                users.Last().Age = (int) reader[3];
            }
            reader.Close();
            return users;
        } 
    }
}
