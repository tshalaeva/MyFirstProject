using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Article : IEntity
    {        
        public string Title {get; set;}
        
        public string Content {get; set;}
                
        public Author Author {get; set;}

        public List<Rating> Rating { get; private set; }

        public int Id { get; set; }

        public Article(int id)
        {
            Id = id;
            Rating = new List<Rating>();             
        }

        public void AddRating(Rating rating)
        {
            rating.SetRating(rating.Value);
            Rating.Add(rating);
        }

        public int GetAverageRating()
        {
            int sum = 0;
            foreach (Rating rating in Rating)
            {
                sum = sum + rating.Value;
            }
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
