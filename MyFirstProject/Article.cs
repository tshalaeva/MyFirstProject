using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    public class Article
    {        
        public string Title {get; set;}
        
        public string Content {get; set;}
                
        public Author Author {get; set;}

        public List<Rating> Rating { get; private set; }

        public int Id {get; private set;}

        public Article(int id)
        {
            Id = id;
            Rating = new List<Rating>();             
        }

        public void AddRating(Rating rating)
        {
            bool flag = false;
            for (int i = 0; i < Rating.Count; i++)
            {
                if (Rating[i].User.Id == rating.User.Id)
                {
                    Rating[i].Value = rating.Value;
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                Rating.Add(rating);
            }
        }

        public int GetAverageRating()
        {
            int sum = 0;
            for (int i = 0; i < Rating.Count; i++)
            {
                sum = sum + Rating[i].Value;
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

        public Comment AddComment(int id, string content, User user)
        {
            Comment comment = new Comment(id); 
            comment.Article = this;
            comment.user = user;
            comment.Content = content;
            return comment;
        }
    }
}
