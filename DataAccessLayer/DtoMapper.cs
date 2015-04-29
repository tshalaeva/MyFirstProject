using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;
using Comment = DataAccessLayer.DtoEntities.DtoComment;

namespace DataAccessLayer
{
    public class DtoMapper
    {
        public Article GetArticle(DtoArticle article)
        {
            var result = new Article
            {
                Id = article.Id,
                Content = article.Content,
                Author = new Author(article.AuthorId),
                Title = article.Title,
                Ratings = GetArticleRatings(article.Ratings)
            };
            return result;
        }

        public Author GetAuthor(DtoUser author)
        {
            var result = new Author
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Age = author.Age,
                NickName = author.NickName,
                Popularity = author.Popularity
            };
            return result;
        }

        public Admin GetAdmin(DtoUser admin)
        {
            var result = new Admin(admin.Id)
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Age = admin.Age,
                Privilegies = GetPrivilegies(admin.Privilegies)
            };
            return result;
        }

        public User GetUser(DtoUser user)
        {
            if(user.Privilegies == null && user.NickName == null)
            {
                var result = new User(user.Id)
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age
                };
                return result;
            }
            if(user.Privilegies != null)
            {
                var result = new Admin(user.Id)
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    Privilegies = GetPrivilegies(user.Privilegies)
                };
                return result;
            }
            else
            {
                var result = new Author(user.Id)
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    NickName = user.NickName,
                    Popularity = user.Popularity
                };
                return result;
            }
        }

        public BaseComment GetComment(Comment comment)
        {
            if (comment.Rating == null)
            {
                var result = new ObjectRepository.Entities.Comment(comment.Id)
                {
                    Content = comment.Content,
                    Article = new Article(comment.ArticleId),
                    User = new User(comment.UserId)
                };
                return result;
            }
            if (comment.Rating is int)
            {
                var review = new Review(comment.Id)
                {
                    Article = new Article(comment.ArticleId),
                    User = new User(comment.UserId),
                    Rating = new Rating((int)comment.Rating),
                    Content = comment.Content
                };
                return review;
            }
            var reviewText = new ReviewText(comment.Id)
            {
                Article = new Article(comment.ArticleId),
                User = new User(comment.UserId),
                Rating = new Rating(ConvertRating(comment.Rating.ToString())),
                Content = comment.Content
            };
            return reviewText;
        }

        private int ConvertRating(string rating)
        {
            if (rating == "Very bad")
            {
                return 1;
            }
            if (rating == "Bad")
            {
                return 2;
            }
            if (rating == "Satisfactorily")
            {
                return 3;
            }
            if (rating == "Good")
            {
                return 4;
            }
            return 5;
        }

        private List<string> GetPrivilegies(string privilegiesString)
        {
            var tmp = privilegiesString.Split(' ');
            var result = (from item in tmp where item != "" select item.Contains(",") ? item.Remove(item.IndexOf(',')) : item).ToList();
            return result.ToList();
        }

        private List<Rating> GetArticleRatings(List<object> ratings)
        {
            var result = new List<Rating>();

            foreach (var value in ratings)
            {
                if (value is string)
                {
                    var intRating = ConvertRating(value.ToString());
                    result.Add(new Rating(intRating));
                    continue;
                }
                result.Add(new Rating((int)value));
            }
            return result;
        }        
    }
}
