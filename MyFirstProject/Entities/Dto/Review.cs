using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject.Entities.Dto
{
    class Review : Comment
    {
        public Review() { }

        public int Rating { get; set; }
    }
}
