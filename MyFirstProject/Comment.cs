using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Comment
    {
        private int id;
        public int Id 
        {
            get
            {
                return id;
            }
        }
        
        public string Content {get; set;}
        
        public User User {get; set;}
        
        public Article Article {get; set;}

        public Comment(int id)
        {
            this.id = id;
        }
    }
}
