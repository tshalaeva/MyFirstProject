using System.Collections.Generic;

namespace MVCProject.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public ArticleViewModel()
        {
        }

        public string Author { get; set; }

        public string Title
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public List<CommentViewModel> Comments { get; set; }

        public CommentViewModel NewComment { get; set; }
    }
}