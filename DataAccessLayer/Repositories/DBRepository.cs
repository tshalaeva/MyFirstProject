using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Dto;
using ObjectRepository.Entities;
//using Comment = MyFirstProject.Entities.Dto.Comment;

namespace DataAccessLayer.Repositories
{
    class DbUserRepository : IRepository<User>
    {
        private readonly AdoHelper _adoHelper;

        private readonly DtoMapper _dtoMapper;

        public DbUserRepository()
        {
            _adoHelper = new AdoHelper();
            _dtoMapper = new DtoMapper();
        }

        public bool Initialized
        {
            get
            {
                return _adoHelper.GetData("User").Rows.Count != 0;
            }
        }

        public List<User> Get()
        {
            var userTable = _adoHelper.GetData("User");
            var users = new List<User>();
            for (var i = 0; i < userTable.Rows.Count; i++)
            {
                var tmpUser = new Dto.DtoEntities.User
                {
                    Id = (int)userTable.Rows[i]["Id"],
                    FirstName = userTable.Rows[i]["FirstName"].ToString(),
                    LastName = userTable.Rows[i]["LastName"].ToString(),
                    Age = (int)userTable.Rows[i]["Age"]
                };
                if (!userTable.Rows[i]["PrivilegiesId"].Equals(DBNull.Value))
                {
                    tmpUser.Privilegies = _adoHelper.GetCellValue("Privilegies", "List", "Id", userTable.Rows[i]["PrivilegiesId"]).ToString();
                }
                if (!userTable.Rows[i]["AuthorId"].Equals(DBNull.Value))
                {
                    tmpUser.NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", userTable.Rows[i]["AuthorId"]).ToString();
                    tmpUser.Popularity =
                        Convert.ToDecimal(_adoHelper.GetCellValue("Author", "Popularity", "Id",
                            userTable.Rows[i]["AuthorId"]));
                }
                users.Add(_dtoMapper.GetUser(tmpUser));
            }
            return users;
        }

        public int Save(User entity)
        {
            if (Exists(entity.Id)) return Update(entity.Id, entity);
            var cmdText = new StringBuilder();
            var type = entity.GetType();
            var props = new List<PropertyInfo>(type.GetProperties());
            if (!(entity is Admin) && !(entity is Author))
            {
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[User] (Id,FirstName,LastName,Age) VALUES({0},'{1}','{2}',{3})",
                    entity.Id, entity.FirstName, entity.LastName, entity.Age);
                return (int)_adoHelper.CrudOperation(cmdText.ToString(), "User");
            }
            if (entity is Admin)
            {
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendLine("DECLARE @Privilegies uniqueidentifier;");
                cmdText.AppendLine("SET @Privilegies=NEWID();");
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[User] (Id,FirstName,LastName,Age,PrivilegiesId) VALUES ({0},'{1}','{2}',{3},@Privilegies); ",
                    entity.Id, entity.FirstName, entity.LastName, entity.Age);
                var privilegyList = props[0].GetValue(entity, null) as List<string>;
                cmdText.AppendFormat("INSERT INTO Privilegies (Id,List) VALUES (@Privilegies, '{0}'); ",
                    GetPrivilegiesString(privilegyList));
                cmdText.AppendLine("COMMIT");
                return (int)_adoHelper.CrudOperation(cmdText.ToString(), "User");
            }
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendLine("DECLARE @AuthorID uniqueidentifier");
            cmdText.AppendLine("SET @AuthorID=NEWID()");
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[User](Id,FirstName,LastName,Age,AuthorId) VALUES ({0},'{1}','{2}',{3},@AuthorID) ",
                entity.Id, entity.FirstName, entity.LastName, entity.Age);
            var nickName = props[0].GetValue(entity, null).ToString();
            var popularity = Convert.ToDecimal(props[1].GetValue(entity, null));
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[Author](Id,NickName,Popularity) VALUES (@AuthorID,'{0}','{1}') ", nickName,
                popularity);
            cmdText.AppendLine("COMMIT");
            return (int)_adoHelper.CrudOperation(cmdText.ToString(), "User");
        }

        public void Delete(User user)
        {
            var cmdText = new StringBuilder();
            if (!(user is Admin) && !(user is Author))
            {
                cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id='{0}'", user.Id);
            }
            else
            {
                if (user is Admin)
                {
                    var privilegyId = (Guid)_adoHelper.GetCellValue("User", "PrivilegiesId", user.Id);
                    cmdText.AppendLine("BEGIN TRANSACTION");
                    cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id={0}) ", user.Id);
                    cmdText.AppendFormat("DELETE FROM [dbo].[Privilegies] WHERE Id={0}", privilegyId);
                    cmdText.AppendLine("COMMIT");
                }
                else
                {
                    var authorId = (Guid)_adoHelper.GetCellValue("User", "AuthorId", user.Id);
                    cmdText.AppendLine("BEGIN TRANSACTION");
                    cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id={0}) ", user.Id);
                    cmdText.AppendFormat("DELETE FROM [dbo].[Author] WHERE Id={0}", authorId);
                    cmdText.AppendLine("COMMIT");
                }
            }
            _adoHelper.CrudOperation(cmdText.ToString(), "User");
        }

        public int Update(int oldUserId, User newUser)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newUser.FirstName, newUser.LastName, newUser.Age, oldUserId);
            if (!(newUser is Admin) && !(newUser is Author))
            {
                return Convert.ToInt32(_adoHelper.CrudOperation(commandText, "User"));
            }
            var type = newUser.GetType();
            var properties = new List<PropertyInfo>(type.GetProperties());
            if (newUser is Admin)
            {
                var privilegiesId = (Guid)_adoHelper.GetCellValue("User", "PrivilegiesId", oldUserId);
                var cmd = string.Format("UPDATE [dbo].[Privilegies] SET List='{0}' WHERE Id='{1}' ", GetPrivilegiesString(properties[0].GetValue(newUser, null) as List<string>), privilegiesId);
                _adoHelper.CrudOperation(cmd, "Privilegies");
                return Convert.ToInt32(_adoHelper.CrudOperation(commandText, "User"));
            }
            var authorId = _adoHelper.GetCellValue("User", "AuthorId", oldUserId);
            var authorCmd = string.Format("UPDATE [dbo].[Author] SET NickName='{0}',Popularity='{1}' WHERE Id='{2}'; ", properties[0].GetValue(newUser, null), properties[1].GetValue(newUser, null), authorId);
            _adoHelper.CrudOperation(authorCmd, "Author");
            return Convert.ToInt32(_adoHelper.CrudOperation(commandText, "User"));
        }

        public User GetById(int? id)
        {
            var userData = _adoHelper.GetData("User", (int)id).Rows[0];

            var dtoUser = new Dto.DtoEntities.User
            {
                Id = (int)userData["Id"],
                FirstName = userData["FirstName"].ToString(),
                LastName = userData["LastName"].ToString(),
                Privilegies = _adoHelper.GetCellValue("Privilegies", "List", "Id", userData["PrivilegiesId"]).ToString(),
                NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", userData["AuthorId"]).ToString(),
                Popularity =
                    Convert.ToInt32(_adoHelper.GetCellValue("Author", "Popularity", "Id", userData["AuthorId"]))
            };

            if ((Guid)userData["PrivilegiesId"] == Guid.Empty && (Guid)userData["AuthorId"] == Guid.Empty)
                return _dtoMapper.GetUser(dtoUser);
            if ((Guid)userData["AuthorId"] != Guid.Empty)
            {
                return _dtoMapper.GetAuthor(dtoUser);
            }
            return _dtoMapper.GetAdmin(dtoUser);
        }

        public User GetRandom()
        {
            var random = new Random();
            var table = _adoHelper.GetData("User");
            var userTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var tmpUser = new Dto.DtoEntities.User
            {
                Id = (int)userTable["Id"],
                FirstName = userTable["FirstName"].ToString(),
                LastName = userTable["LastName"].ToString(),
                Age = (int)userTable["Age"]
            };
            tmpUser.Privilegies = _adoHelper.GetCellValue("Privilegies", "List", "Id", userTable["PrivilegiesId"]).ToString();
            tmpUser.NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", userTable["AuthorId"]).ToString();
            tmpUser.Popularity =
                Convert.ToDecimal(_adoHelper.GetCellValue("Author", "Popularity", "Id", userTable["AuthorId"]));
            return _dtoMapper.GetUser(tmpUser);
        }

        private string GetPrivilegiesString(List<string> privilegies)
        {
            var result = privilegies[0];
            for (int i = 1; i < privilegies.Count; i++)
            {
                result = string.Format("{0}, {1}", result, privilegies[i]);
            }
            return result;
        }

        private bool Exists(int id)
        {
            var user = _adoHelper.GetCellValue("User", "Id", id);
            return user != null;
        }
    }

    class DbArticleRepository : IRepository<Article>
    {
        private readonly AdoHelper _adoHelper;

        private readonly DtoMapper _dtoMapper;

        public DbArticleRepository()
        {
            _adoHelper = new AdoHelper();
            _dtoMapper = new DtoMapper();
        }

        public bool Initialized
        {
            get
            {
                if (_adoHelper.GetData("Article").Rows.Count != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public List<Article> Get()
        {
            var articles = new List<Article>();
            var articlesTable = _adoHelper.GetData("Article");
            for (int i = 0; i < articlesTable.Rows.Count; i++)
            {
                var dtoArticle = new Dto.DtoEntities.Article
                {
                    Id = (int)articlesTable.Rows[i]["Id"],
                    Title = articlesTable.Rows[i]["Title"].ToString(),
                    Content = articlesTable.Rows[i]["Content"].ToString(),
                    AuthorId = Convert.ToInt32(_adoHelper.GetCellValue("User", "Id", "AuthorId", articlesTable.Rows[i]["AuthorId"])),
                    Ratings = new List<object>()
                };
                var rating = _adoHelper.GetData("Rating").Rows;
                for (int j = 0; j < rating.Count; j++)
                {
                    if ((int)rating[j]["ArticleId"] == (int)articlesTable.Rows[i]["Id"])
                    {
                        dtoArticle.Ratings.Add((int)rating[j]["Value"]);
                    }
                }
                var textRating = _adoHelper.GetData("TextRating").Rows;
                for (var j = 0; j < textRating.Count; j++)
                {
                    if ((int)textRating[j]["ArticleId"] == (int)articlesTable.Rows[i]["Id"])
                    {
                        dtoArticle.Ratings.Add(textRating[i]["Value"]);
                    }
                }
                var currentArticle = _dtoMapper.GetArticle(dtoArticle);
                currentArticle.Author = new Author()
                {
                    Id = dtoArticle.AuthorId,
                    FirstName = _adoHelper.GetCellValue("User", "FirstName", dtoArticle.AuthorId).ToString(),
                    LastName = _adoHelper.GetCellValue("User", "LastName", dtoArticle.AuthorId).ToString(),
                    Age = Convert.ToInt32(_adoHelper.GetCellValue("User", "Age", dtoArticle.AuthorId)),
                    NickName = _adoHelper.GetCellValue("Author", "NickName", "Id", _adoHelper.GetCellValue("User", "AuthorId", dtoArticle.AuthorId)).ToString(),
                    Popularity = Convert.ToDecimal(_adoHelper.GetCellValue("Author", "Popularity", "Id", _adoHelper.GetCellValue("User", "AuthorId", dtoArticle.AuthorId)))
                };

                articles.Add(currentArticle);
            }
            return articles;
        }

        public int Save(Article entity)
        {
            if (Exists(entity.Id)) return Update(entity.Id, entity);
            var cmdText = new StringBuilder();
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendLine("DECLARE @authorId uniqueidentifier");
            cmdText.AppendFormat("SET @authorId='{0}' ", _adoHelper.GetCellValue("User", "AuthorId", entity.Author.Id));
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[Article](Id,Title,Content,AuthorId) VALUES({0},'{1}','{2}','{3}') ",
                entity.Id, entity.Title, entity.Content, _adoHelper.GetCellValue("User", "AuthorId", entity.Author.Id));
            cmdText.AppendLine("COMMIT");
            return (int)_adoHelper.CrudOperation(cmdText.ToString(), "Article");
        }

        public void Delete(Article article)
        {
            var cmdText = string.Format("DELETE FROM [dbo].[Article] WHERE Id={0}", article.Id);
            _adoHelper.CrudOperation(cmdText, "Article");
        }

        public int Update(int oldArticleId, Article newArticle)
        {
            var commandText = string.Format("UPDATE [dbo].[Article] SET Title='{0}',Content='{1}' WHERE Id={2}", newArticle.Title, newArticle.Content, oldArticleId);
            return Convert.ToInt32(_adoHelper.CrudOperation(commandText, "Article"));
        }

        public Article GetById(int? id)
        {
            var articleTable = _adoHelper.GetData("Article", id).Rows[0];
            var dtoArticle = new Dto.DtoEntities.Article
            {
                Id = Convert.ToInt32(articleTable["Id"]),
                Title = articleTable["Title"].ToString(),
                Content = articleTable["Content"].ToString(),
                AuthorId = Convert.ToInt32(_adoHelper.GetCellValue("User", "Id", "AuthorId", articleTable["AuthorId"])),
                Ratings = new List<object>()
            };
            var rating = _adoHelper.GetData("Rating").Rows;
            for (var i = 0; i < rating.Count; i++)
            {
                if (rating[i]["ArticleId"] == articleTable["Id"])
                {
                    dtoArticle.Ratings.Add((int)rating[i]["Value"]);
                }
            }
            var textRating = _adoHelper.GetData("TextRating").Rows;
            for (var i = 0; i < textRating.Count; i++)
            {
                if (textRating[i]["ArticleId"] == articleTable["Id"])
                {
                    dtoArticle.Ratings.Add(textRating[i]["Value"]);
                }
            }
            var article = _dtoMapper.GetArticle(dtoArticle);
            return article;
        }

        public Article GetRandom()
        {
            var random = new Random();
            var table = _adoHelper.GetData("Article");
            var articleTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var article = new Dto.DtoEntities.Article
            {
                Id = (int)articleTable["Id"],
                Title = articleTable["Title"].ToString(),
                Content = articleTable["Content"].ToString(),
                AuthorId = Convert.ToInt32(_adoHelper.GetCellValue("User", "Id", "AuthorId", articleTable["AuthorId"])),
                Ratings = new List<object>()
            };
            var rating = _adoHelper.GetData("Rating").Rows;
            for (var i = 0; i < rating.Count; i++)
            {
                if (rating[i]["ArticleId"] == articleTable["Id"])
                {
                    article.Ratings.Add((int)rating[i]["Value"]);
                }
            }
            var textRating = _adoHelper.GetData("TextRating").Rows;
            for (var i = 0; i < textRating.Count; i++)
            {
                if (textRating[i]["ArticleId"] == articleTable["Id"])
                {
                    article.Ratings.Add(textRating[i]["Value"]);
                }
            }
            return _dtoMapper.GetArticle(article);
        }

        private bool Exists(int id)
        {
            var article = _adoHelper.GetCellValue("Article", "Id", id);
            return article != null;
        }
    }

    class DbCommentRepository : IRepository<BaseComment>
    {
        private readonly AdoHelper _adoHelper;

        private readonly DtoMapper _dtoMapper;

        public DbCommentRepository()
        {
            _adoHelper = new AdoHelper();
            _dtoMapper = new DtoMapper();
        }

        public bool Initialized
        {
            get
            {
                if (_adoHelper.GetData("Comments").Rows.Count != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public int Update(int oldComment, BaseComment newComment)
        {
            if (!(newComment is Review))
            {
                return
                    Convert.ToInt32(_adoHelper.CrudOperation(
                        string.Format("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1}",
                            newComment.Content, oldComment), "Comments"));
            }
            var type = newComment.GetType();
            var properties = new List<PropertyInfo>(type.GetProperties());
            var cmd = new StringBuilder();
            cmd.AppendLine("BEGIN TRANSACTION");
            cmd.AppendFormat("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1} ",
                newComment.Content, oldComment);
            if (newComment is ReviewText)
            {
                cmd.AppendFormat("UPDATE [dbo].[TextRating] SET Value={0} WHERE CommentId={1} ", properties[0].GetValue(newComment, null), newComment.Id);
                cmd.AppendLine("COMMIT");
                return Convert.ToInt32(_adoHelper.CrudOperation(cmd.ToString(), "Comment"));
            }
            cmd.AppendFormat("UPDATE [dbo].[Rating] SET Value={0} WHERE CommentId={1} ", (properties[0].GetValue(newComment, null) as Rating).Value, newComment.Id);
            cmd.AppendLine("COMMIT");
            return Convert.ToInt32(_adoHelper.CrudOperation(cmd.ToString(), "Comment"));
        }

        public int Save(BaseComment comment)
        {
            if (Exists(comment.Id)) return Update(comment.Id, comment);
            var cmdText = new StringBuilder();
            var props = new List<PropertyInfo>(comment.GetType().GetProperties());
            if (!(comment is Review))
            {
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](Id, UserId,ArticleId,Content) VALUES({0},'{1}','{2}','{3}') ",
                    comment.Id, comment.User.Id, comment.Article.Id, comment.Content);
                return (int)_adoHelper.CrudOperation(cmdText.ToString(), "Comments");
            }
            var ratingValue = (props[0].GetValue(comment, null) as Rating).Value;
            if (comment is ReviewText)
            {
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](Id,UserId,ArticleId,Content) VALUES({0},'{1}','{2}','{3}') ",
                    comment.Id, comment.User.Id, comment.Article.Id, comment.Content);
                cmdText.AppendLine("COMMIT");
                var reviewTextId = _adoHelper.CrudOperation(cmdText.ToString(), "Comments");
                var ratingCmd = string.Format("INSERT INTO [dbo].[TextRating](Id,ArticleId,Value,CommentId) VALUES('{0}','{1}',{2},{3}) ", comment.Id, comment.Article.Id, ratingValue, reviewTextId);
                _adoHelper.CrudOperation(ratingCmd, "Rating");
                return (int)reviewTextId;
            }
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[Comments](Id,UserId,ArticleId,Content) VALUES({0},{1},{2},'{3}') ",
                comment.Id, comment.User.Id, comment.Article.Id, comment.Content);
            cmdText.AppendLine("COMMIT");
            var commentId = _adoHelper.CrudOperation(cmdText.ToString(), "Comments");
            var ratingCommand = string.Format("INSERT INTO [dbo].[Rating](Id,ArticleId,Value,CommentId) VALUES({0},{1},{2},{3}) ", comment.User.Id, comment.Article.Id, ratingValue, commentId);
            _adoHelper.CrudOperation(ratingCommand, "Rating");
            return (int)commentId;
        }

        public BaseComment GetRandom()
        {
            var random = new Random();
            var table = _adoHelper.GetData("Comments");
            var commentTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var ratingInt = _adoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            var ratingText = _adoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            var rating = ratingInt ?? ratingText;
            var dtoComment = new Dto.DtoEntities.Comment()
            {
                Id = (int)commentTable["Id"],
                Content = commentTable["Content"].ToString(),
                ArticleId = (int)commentTable["ArticleId"],
                UserId = (int)commentTable["UserId"],
                Rating = rating
            };
            //var comment = new ObjectRepository.Entities.Comment
            //{
            //    Id = (int)commentTable["Id"],
            //    Content = commentTable["Content"].ToString(),
            //    ArticleId = (int)commentTable["ArticleId"],
            //    UserId = (int)commentTable["UserId"]
            //};
            //var rating = _adoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            //var textRating = _adoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            //if (rating != null)
            //{
            //    comment.Rating = (int)rating;
            //}
            //comment.Rating = (int)textRating;            
            return _dtoMapper.GetComment(dtoComment);
        }

        public BaseComment GetById(int? id)
        {
            var commentData = _adoHelper.GetData("Comments", (int)id);
            var commentTable = commentData.Rows[(int)id];
            var comment = new Dto.DtoEntities.Comment
            {
                Id = (int)commentTable["Id"],
                Content = commentTable["Content"].ToString(),
                ArticleId = (int)commentTable["ArticleId"],
                UserId = (int)commentTable["UserId"],
            };
            var rating = _adoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            var textRating = _adoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            if (((int)rating != 0) && (textRating != DBNull.Value)) return _dtoMapper.GetComment(comment);
            if (((int)rating == 0))
            {
                comment.Rating = rating;
            }
            comment.Rating = textRating;
            return _dtoMapper.GetComment(comment);
        }

        public List<BaseComment> Get()
        {
            var comments = new List<BaseComment>();
            var commentsTable = _adoHelper.GetData("Comments");
            for (var i = 0; i < commentsTable.Rows.Count; i++)
            {
                var dtoComment = new Dto.DtoEntities.Comment
                {
                    Id = (int)commentsTable.Rows[i]["Id"],
                    Content = commentsTable.Rows[i]["Content"].ToString(),
                    ArticleId = (int)commentsTable.Rows[i]["ArticleId"],
                    UserId = (int)commentsTable.Rows[i]["UserId"]
                };
                var rating = _adoHelper.GetCellValue("Rating", "Value", "CommentId", commentsTable.Rows[i]["Id"]);
                var textRating = _adoHelper.GetCellValue("TextRating", "Value", "CommentId", commentsTable.Rows[i]["Id"]);
                if (rating != null || textRating != DBNull.Value)
                {
                    dtoComment.Rating = rating ?? textRating;
                }
                comments.Add(_dtoMapper.GetComment(dtoComment));
            }
            return comments;
        }

        public void Delete(BaseComment comment)
        {
            var cmdText = string.Format("DELETE FROM [dbo].[Comments] WHERE Id={0}", comment.Id);
            _adoHelper.CrudOperation(cmdText, "Comments");
        }

        private bool Exists(int id)
        {
            var comment = _adoHelper.GetCellValue("Comments", "Id", id);
            return comment != null;
        }
    }
}
