using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyFirstProject.Entities;

namespace MVCProject.Models
{
    public class ArticleModel
    {
        private readonly Article m_article;

        public List<CommentModel> Comments { get; set; }

        public ArticleModel(Article article)
        {
            m_article = article;
        }

        public string Title
        {
            get { return m_article.Title; }
            set { throw new NotImplementedException(); }
        }

        public string Author
        {
            get { return m_article.Author.NickName; }             
            set {}
        }

        public string Content
        {
            get { return m_article.Content; }
            set {}
        }

        public string AverageRating
        {
            get { return m_article.GetAverageRating().ToString(); }
        }

        public int Id
        {
            get { return m_article.Id; }
        } 
    }
}