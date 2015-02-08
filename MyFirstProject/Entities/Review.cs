namespace MyFirstProject.Entities
{
    public class Review : BaseComment
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
            return string.Format("{0} \nRating: {1}", base.ToString(), Rating.Value.ToString());
        }
    }
}
