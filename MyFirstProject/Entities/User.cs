﻿namespace MyFirstProject.Entities
{
    public class User:IEntity
    {
        public User(int idValue)
        {
            Id = idValue;
        }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public int Age { get; set; }

        public int Id { get; private set; }        
    }
}