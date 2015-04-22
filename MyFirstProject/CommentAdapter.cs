using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class CommentAdapter
    {
        private readonly BaseComment m_comment;

        public CommentAdapter(BaseComment comment)
        {
            m_comment = comment;
        }

        public int Id
        {
            get { return m_comment.Id; }            
        }

        public User User { get { return m_comment.User; } }

        public string Content
        {
            get { return m_comment.Content; }
        }

        public Article Article
        {
            get {return m_comment.Article;}
        }

        public override string ToString()
        {
 	        return m_comment.ToString();
        }       
    }
}
