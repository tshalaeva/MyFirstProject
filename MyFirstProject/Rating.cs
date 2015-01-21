﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Rating
    {
        private int value;        
        private User user;

        public Rating(int val, User ratingUser)
        {
            value = val;
            user = ratingUser;
        }

        public int getRating()
        {
            return value;
        }

        public void setRating(int ratingValue, User ratingUser)
        {
            if (ratingValue > 5)
            {
                ratingValue = 5;
            }
            if (ratingValue < 1)
            {
                ratingValue = 1;
            }
            value = ratingValue;
            user = ratingUser;
        }

        public void setRating(int ratingValue)
        {
            value = ratingValue;
        }

        public User getUser()
        {
            return user;
        }
    }
}
