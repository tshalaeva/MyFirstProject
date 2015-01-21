using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Author : User
    {
        private string nickName;
        private double popularity;

        public Author(int id, string firstName, string lastName, int age, string nickNameValue, double popularityValue)
            : base(id, firstName, lastName, age)
        {
            nickName = nickNameValue;
            popularity = popularityValue;
        }

        public string getNickName()
        {
            return nickName;
        }

        public void setNickName(string nickNameValue)
        {
            nickName = nickNameValue;
        }

        public double getPopularity()
        {
            return popularity;
        }

        public void setPopularity(double popularityValue)
        {
            popularity = popularityValue;
        }
    }
}
