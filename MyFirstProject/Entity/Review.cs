using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Review : BaseComment, IEntity
    {       
        public Rating Rating { get; set; }

        public Review(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return base.ToString() + "\n" + "Rating: " + Rating.Value.ToString();
        }

        public override bool IsReview()
        {
            return true;
        }
    }
}
