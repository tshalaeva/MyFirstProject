﻿using System.Collections.Generic;
using System.Linq;

namespace ObjectRepository.Entities
{
    public class Article:IEntity
    {
        public Article(int id)
        {
            Id = id;
            Ratings = new List<Rating>();
        }

        public Article()
        {
            Ratings = new List<Rating>();
        }

        public string Title { get; set; }
        
        public string Content { get; set; }
                
        public Author Author { get; set; }

        public List<Rating> Ratings { get; set; }

        public int Id { get; set; }        

        public void AddRating(Rating rating)
        {
            rating.SetRating(rating.Value);
            Ratings.Add(rating);
        }

        public int GetAverageRating()
        {
            var sum = Ratings.Aggregate(0, (current, rating) => current + rating.Value);
            if (Ratings.Count != 0)
            {
                return sum / Ratings.Count;
            }
            return 0;
        }
    }
}
