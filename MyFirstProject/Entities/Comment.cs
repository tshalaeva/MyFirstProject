namespace MyFirstProject.Entities
{
    public class Comment : BaseComment, IEntity
    {   
        public Comment(int id)
        {
            Id = id;
        }

        public Comment(int id, string content, User user, Article article)
        {
            Id = id;
            Article = article;
            User = user;
            Content = content;
        }
    }
}
