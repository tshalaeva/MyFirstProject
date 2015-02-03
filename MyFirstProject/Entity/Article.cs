using System.Collections.Generic;
using System.Linq;

namespace MyFirstProject.Entity
{
    public class Article : IEntity
    {
        public Article(int id)
        {
            Id = id;
            Rating = new List<Rating>();
        }

        public string Title { get; set; }
        
        public string Content { get; set; }
                
        public Author Author { get; set; }

        public List<Rating> Rating { get; private set; }

        public int Id { get; set; }        

        public void AddRating(Rating rating)
        {
            rating.SetRating(rating.Value);
            Rating.Add(rating);
        }

        public int GetAverageRating()
        {
            var sum = Rating.Aggregate(0, (current, rating) => current + rating.Value);
            if (Rating.Count != 0)
            {
                return sum / Rating.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}
