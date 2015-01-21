using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Comment
    {
        private int id;
        private string content;
        private User user;

        public Comment(int idValue, string contentValue, User commentUser)
        {
            id = idValue;
            content = contentValue;
            user = commentUser;
        }

        public int getId()
        {
            return id;
        }

        public string getContent()
        {
            return content;
        }

        public void setContent(string contentValue)
        {
            content = contentValue;
        }

        public User getUser()
        {
            return user;
        }
    }
}
