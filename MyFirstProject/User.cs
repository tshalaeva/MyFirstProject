using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class User
    {        
        public string FirstName {get; set;}
        
        public string LastName {get; set;}
        
        public int Age {get; set;}
        
        protected int id;
        public int Id
        {
            get
            {
                return id;
            }
        }

        public User(int idValue)
        {
            id = idValue;
        }
    }
}
