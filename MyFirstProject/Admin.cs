using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Admin : User
    {
        public List<string> Privilegies { get; set; }

        public Admin(int id)
            : base(id)
        {            
        }
    }
}
