namespace MyFirstProject.Entity
{
    public class Review : BaseComment, IEntity
    {
        public Review(int id)
        {
            Id = id;
        }

        public Review() { }

        public Review(int id, string content, User user, Article article, Rating rating)
        {
            Id = id;
            Article = article;
            User = user;            
            Content = content;
            Rating = rating;       
        }

        public Rating Rating { get; set; }        

        public override string ToString()
        {
            return base.ToString() + "\n" + "Rating: " + Rating.Value.ToString();
        }
    }
}
