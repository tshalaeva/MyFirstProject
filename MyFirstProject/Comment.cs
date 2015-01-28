using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Comment
    {        
        public int Id {get; private set;}        
        
        public string Content {get; set;}
        
        public User user {get; set;}
        
        public Article Article {get; set;}

        public Comment(int id)
        {
            Id = id;
        }
    }
}
