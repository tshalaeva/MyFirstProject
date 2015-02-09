using System.Collections.Generic;

namespace MyFirstProject.Entities
{
    public class ReviewText : Review
    {
        private static readonly Dictionary<int, string> s_ratingTexts = new Dictionary<int, string> { { 1, "Very bad" }, { 2, "Bad" }, { 3, "Satisfactorily" }, { 4, "Good" }, { 5, "Fine" } };

        public ReviewText(int id) : base(id) { }

        public ReviewText(int id, string content, User user, Article article, Rating rating) : base(id, content, user, article, rating) { }        

        public override string ToString()
        {
            return string.Format("{0} {1}:\n{2}\nRating: {3}", User.FirstName, User.LastName, Content, ConvertRatingValue());
        }

        private string ConvertRatingValue()
        {
            string result;
            s_ratingTexts.TryGetValue(Rating.Value, out result);
            return result;
        }

        public new int GetEntityCode()
        {
            return 2;
        }
    }
}
