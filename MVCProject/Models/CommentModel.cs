using ObjectRepository.Entities;

namespace MVCProject.Models
{
    public class CommentModel
    {
        private readonly BaseComment _mComment;

        public int Id
        {
            get { return _mComment.Id; }            
        }

        public User User { get { return _mComment.User; } }

        public ArticleModel Article 
        {
            get { return new ArticleModel(_mComment.Article); }
        }

        public string Content 
        {
            get { return _mComment.Content; }
        }

        public CommentModel(BaseComment comment)
        {
            _mComment = comment;
        }

        public string Show()
        {
            return _mComment.ToString();
        }
    }
}