using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string UserFirstName { get; set; }

        [Required]
        public string UserLastName { get; set; }

        [Required]
        public int UserAge { get; set; }

        [Required]
        public string Content { get; set; }

        public int ArticleId { get; set; }

        public object Rating { get; set; }

        public string Show()
        {
            return Rating != null ? string.Format("{0} {1}: {2} \n Rating: {3}", UserFirstName.Trim(), UserLastName.Trim(), Content.Trim(), Rating) : string.Format("{0} {1}: {2}", UserFirstName.Trim(), UserLastName.Trim(), Content.Trim());
        }
    }
}