using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Rating
    {        
        public int Value {get; private set;}                        
        
        public Rating(int value)
        {
            this.Value = value;         
        }

        public void SetRating(int value)
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
        }
    }
}
