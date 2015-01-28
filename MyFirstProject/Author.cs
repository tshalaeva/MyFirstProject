using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Author : User
    {        
        public string NickName {get; set;}
        
        public double Popularity {get; set;}

        public Author(int id) : base(id)
        {
        }
    }
}
