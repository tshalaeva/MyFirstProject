using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class User : IEntity
    {        
        public string FirstName {get; set;}
        
        public string LastName {get; set;}
        
        public int Age {get; set;}

        public int Id { get; set; }

        public User(int idValue)
        {
            Id = idValue;
        }
    }
}
