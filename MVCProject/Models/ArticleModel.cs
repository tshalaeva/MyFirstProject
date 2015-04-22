using System.Collections.Generic;
using MyFirstProject;

namespace MVCProject.Models
{
    public class ArticleModel
    {
        private readonly ArticleAdapter m_adapter;

        public List<CommentModel> Comments { get; set; }

        public ArticleModel(ArticleAdapter adapter)
        {
            m_adapter = adapter;
        }

        public string Title
        {
            get { return m_adapter.Title; }            
        }

        public string Author
        {
            get { return m_adapter.Author; }                         
        }

        public string Content
        {
            get { return m_adapter.Content; }            
        }

        public int Id
        {
            get { return m_adapter.Id; }
        } 
    }
}