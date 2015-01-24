using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Rating
    {        
        public int Value {get; set;}
        
        private User user;
        public User User
        {
            get
            {
                return user;
            }
        }

        public Rating(int value, User user)
        {
            this.Value = value;
            this.user = user;
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
            this.user = user;
        }
    }
}
