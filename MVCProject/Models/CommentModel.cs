using MyFirstProject.Entities;

namespace MVCProject.Models
{
    public class CommentModel
    {
        private readonly BaseComment m_comment;
        public int Id
        {
            get { return m_comment.Id; }            
        }

        public User User { get { return m_comment.User; } }

        public ArticleModel Article {
            get { return new ArticleModel(m_comment.Article); }
        }

        public string Content {
            get { return m_comment.Content; }
        }

        public CommentModel(BaseComment comment)
        {
            m_comment = comment;
        }

        public string Show()
        {
            return m_comment.ToString();
        }
    }
}