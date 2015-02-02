using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public abstract class BaseComment 
    {        
        public string Content { get; set; }

        public User User { get; set; }

        public Article Article { get; set; }

        public int Id { get; set; }

        public override string ToString()
        {
            return User.FirstName + " " + User.LastName + ":\n" + Content;
        }

        public void Display()
        {
            Console.WriteLine(ToString());
        }

        public virtual bool IsReview()
        {
            return true; //
        }
    }
}
