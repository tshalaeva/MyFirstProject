﻿namespace MyFirstProject.Entities
{
    public abstract class BaseComment:IEntity
    {        
        public string Content { get; set; }

        public User User { get; set; }

        public Article Article { get; set; }

        public int Id { get; protected set; }        

        public BaseComment()
        {}

        public override string ToString()
        {
            return string.Format("{0} {1}:\n{2}", User.FirstName, User.LastName, Content);
        }

        public int GetEntityCode()
        {
            return 0;
        }
    }
}
