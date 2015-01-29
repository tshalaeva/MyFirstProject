using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Review : IComment
    {
        public int Id { get; private set; }

        public string Content { get; set; }        

        public Article Article { get; set; }

        public Rating Rating { get; set; }

        public Review(int id)
        {
            Id = id;
        }

        public void display()
        {
            Console.WriteLine(Rating.User.FirstName + " " + Rating.User.LastName);
            Console.WriteLine(Rating.Value);
            Console.WriteLine(Content);
        }
    }
}
