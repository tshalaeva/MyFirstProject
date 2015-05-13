using ObjectRepository.Entities;

namespace MVCProject.Models
{
    public class CommentModel
    {
        private readonly BaseComment _mComment;

        public CommentModel()
        {
        }

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

    public class CommentViewModel
    {
        public CommentViewModel()
        {
        }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public int UserAge { get; set; }

        public string Content { get; set; }

        public int ArticleId { get; set; }
    }
}