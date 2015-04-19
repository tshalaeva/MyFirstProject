namespace MyFirstProject.Entities.Dto
{
    class Comment
    {
        public string Content { get; set; }

        public int UserId { get; set; }

        public int ArticleId { get; set; }

        public int Id { get; set; }

        public object Rating { get; set; }
    }
}
