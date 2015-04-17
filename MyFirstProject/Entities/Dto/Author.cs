using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject.Entities.Dto
{
    class Author : User
    {
        public int AuthorId { get; set; }

        public string NickName { get; set; }

        public decimal Popularity { get; set; }
    }
}
