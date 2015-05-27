using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        [Required]
        public string Title
        {
            get;
            set;
        }

        [Required]
        public string Content
        {
            get;
            set;
        }

        public List<CommentViewModel> Comments { get; set; }

        public CommentViewModel NewComment { get; set; }
    }
}