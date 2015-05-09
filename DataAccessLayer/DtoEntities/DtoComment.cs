namespace DataAccessLayer.DtoEntities
{
    public class DtoComment
    {
        public string Content { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserLastName { get; set; }

        public int UserAge { get; set; }

        public int ArticleId { get; set; }

        public int Id { get; set; }

        public object Rating { get; set; }
    }
}
