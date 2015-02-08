using System;

namespace MyFirstProject.Entities
{
    public abstract class BaseComment:IEntity
    {        
        public string Content { get; set; }

        public User User { get; set; }

        public Article Article { get; set; }

        public int Id { get; protected set; }

        public override string ToString()
        {
            return User.FirstName + " " + User.LastName + ":\n" + Content;
        }

        public void Display()
        {
            Console.WriteLine(ToString());
        }
    }
}
