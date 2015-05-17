namespace MVCProject.Models
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
        }

        public int Id { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public int UserAge { get; set; }

        public string Content { get; set; }

        public int ArticleId { get; set; }

        public object Rating { get; set; }

        public string Show()
        {
            return Rating != null ? string.Format("{0} {1}: {2} \n Rating: {3}", UserFirstName.Trim(), UserLastName.Trim(), Content.Trim(), Rating) : string.Format("{0} {1}: {2}", UserFirstName.Trim(), UserLastName.Trim(), Content.Trim());
        }
    }
}