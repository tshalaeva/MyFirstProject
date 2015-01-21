using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class User
    {
        protected string firstName;
        protected string lastName;
        protected int age;
        protected int id;

        public User(int idValue, string firstNameValue, string lastNameValue, int ageValue)
        {
            id = idValue;
            firstName = firstNameValue;
            lastName = lastNameValue;
            age = ageValue;
        }

        public string getFirstName()
        {
            return firstName;
        }

        public void setFirstName(string name)
        {
            firstName = name;
        }

        public string getLastName()
        {
            return lastName;
        }

        public void setLastName(string name)
        {
            lastName = name;
        }

        public int getAge()
        {
            return age;
        }

        public void setAge(int ageValue)
        {
            age = ageValue;
        }

        public int getId()
        {
            return id;
        }
    }
}
