using ObjectRepository.Entities;

namespace MVCProject.Adapters
{
    public class CommentAdapter
    {
        private readonly BaseComment _mComment;

        public CommentAdapter(BaseComment comment)
        {
            _mComment = comment;
        }

        public int Id
        {
            get { return _mComment.Id; }            
        }

        public User User { get { return _mComment.User; } }

        public string Content
        {
            get { return _mComment.Content; }
        }

        public Article Article
        {
            get {return _mComment.Article;}
        }

        public override string ToString()
        {
 	        return _mComment.ToString();
        }       
    }
}
