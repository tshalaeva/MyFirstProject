using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstProject.Entities;
using MyFirstProject.Entities.Dto;

namespace MyFirstProject
{
    class DtoMapper
    {
        private AdoHelper _adoHelper = new AdoHelper();

        public DtoMapper() { }

        public Entities.Article GetArticle(Entities.Dto.Article article)
        {
            var result = new Entities.Article
            {
                Id = article.Id,
                Content = article.Content,
                Author = (Entities.Author)GetUserById((int)_adoHelper.GetCellValue("User", "Id", "AuthorId", article.AuthorId)),
                Title = article.Title,
                Ratings = GetArticleRatings(article.Id)
            };
            return result;
        }

        public Entities.Author GetAuthor(Entities.Dto.User author)
        {
            var result = new Entities.Author
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Age = author.Age,
                NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", author.AuthorId).ToString(),
                Popularity = (decimal)_adoHelper.GetCellValue("Author", "Popularity", "Id", author.AuthorId)
            };
            return result;
        }

        public Entities.Admin GetAdmin(Entities.Dto.User admin)
        {
            var result = new Entities.Admin(admin.Id)
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Age = admin.Age,
                Privilegies = GetPrivilegies(_adoHelper.GetCellValue("Privilegies", "List", "Id", admin.PrivilegiesId).ToString())
            };
            return result;
        }

        public Entities.User GetUser(Entities.Dto.User user)
        {
            if (user.PrivilegiesId.Equals(Guid.Empty) && user.AuthorId.Equals(Guid.Empty))
            {
                var result = new Entities.User(user.Id)
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age
                };
                return result;
            }
            else
            {
                if (!user.PrivilegiesId.Equals(Guid.Empty))
                {
                    var result = new Entities.Admin(user.Id)
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = user.Age,
                        Privilegies = GetPrivilegies(_adoHelper.GetCellValue("Privilegies", "List", "Id", user.PrivilegiesId).ToString())
                    };
                    return result;
                }
                else
                {
                    var result = new Entities.Author(user.Id)
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = user.Age,
                        NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", user.AuthorId).ToString(),
                        Popularity = Convert.ToDecimal(_adoHelper.GetCellValue("Author", "Popularity", "Id", user.AuthorId))
                    };
                    return result;
                }
            }
        }        

        public Entities.Article GetArticleById(int id)
        {
            var articleData = _adoHelper.GetData("Article", id).Rows[0];
            var article = new Entities.Article((int) articleData[0])
            {
                Author =
                    (Entities.Author)
                        GetUserById((int) _adoHelper.GetCellValue("User", "Id", "AuthorId", articleData["AuthorId"])),
                Content = articleData["Content"].ToString(),
                Title = articleData["Title"].ToString()
            };
            article.Ratings = GetArticleRatings(article.Id);
            return article;
        }

        public BaseComment GetComment(Entities.Dto.Comment comment)
        {
            var articleData = _adoHelper.GetData("Article", comment.ArticleId);
            var article = new Entities.Article((int)articleData.Rows[0][0]);
            var user = GetUserById(comment.UserId);
            if (comment.Rating == null)
            {
                var result = new Entities.Comment(comment.Id)
                {
                    Content = comment.Content,
                    Article = article,
                    User = user
                };
                return result;
            }
            if (comment.Rating is int)
            {
                var review = new Entities.Review(comment.Id)
                {
                    Article = article,
                    User = user,
                    Rating = new Rating((int) _adoHelper.GetCellValue("Rating", "Value", "CommentId", comment.Id)),
                    Content = comment.Content
                };
                return review;
            }
            var reviewText = new ReviewText(comment.Id)
            {
                Article = article,
                User = user,
                Rating = new Rating(Convert.ToInt32(_adoHelper.GetCellValue("TextRating", "Value", "CommentId", comment.Id))),
                Content = comment.Content
            };
            return reviewText;
        }

        private List<string> GetPrivilegies(string privilegiesString)
        {
            var tmp = privilegiesString.Split(' ');
            var result = new List<string>();
            foreach (var item in tmp)
            {
                if (item != "")
                {
                    if (item.Contains(","))
                    {
                        result.Add(item.Remove(item.IndexOf(',')));
                    }
                    else
                    {
                        result.Add(item);
                    }
                }
            }
            return result.ToList();
        }

        private List<Rating> GetArticleRatings(int articleId)
        {
            var result = new List<Rating>();
            var ratingData = _adoHelper.GetData("Rating");

            for(int i = 0; i < ratingData.Rows.Count; i++)
            {
                if ((int)ratingData.Rows[i]["ArticleId"] == articleId)
                {
                    result.Add(new Rating((int)ratingData.Rows[i]["Value"]));
                }
            }
            return result;
        }

        private Entities.User GetUserById(int id)
        {
            var userData = _adoHelper.GetData("User", id).Rows[0];

            if (userData["PrivilegiesId"].Equals(DBNull.Value) && userData["AuthorId"].Equals(DBNull.Value))
            {
                var user = new Entities.User((int)userData[0]);
                user.FirstName = userData["FirstName"].ToString();
                user.LastName = userData["LastName"].ToString();
                user.Age = (int)userData["Age"];
                return user;
            }
            else
            {
                if (userData["PrivilegiesId"].Equals(DBNull.Value))
                {
                    var author = new Entities.Author((int)userData["Id"]);
                    author.FirstName = userData["FirstName"].ToString();
                    author.LastName = userData["LastName"].ToString();
                    author.Age = (int)userData["Age"];
                    author.NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", userData["AuthorId"]).ToString();
                    author.Popularity = Convert.ToDecimal(_adoHelper.GetCellValue("Author", "Popularity", "Id", userData["AuthorId"]));
                    return author;
                }
                else
                {
                    var admin = new Entities.Admin((int)userData["Id"]);
                    admin.FirstName = userData["FirstName"].ToString();
                    admin.LastName = userData["LastName"].ToString();
                    admin.Age = (int)userData["Age"];
                    admin.Privilegies = GetPrivilegies(_adoHelper.GetCellValue("Privilegies", "List", "Id", userData[5]).ToString());
                    return admin;
                }
            }
        }
    }
}
