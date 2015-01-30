using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Comment : Entity, IComment
    {                           
        public string Content {get; set;}
        
        public User User {get; set;}

        public Article Article { get; set; }

        public Comment(int id)
        {
            Id = id;
        }

        public void Display()
        {
            Console.WriteLine(User.FirstName + " " + User.LastName);
            Console.WriteLine(Content);
        }

        public bool IsReview()
        {
            return false;
        }
    }
}
