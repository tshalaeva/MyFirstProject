namespace MVCProject.Models
{
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

        public string Show()
        {
            return string.Format("{0} {1}: {2}", UserFirstName, UserLastName, Content);
        }
    }
}