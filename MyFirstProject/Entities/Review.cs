﻿namespace MyFirstProject.Entities
{
    public class Review : BaseComment
    {
        public Review(int id)
        {
            Id = id;
        }

        public Review(int id, string content, User user, Article article, Rating rating)
        {
            Id = id;
            Article = article;
            User = user;            
            Content = content;
            Rating = rating;
            Article.AddRating(Rating);
        }

        protected Rating Rating { get; private set; }        

        public override string ToString()
        {
            return string.Format("{0} \nRating: {1}", base.ToString(), Rating.Value.ToString());
        }

        public new int GetEntityCode()
        {
            return 1;
        }
    }
}
