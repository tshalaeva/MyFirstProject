using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject.Entities;

namespace MyFirstProject
{
    public class ArticleAdapter
    {
        private readonly Article m_article;
        
        public ArticleAdapter(Article article)
        {
            m_article = article;
        }        

        public string Title
        {
            get { return m_article.Title; }            
        }

        public string Author
        {
            get { return m_article.Author.NickName; }                         
        }

        public string Content
        {
            get { return m_article.Content; }            
        }

        public int Id
        {
            get { return m_article.Id; }
        }
    }
}
