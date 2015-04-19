using System.Collections.Generic;
using MyFirstProject.Entities;
using System.Text;
using System.Reflection;
using System;

namespace MyFirstProject.Repository
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
                if (_adoHelper.GetData("User").Rows.Count != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public List<User> Get()
        {
            var userTable = _adoHelper.GetData("User");
            var users = new List<User>();
            for (var i = 0; i < userTable.Rows.Count; i++)
            {
                var tmpUser = new Entities.Dto.User();
                tmpUser.Id = (int)userTable.Rows[i]["Id"];
                tmpUser.FirstName = userTable.Rows[i]["FirstName"].ToString();
                tmpUser.LastName = userTable.Rows[i]["LastName"].ToString();
                tmpUser.Age = (int)userTable.Rows[i]["Age"];
                if (!userTable.Rows[i]["PrivilegiesId"].Equals(DBNull.Value))
                {
                    tmpUser.PrivilegiesId = (Guid)userTable.Rows[i]["PrivilegiesId"];
                }
                if (!userTable.Rows[i]["AuthorId"].Equals(DBNull.Value))
                {
                    tmpUser.AuthorId = (Guid)userTable.Rows[i]["AuthorId"];
                }
                users.Add(_dtoMapper.GetUser(tmpUser));
            }
            return users;
        }

        public int Save(User entity)
        {
            var cmdText = new StringBuilder();
            var type = entity.GetType();
            var props = new List<PropertyInfo>(type.GetProperties());
            if (!(entity is Admin) && !(entity is Author))
            {
                cmdText.AppendFormat(
                "INSERT INTO [dbo].[User](FirstName,LastName,Age) VALUES('{0}','{1}',{2})",
                entity.FirstName, entity.LastName, entity.Age);
                return (int)_adoHelper.CRUDOperation(cmdText.ToString(), "User");
            }
            if (entity is Admin)
            {
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendLine("DECLARE @Privilegies uniqueidentifier");
                cmdText.AppendLine("SET @Privilegies=NEWID()");
                cmdText.AppendFormat(
                "INSERT INTO [dbo].[User](FirstName,LastName,Age,PrivilegiesId) VALUES ('{0}','{1}',{2},@Privilegies) ",
                entity.FirstName, entity.LastName, entity.Age);
                var privilegyList = props[0].GetValue(entity,null) as List<string>;
                cmdText.AppendFormat("INSERT INTO Privilegies(Id,List) VALUES (@Privilegies, '{0}') ", GetPrivilegiesString(privilegyList));
                cmdText.AppendLine("COMMIT");
                return (int)_adoHelper.CRUDOperation(cmdText.ToString(), "User");
            }
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendLine("DECLARE @AuthorID uniqueidentifier");
            cmdText.AppendLine("SET @AuthorID=NEWID()");
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[User](FirstName,LastName,Age,AuthorId) VALUES ('{0}','{1}',{2},@AuthorID); ",
                entity.FirstName, entity.LastName, entity.Age);
            var nickName = props[0].GetValue(entity, null);
            var popularity = props[1].GetValue(entity, null);
            cmdText.AppendFormat("INSERT INTO [dbo].[Author](Id,NickName,Popularity) VALUES (@AuthorID, '{0}', '{1}'); ", nickName, popularity);
            cmdText.AppendLine("SELECT Id FROM [dbo].[User]");
            cmdText.AppendLine("COMMIT");
            return (int)_adoHelper.CRUDOperation(cmdText.ToString(), "User");
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
                    var getIdCommand = string.Format("SELECT PrivilegyId FROM [dbo].[User] WHERE Id={0}", user.Id);
                    var privilegyId = (Guid)_adoHelper.GetCellValue("User", "PrivilegiesId", user.Id);
                    cmdText.AppendLine("BEGIN TRANSACTION");
                    cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id={0}) ", user.Id);
                    cmdText.AppendFormat("DELETE FROM [dbo].[Privilegies] WHERE Id={0}", privilegyId);
                    cmdText.AppendLine("COMMIT");
                }
                else
                {
                    var getIdCommand = string.Format("SELECT AuthorId FROM [dbo].[User] WHERE Id={0}", user.Id);
                    var authorId = (Guid)_adoHelper.GetCellValue("User", "AuthorId", user.Id);
                    cmdText.AppendLine("BEGIN TRANSACTION");
                    cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id={0}) ", user.Id);
                    cmdText.AppendFormat("DELETE FROM [dbo].[Author] WHERE Id={0}", authorId);
                    cmdText.AppendLine("COMMIT");
                }
            }
            _adoHelper.CRUDOperation(cmdText.ToString(), "User");
        }

        public void Update(User oldUser, User newUser)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newUser.FirstName, newUser.LastName, newUser.Age, oldUser.Id);
            _adoHelper.CRUDOperation(commandText, "User");
        }

        public User GetById(int? id)
        {
            var userData = _adoHelper.GetData("User", (int)id).Rows[0];

            var dtoUser = new Entities.Dto.User();
            dtoUser.Id = (int)userData["Id"];
            dtoUser.FirstName = userData["FirstName"].ToString();
            dtoUser.LastName = userData["LastName"].ToString();
            dtoUser.PrivilegiesId = (Guid)userData["PrivilegiesId"];
            dtoUser.AuthorId = (Guid)userData["AuthorId"];

            if (dtoUser.PrivilegiesId != Guid.Empty || dtoUser.AuthorId != Guid.Empty)
            {
                if (dtoUser.AuthorId != Guid.Empty)
                {
                    return _dtoMapper.GetAuthor(dtoUser);
                }
                else
                {
                    return _dtoMapper.GetAdmin(dtoUser);
                }
            }
            return _dtoMapper.GetUser(dtoUser);
        }

        public User GetRandom()
        {
            var random = new Random();
            var table = _adoHelper.GetData("User");
            var userTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var tmpUser = new Entities.Dto.User();
            tmpUser.Id = (int)userTable["Id"];
            tmpUser.FirstName = userTable["FirstName"].ToString();
            tmpUser.LastName = userTable["LastName"].ToString();
            tmpUser.Age = (int)userTable["Age"];
            tmpUser.PrivilegiesId = (Guid)userTable["PrivilegiesId"];
            tmpUser.AuthorId = (Guid)userTable["AuthorId"];
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
                else
                {
                    return false;
                }
            }
        }

        public List<Article> Get()
        {
            var articles = new List<Article>();
            var articlesTable = _adoHelper.GetData("Article");
            for (int i = 0; i < articlesTable.Rows.Count; i++)
            {
                var dtoArticle = new Entities.Dto.Article();
                dtoArticle.Id = (int)articlesTable.Rows[i]["Id"];
                dtoArticle.Title = articlesTable.Rows[i]["Title"].ToString();
                dtoArticle.Content = articlesTable.Rows[i]["Content"].ToString();
                dtoArticle.AuthorId = (Guid)articlesTable.Rows[i]["AuthorId"];
                articles.Add(_dtoMapper.GetArticle(dtoArticle));
            }
                return articles;
        }

        public int Save(Article entity)
        {
            var cmdText = new StringBuilder();
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendLine("DECLARE @authorId uniqueidentifier");
            cmdText.AppendFormat("SET @authorId='{0}' ", _adoHelper.GetCellValue("User", "AuthorId", entity.Author.Id));
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[Article](Title,Content,AuthorId) VALUES('{0}','{1}','{2}') ",
                entity.Title, entity.Content, _adoHelper.GetCellValue("User", "AuthorId", entity.Author.Id));
            cmdText.AppendLine("COMMIT");
            return (int)_adoHelper.CRUDOperation(cmdText.ToString(), "Article");
        }

        public void Delete(Article article)
        {
            var cmdText = string.Format("DELETE FROM [dbo].[Article] WHERE Id={0}", article.Id);
            _adoHelper.CRUDOperation(cmdText, "Article");
        }

        public void Update(Article oldArticle, Article newArticle)
        {
            var commandText = string.Format("UPDATE [dbo].[Article] SET Title='{0}',Content='{1}',Author={2} WHERE Id={3}", newArticle.Title, newArticle.Content, newArticle.Author.Id, oldArticle.Id);
            _adoHelper.CRUDOperation(commandText, "Article");
        }

        public Article GetById(int? id)
        {
            return _dtoMapper.GetArticleById((int)id);
        }

        public Article GetRandom()
        {
            var random = new Random();
            var table = _adoHelper.GetData("Article");
            var articleTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var article = new Entities.Dto.Article();
            article.Id = (int)articleTable["Id"];
            article.Title = articleTable["Title"].ToString();
            article.Content = articleTable["Content"].ToString();
            article.AuthorId = (Guid)articleTable["AuthorId"];
            return _dtoMapper.GetArticle(article);
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

        public void Update(BaseComment oldComment, BaseComment newComment)
        {

        }

        public int Save(BaseComment comment)
        {
            var cmdText = new StringBuilder();
            var props = new List<PropertyInfo>(comment.GetType().GetProperties());
            if (!(comment is Review))
            {
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) VALUES('{0}','{1}','{2}') ",
                    comment.User.Id, comment.Article.Id, comment.Content);
                return (int)_adoHelper.CRUDOperation(cmdText.ToString(), "Comments");
            }
            var ratingValue = (props[0].GetValue(comment, null) as Rating).Value;
            if (comment is ReviewText)
            {
                cmdText.AppendLine("BEGIN TRANSACTION");
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) VALUES('{0}','{1}','{2}') ",
                    comment.User.Id, comment.Article.Id, comment.Content);                
                cmdText.AppendLine("COMMIT");
                var reviewTextId = _adoHelper.CRUDOperation(cmdText.ToString(), "Comments");
                var ratingCmd = string.Format("INSERT INTO [dbo].[TextRating](Id,ArticleId,Value,CommentId) VALUES('{0}','{1}',{2},{3}) ", comment.User.Id, comment.Article.Id, ratingValue, reviewTextId);
                _adoHelper.CRUDOperation(ratingCmd, "Rating");
                return (int)reviewTextId;
            }
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) VALUES({0},{1},'{2}') ",
                comment.User.Id, comment.Article.Id, comment.Content);
            cmdText.AppendLine("COMMIT");
            var commentId = _adoHelper.CRUDOperation(cmdText.ToString(), "Comments");
            var ratingCommand = string.Format("INSERT INTO [dbo].[Rating](Id,ArticleId,Value,CommentId) VALUES({0},{1},{2},{3}) ", comment.User.Id, comment.Article.Id, ratingValue, commentId);
            _adoHelper.CRUDOperation(ratingCommand, "Rating");
            return (int)commentId;
        }

        public BaseComment GetRandom()
        {
            var random = new Random();
            var table = _adoHelper.GetData("Comments");
            var commentTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var comment = new Entities.Dto.Comment
            {
                Id = (int) commentTable["Id"],
                Content = commentTable["Content"].ToString(),
                ArticleId = (int) commentTable["ArticleId"],
                UserId = (int) commentTable["UserId"]
            };
            var rating = _adoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            var textRating = _adoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            if (rating != null)
            {
                comment.Rating = (int) rating;
            }
            comment.Rating = (int) textRating;
            return _dtoMapper.GetComment(comment);
        }

        public BaseComment GetById(int? id)
        {
            var commentData = _adoHelper.GetData("Comments", (int)id);
            var commentTable = commentData.Rows[(int)id];
            var comment = new Entities.Dto.Comment
            {
                Id = (int) commentTable["Id"],
                Content = commentTable["Content"].ToString(),
                ArticleId = (int) commentTable["ArticleId"],
                UserId = (int) commentTable["UserId"],                
            };
            var rating = _adoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            var textRating = _adoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            if (((int) rating != 0) && (textRating != DBNull.Value)) return _dtoMapper.GetComment(comment);
            if (((int) rating == 0))
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
                var dtoComment = new Entities.Dto.Comment
                {
                    Id = (int) commentsTable.Rows[i]["Id"],
                    Content = commentsTable.Rows[i]["Content"].ToString(),
                    ArticleId = (int) commentsTable.Rows[i]["ArticleId"],
                    UserId = (int) commentsTable.Rows[i]["UserId"]
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
            _adoHelper.CRUDOperation(cmdText, "Comments");
        }
    }
}
