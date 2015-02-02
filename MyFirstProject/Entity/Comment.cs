using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Comment : BaseComment, IEntity
    {   
        public Comment(int id)
        {
            Id = id;
        }

        public override bool IsReview()
        {
            return false;
        }
    }
}
