using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Rating
    {        
        public int Value {get; set;}
                
        public User User {get; private set;}
        
        public Rating(int value, User user)
        {
            this.Value = value;
            User = user;
        }

        public void setRating(int value, User user)
        {
            if (value > 5)
            {
                value = 5;
            }
            if (value < 1)
            {
                value = 1;
            }
            this.Value = value;
            User = user;
        }
    }
}
