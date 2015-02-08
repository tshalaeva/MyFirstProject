namespace MyFirstProject.Entities
{
    public class ReviewText : Review
    {
        public ReviewText(int id) : base(id) { }

        public ReviewText(int id, string content, User user, Article article, Rating rating) : base(id, content, user, article, rating) { }

        public override string ToString()
        {
            return User.FirstName + " " + User.LastName + ":\n" + Content + " " + ConvertRatingValue();
        }

        private string ConvertRatingValue()
        {
            string rating = "";
            switch (Rating.Value)
            {
                case 1:
                    {
                        rating = "Very bad";
                        break;
                    }

                case 2:
                    {
                        rating = "Bad";
                        break;
                    }

                case 3:
                    {
                        rating = "Satisfactorily";
                        break;
                    }

                case 4:
                    {
                        rating = "Good";
                        break;
                    }

                case 5:
                    {
                        rating = "Fine";
                        break;
                    }
            }

            return rating;
        }
    }
}
