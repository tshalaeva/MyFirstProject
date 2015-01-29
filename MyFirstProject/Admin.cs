using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Admin : User
    {
        private List<string> privilegies;
        public List<string> Privilegies
        {
            get 
            {
                return privilegies;
            }
            set
            {
                privilegies = value;
            }
        }

        public Admin(int id)
            : base(id)
        {
            privilegies = new List<string>();
        }
    }
}
