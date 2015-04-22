using MyFirstProject.Entities;
using MyFirstProject;

namespace MVCProject.Models
{
    public class CommentModel
    {
        private readonly CommentAdapter m_adapter;

        public int Id
        {
            get { return m_adapter.Id; }            
        }

        public User User { get { return m_adapter.User; } }

        public ArticleModel Article 
        {
            get { return new ArticleModel(new ArticleAdapter(m_adapter.Article)); }
        }

        public string Content 
        {
            get { return m_adapter.Content; }
        }

        public CommentModel(CommentAdapter comment)
        {
            m_adapter = comment;
        }

        public string Show()
        {
            return m_adapter.ToString();
        }
    }
}