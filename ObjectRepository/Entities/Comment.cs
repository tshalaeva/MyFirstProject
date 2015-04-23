namespace ObjectRepository.Entities
{
    public class Comment : BaseComment
    {   
        public Comment(int id)
        {
            Id = id;
        }

        public Comment() { }

        public Comment(int id, string content, User user, Article article)
        {
            Id = id;
            Article = article;
            User = user;
            Content = content;
        }

        public new int GetEntityCode()
        {
            return 0;
        }
    }
}
