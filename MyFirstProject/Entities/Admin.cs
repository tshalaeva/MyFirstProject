using System.Collections.Generic;

namespace MyFirstProject.Entities
{
    public class Admin : User
    {
        public Admin(int id)
            : base(id)
        {
        }

        public List<string> Privilegies { get; set; }        
    }
}
