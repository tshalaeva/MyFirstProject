namespace MyFirstProject.Entity
{
    public class Review : BaseComment, IEntity
    {
        public Review(int id)
        {
            Id = id;
        }

        public Rating Rating { get; set; }        

        public override string ToString()
        {
            return base.ToString() + "\n" + "Rating: " + Rating.Value.ToString();
        }

        public override bool IsReview()
        {
            return true;
        }
    }
}
