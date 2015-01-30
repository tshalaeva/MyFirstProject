using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Review : Entity, IComment
    {       
        public string Content { get; set; }        

        public Article Article { get; set; }

        public Rating Rating { get; set; }

        public User User { get; set; }

        public Review(int id) : base(id)
        {
        }

        public void Display()
        {
            Console.WriteLine(User.FirstName + " " + User.LastName);
            Console.WriteLine(Rating.Value);
            Console.WriteLine(Content);
        }

        public bool IsReview()
        {
            return true;
        }
    }
}
