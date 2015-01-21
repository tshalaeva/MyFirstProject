using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Admin : User
    {
        private string[] privilegies;

        public Admin(int id, string firstName, string lastName, int age, string[] privilegiesValues)
            : base(id, firstName, lastName, age)
        {
            privilegies = privilegiesValues;
        }

        public string[] getPrivilegies()
        {
            return privilegies;
        }

        public void printListOfPrivilegies()
        {

        }
    }
}
