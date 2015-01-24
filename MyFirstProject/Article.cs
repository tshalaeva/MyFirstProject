using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstProject
{
    class Article
    {        
        public string Title {get; set;}
        
        public string Content {get; set;}
                
        public Author Author {get; set;}      

        private List<Rating>  rating;
        public List<Rating> Rating
        {
            get
            {
                return rating;
            }
        }

        private int id;
        public int Id
        {
            get
            {
                return id;
            }
        }

        public Article(int id)
        {
            this.id = id;                 
            rating = new List<Rating>();
        }

        public void setRating(Rating rating)
        {
            bool flag = false;
            for(int i = 0; i < this.rating.Count; i++)
            {
                if (this.rating[i].User.Id == rating.User.Id)
                {
                    this.rating[i].Value = rating.Value;
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                this.rating.Add(rating);
            }
        }

        public int getAverageRating()
        {
            int sum = 0;
            for (int i = 0; i < rating.Count; i++)
            {
                sum = sum + rating[i].Value;
            }
            if (rating.Count != 0)
            {
                return sum / rating.Count;
            }
            else
            {
                return 0;
            }
        }

        public Comment addComment(int id, string content, User user)
        {
            Comment comment = new Comment(id); 
            comment.Article = this;
            comment.User = user;
            comment.Content = content;
            return comment;
        }
    }
}
