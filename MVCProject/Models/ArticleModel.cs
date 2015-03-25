using System.Collections.Generic;
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