using System.Collections.Generic;
using System.Linq;
using ObjectRepository.Entities;
using Comment = Dto.DtoEntities.Comment;

//using Article = MyFirstProject.Entities.Dto.Article;
//using Comment = MyFirstProject.Entities.Dto.Comment;
//using User = MyFirstProject.Entities.Dto.User;

namespace Dto
{
    public class DtoMapper
    {
        //private AdoHelper _adoHelper = new AdoHelper();

        public Article GetArticle(DtoEntities.Article article)
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

        public Author GetAuthor(DtoEntities.User author)
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

        public Admin GetAdmin(DtoEntities.User admin)
        {
            var result = new Admin(admin.Id)
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Age = admin.Age,
                //Privilegies = GetPrivilegies(_adoHelper.GetCellValue("Privilegies", "List", "Id", admin.PrivilegiesId).ToString())
                Privilegies = GetPrivilegies(admin.Privilegies)
            };
            return result;
        }

        public User GetUser(DtoEntities.User user)
        {
            //if (user.PrivilegiesId.Equals(Guid.Empty) && user.AuthorId.Equals(Guid.Empty))
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
            //if (!user.PrivilegiesId.Equals(Guid.Empty))
            if(user.Privilegies != null)
            {
                var result = new Admin(user.Id)
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    Privilegies = GetPrivilegies(user.Privilegies)
                    //Privilegies = GetPrivilegies(_adoHelper.GetCellValue("Privilegies", "List", "Id", user.PrivilegiesId).ToString())
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
                    //NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", user.AuthorId).ToString(),
                    //Popularity = Convert.ToDecimal(_adoHelper.GetCellValue("Author", "Popularity", "Id", user.AuthorId))
                    NickName = user.NickName,
                    Popularity = user.Popularity
                };
                return result;
            }
        }

        //public Article GetArticleById(int id)
        //{
        //    var articleData = _adoHelper.GetData("Article", id).Rows[0];
        //    var article = new Article((int) articleData[0])
        //    {
        //        Author =
        //            (Author)
        //                GetUserById((int) _adoHelper.GetCellValue("User", "Id", "AuthorId", articleData["AuthorId"])),
        //        Content = articleData["Content"].ToString(),
        //        Title = articleData["Title"].ToString()
        //    };
        //    article.Ratings = GetArticleRatings(article.Id);
        //    return article;
        //}

        public BaseComment GetComment(Comment comment)
        {
            //var articleData = _adoHelper.GetData("Article", comment.ArticleId);
            //var article = new Article((int)articleData.Rows[0][0]);
            //var user = GetUserById(comment.UserId);
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
                    //Rating = new Rating((int) _adoHelper.GetCellValue("Rating", "Value", "CommentId", comment.Id)),
                    Rating = new Rating((int)comment.Rating),
                    Content = comment.Content
                };
                return review;
            }
            var reviewText = new ReviewText(comment.Id)
            {
                Article = new Article(comment.ArticleId),
                User = new User(comment.UserId),
                //Rating = new Rating(Convert.ToInt32(_adoHelper.GetCellValue("TextRating", "Value", "CommentId", comment.Id))),
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

        //private User GetUserById(int id)
        //{
        //    var userData = _adoHelper.GetData("User", id).Rows[0];

        //    if (userData["PrivilegiesId"].Equals(DBNull.Value) && userData["AuthorId"].Equals(DBNull.Value))
        //    {
        //        var user = new User((int)userData[0]);
        //        user.FirstName = userData["FirstName"].ToString();
        //        user.LastName = userData["LastName"].ToString();
        //        user.Age = (int)userData["Age"];
        //        return user;
        //    }
        //    else
        //    {
        //        if (userData["PrivilegiesId"].Equals(DBNull.Value))
        //        {
        //            var author = new Author((int)userData["Id"]);
        //            author.FirstName = userData["FirstName"].ToString();
        //            author.LastName = userData["LastName"].ToString();
        //            author.Age = (int)userData["Age"];
        //            author.NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", userData["AuthorId"]).ToString();
        //            author.Popularity = Convert.ToDecimal(_adoHelper.GetCellValue("Author", "Popularity", "Id", userData["AuthorId"]));
        //            return author;
        //        }
        //        else
        //        {
        //            var admin = new Admin((int)userData["Id"]);
        //            admin.FirstName = userData["FirstName"].ToString();
        //            admin.LastName = userData["LastName"].ToString();
        //            admin.Age = (int)userData["Age"];
        //            admin.Privilegies = GetPrivilegies(_adoHelper.GetCellValue("Privilegies", "List", "Id", userData[5]).ToString());
        //            return admin;
        //        }
        //    }
        //}
    }
}
