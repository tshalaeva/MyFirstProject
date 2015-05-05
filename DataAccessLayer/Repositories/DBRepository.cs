using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;
using StructureMap;

namespace DataAccessLayer.Repositories
{
    public class DbUserRepository : IRepository<User>
    {
        protected readonly AdoHelper AdoHelper;

        protected readonly DtoMapper DtoMapper;

        private readonly DbAdminRepository adminRepository;

        private readonly DbAuthorRepository authorRepository;

        public DbUserRepository()
        {

        }

        [DefaultConstructor]
        public DbUserRepository(DbAdminRepository adminRepo, DbAuthorRepository authorRepo)
        {
            AdoHelper = new AdoHelper();
            DtoMapper = new DtoMapper();
            adminRepository = adminRepo;
            authorRepository = authorRepo;
        }

        public bool Initialized
        {
            get
            {
                var table = AdoHelper.GetData("User").Rows.Count;
                return table != 0;
            }
        }

        public List<User> Get()
        {
            var userTable = AdoHelper.GetData("User");
            var users = new List<User>();
            for (var i = 0; i < userTable.Rows.Count; i++)
            {
                var tmpUser = new DtoUser
                {
                    Id = (int)userTable.Rows[i]["Id"],
                    FirstName = userTable.Rows[i]["FirstName"].ToString(),
                    LastName = userTable.Rows[i]["LastName"].ToString(),
                    Age = (int)userTable.Rows[i]["Age"]
                };
                if (!userTable.Rows[i]["PrivilegiesId"].Equals(DBNull.Value))
                {
                    tmpUser.Privilegies = AdoHelper.GetCellValue("Privilegies", "List", "Id", userTable.Rows[i]["PrivilegiesId"]).ToString();
                }
                if (!userTable.Rows[i]["AuthorId"].Equals(DBNull.Value))
                {
                    tmpUser.NickName = AdoHelper.GetCellValue("Author", "NickName", "Id", userTable.Rows[i]["AuthorId"]).ToString();
                    tmpUser.Popularity =
                        Convert.ToDecimal(AdoHelper.GetCellValue("Author", "Popularity", "Id",
                            userTable.Rows[i]["AuthorId"]));
                }
                users.Add(DtoMapper.GetUser(tmpUser));
            }
            return users;
        }

        public int Save(User entity)
        {
            if (Exists(entity.Id)) return Update(entity.Id, entity);
            var cmdText = new StringBuilder();
            if (!(entity is Admin) && !(entity is Author))
            {
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[User] (FirstName,LastName,Age) OUTPUT Inserted.Id VALUES('{0}','{1}',{2})",
                    entity.FirstName, entity.LastName, entity.Age);
                return (int)AdoHelper.CrudOperation(cmdText.ToString(), "User");
            }
            if (entity is Admin)
            {
                return adminRepository.SaveAdmin((Admin)entity);
            }
            return authorRepository.SaveAuthor((Author)entity);
        }

        public void Delete(int userId)
        {
            var data = Get();
            var cmdText = new StringBuilder();
            if (!(data[userId] is Admin) && !(data[userId] is Author))
            {
                cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id='{0}'", userId);

                AdoHelper.CrudOperation(cmdText.ToString(), "User");
            }
            else
            {
                if (data[userId] is Admin)
                {
                    adminRepository.DeleteAdmin(userId);
                }
                else
                {
                    authorRepository.DeleteAuthor(userId);
                }
            }
        }

        public int Update(int oldUserId, User newUser)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newUser.FirstName, newUser.LastName, newUser.Age, oldUserId);
            if (!(newUser is Admin) && !(newUser is Author))
            {
                return Convert.ToInt32(AdoHelper.CrudOperation(commandText, "User"));
            }
            if (newUser is Admin)
            {
                return adminRepository.UpdateAdmin(oldUserId, (Admin)newUser);
            }
            return authorRepository.UpdateAuthor(oldUserId, (Author)newUser);
        }

        public User GetById(int? id)
        {
            var userData = AdoHelper.GetData("User", (int)id).Rows[0];

            var dtoUser = new DtoUser
            {
                Id = (int)userData["Id"],
                FirstName = userData["FirstName"].ToString(),
                LastName = userData["LastName"].ToString(),
                Privilegies = AdoHelper.GetCellValue("Privilegies", "List", "Id", userData["PrivilegiesId"]).ToString(),
                NickName = AdoHelper.GetCellValue("Author", "NickName", "Id", userData["AuthorId"]).ToString(),
                Popularity =
                    Convert.ToInt32(AdoHelper.GetCellValue("Author", "Popularity", "Id", userData["AuthorId"]))
            };

            if ((Guid)userData["PrivilegiesId"] == Guid.Empty && (Guid)userData["AuthorId"] == Guid.Empty)
                return DtoMapper.GetUser(dtoUser);
            if ((Guid)userData["AuthorId"] != Guid.Empty)
            {
                return DtoMapper.GetAuthor(dtoUser);
            }
            return DtoMapper.GetAdmin(dtoUser);
        }

        public User GetRandom()
        {
            var random = new Random();
            var table = AdoHelper.GetData("User");
            var userTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var tmpUser = new DtoUser
            {
                Id = (int)userTable["Id"],
                FirstName = userTable["FirstName"].ToString(),
                LastName = userTable["LastName"].ToString(),
                Age = (int)userTable["Age"]
            };
            tmpUser.Privilegies = AdoHelper.GetCellValue("Privilegies", "List", "Id", userTable["PrivilegiesId"]).ToString();
            tmpUser.NickName = AdoHelper.GetCellValue("Author", "NickName", "Id", userTable["AuthorId"]).ToString();
            tmpUser.Popularity =
                Convert.ToDecimal(AdoHelper.GetCellValue("Author", "Popularity", "Id", userTable["AuthorId"]));
            return DtoMapper.GetUser(tmpUser);
        }

        protected string GetPrivilegiesString(List<string> privilegies)
        {
            var result = privilegies[0];
            for (int i = 1; i < privilegies.Count; i++)
            {
                result = string.Format("{0}, {1}", result, privilegies[i]);
            }
            return result;
        }

        protected bool Exists(int id)
        {
            var user = AdoHelper.GetCellValue("User", "Id", id);
            return user != null;
        }
    }

    public class DbAdminRepository : DbUserRepository
    {
        private readonly AdoHelper adminAdoHelper = new AdoHelper();

        public List<Admin> GetAdmins()
        {
            var adminTable = AdoHelper.GetData("Privilegies");
            var admins = new List<Admin>();
            for (var i = 0; i < adminTable.Rows.Count; i++)
            {
                var tmpUser = new DtoUser
                {
                    Id = (int)AdoHelper.GetCellValue("User", "Id", "PrivilegiesId", adminTable.Rows[i]["Id"]),
                    FirstName = AdoHelper.GetCellValue("User", "FirstName", "PrivilegiesId", adminTable.Rows[i]["Id"]).ToString(),
                    LastName = AdoHelper.GetCellValue("User", "LastName", "PrivilegiesId", adminTable.Rows[i]["Id"]).ToString(),
                    Age = (int)AdoHelper.GetCellValue("User", "Age", "PrivilegiesId", adminTable.Rows[i]["Id"]),
                    Privilegies = adminTable.Rows[i]["List"].ToString()
                };
                admins.Add(DtoMapper.GetAdmin(tmpUser));
            }
            return admins;
        }

        public int SaveAdmin(Admin entity)
        {
            var cmdText = new StringBuilder();
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendLine("DECLARE @Privilegies uniqueidentifier;");
            cmdText.AppendLine("SET @Privilegies=NEWID();");
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[User] (FirstName,LastName,Age,PrivilegiesId) OUTPUT Inserted.Id VALUES ('{0}','{1}',{2},@Privilegies); ",
                entity.FirstName, entity.LastName, entity.Age);
            var privilegyList = entity.Privilegies;
            cmdText.AppendFormat("INSERT INTO Privilegies (Id,List) VALUES (@Privilegies, '{0}'); ",
                GetPrivilegiesString(privilegyList));
            cmdText.AppendLine("COMMIT");
            return (int)adminAdoHelper.CrudOperation(cmdText.ToString(), "User");
        }

        public void DeleteAdmin(int adminId)
        {
            var cmdText = new StringBuilder();
            var privilegyId = (Guid)AdoHelper.GetCellValue("User", "PrivilegiesId", adminId);
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id={0}) ", adminId);
            cmdText.AppendFormat("DELETE FROM [dbo].[Privilegies] WHERE Id={0}", privilegyId);
            cmdText.AppendLine("COMMIT");
            adminAdoHelper.CrudOperation(cmdText.ToString(), "User");
        }

        public int UpdateAdmin(int oldAdminId, Admin newAdmin)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newAdmin.FirstName, newAdmin.LastName, newAdmin.Age, oldAdminId);
            var privilegiesId = (Guid)adminAdoHelper.GetCellValue("User", "PrivilegiesId", oldAdminId);
            var cmd = string.Format("UPDATE [dbo].[Privilegies] SET List='{0}' WHERE Id='{1}' ", GetPrivilegiesString(newAdmin.Privilegies), privilegiesId);
            adminAdoHelper.CrudOperation(cmd, "Privilegies");
            return Convert.ToInt32(adminAdoHelper.CrudOperation(commandText, "User"));
        }
    }

    public class DbAuthorRepository : DbUserRepository
    {
            private AdoHelper authorAdoHelper = new AdoHelper();

        public List<Author> GetAuthors()
        {
            var authorTable = authorAdoHelper.GetData("Author");
            var authors = new List<Author>();
            for (var i = 0; i < authorTable.Rows.Count; i++)
            {
                var tmpUser = new DtoUser
                {
                    Id = (int)authorAdoHelper.GetCellValue("User", "Id", "AuthorId", authorTable.Rows[i]["Id"]),
                    FirstName = authorAdoHelper.GetCellValue("User", "FirstName", "AuthorId", authorTable.Rows[i]["Id"]).ToString(),
                    LastName = authorAdoHelper.GetCellValue("User", "LastName", "AuthorId", authorTable.Rows[i]["Id"]).ToString(),
                    Age = (int)authorAdoHelper.GetCellValue("User", "Age", "AuthorId", authorTable.Rows[i]["Id"]),
                    NickName = authorTable.Rows[i]["NickName"].ToString(),
                    Popularity = Convert.ToDecimal(authorTable.Rows[i]["Popularity"])
                };
                authors.Add(DtoMapper.GetAuthor(tmpUser));
            }
            return authors;
        }

        public int SaveAuthor(Author entity)
        {
            var cmdText = new StringBuilder();
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendLine("DECLARE @AuthorID uniqueidentifier");
            cmdText.AppendLine("SET @AuthorID=NEWID()");
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[User](FirstName,LastName,Age,AuthorId) OUTPUT Inserted.Id VALUES ('{0}','{1}',{2},@AuthorID) ",
                entity.FirstName, entity.LastName, entity.Age);
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[Author](Id,NickName,Popularity) VALUES (@AuthorID,'{0}','{1}') ", entity.NickName,
                entity.Popularity);
            cmdText.AppendLine("COMMIT");
            return (int)authorAdoHelper.CrudOperation(cmdText.ToString(), "User");
        }

        public void DeleteAuthor(int authorId)
        {
            var cmdText = new StringBuilder();
            var authorGuid = (Guid)authorAdoHelper.GetCellValue("User", "AuthorId", authorId);
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id={0}) ", authorId);
            cmdText.AppendFormat("DELETE FROM [dbo].[Author] WHERE Id={0}", authorGuid);
            cmdText.AppendLine("COMMIT");
            authorAdoHelper.CrudOperation(cmdText.ToString(), "User");
        }

        public int UpdateAuthor(int oldAuthorId, Author newAuthor)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newAuthor.FirstName, newAuthor.LastName, newAuthor.Age, oldAuthorId);
            var authorId = authorAdoHelper.GetCellValue("User", "AuthorId", oldAuthorId);
            var authorCmd = string.Format("UPDATE [dbo].[Author] SET NickName='{0}',Popularity='{1}' WHERE Id='{2}'; ", newAuthor.NickName, newAuthor.Popularity, authorId);
            authorAdoHelper.CrudOperation(authorCmd, "Author");
            return Convert.ToInt32(authorAdoHelper.CrudOperation(commandText, "User"));
        }
    }

    public class DbArticleRepository : IRepository<Article>
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
                var dtoArticle = new DtoArticle
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
                        dtoArticle.Ratings.Add(textRating[j]["Value"]);
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
                "INSERT INTO [dbo].[Article](Title,Content,AuthorId) OUTPUT Inserted.Id VALUES('{0}','{1}','{2}') ",
                entity.Title, entity.Content, _adoHelper.GetCellValue("User", "AuthorId", entity.Author.Id));
            cmdText.AppendLine("COMMIT");
            return (int)_adoHelper.CrudOperation(cmdText.ToString(), "Article");
        }

        public void Delete(int articleId)
        {
            var cmdText = string.Format("DELETE FROM [dbo].[Article] WHERE Id={0}", articleId);
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
            var dtoArticle = new DtoArticle
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
            var article = new DtoArticle
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

    public class DbCommentRepository : IRepository<BaseComment>
    {
        protected readonly AdoHelper AdoHelper;

        public readonly DtoMapper DtoMapper;

        private readonly DbReviewRepository m_reviewRepository;

        private DbReviewTextRepository m_reviewTextRepository;

        public DbCommentRepository()
        {
        }

        [DefaultConstructor]
        public DbCommentRepository(DbReviewRepository reviewRepository, DbReviewTextRepository reviewTextRepository)
        {
            AdoHelper = new AdoHelper();
            DtoMapper = new DtoMapper();
            m_reviewRepository = reviewRepository;
            m_reviewTextRepository = reviewTextRepository;
        }

        public bool Initialized
        {
            get
            {
                if (AdoHelper.GetData("Comments").Rows.Count != 0)
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
                    Convert.ToInt32(AdoHelper.CrudOperation(
                        string.Format("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1}",
                            newComment.Content, oldComment), "Comments"));
            }
            if (newComment is ReviewText)
            {
                return m_reviewTextRepository.UpdateReviewText(oldComment, (ReviewText)newComment);
            }
            return m_reviewRepository.UpdateReview(oldComment, (Review)newComment);
        }

        public int Save(BaseComment comment)
        {
            if (Exists(comment.Id)) return Update(comment.Id, comment);
            var cmdText = new StringBuilder();
            if (!(comment is Review))
            {
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) OUTPUT Inserted.Id VALUES('{0}','{1}','{2}') ",
                    comment.User.Id, comment.Article.Id, comment.Content);
                return (int)AdoHelper.CrudOperation(cmdText.ToString(), "Comments");
            }
            if (comment is ReviewText)
            {
                return m_reviewTextRepository.SaveReviewText((ReviewText)comment);
            }
            return m_reviewRepository.SaveReview((Review)comment);
        }

        public BaseComment GetRandom()
        {
            var random = new Random();
            var table = AdoHelper.GetData("Comments");
            var commentTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var ratingInt = AdoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            var ratingText = AdoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            var rating = ratingInt ?? ratingText;
            var dtoComment = new DtoComment
            {
                Id = (int)commentTable["Id"],
                Content = commentTable["Content"].ToString(),
                ArticleId = (int)commentTable["ArticleId"],
                UserId = (int)commentTable["UserId"],
                Rating = rating
            };
            return DtoMapper.GetComment(dtoComment);
        }

        public BaseComment GetById(int? id)
        {
            var commentData = AdoHelper.GetData("Comments", (int)id);
            var commentTable = commentData.Rows[(int)id];
            var comment = new DtoComment
            {
                Id = (int)commentTable["Id"],
                Content = commentTable["Content"].ToString(),
                ArticleId = (int)commentTable["ArticleId"],
                UserId = (int)commentTable["UserId"],
            };
            var rating = AdoHelper.GetCellValue("Rating", "Value", "CommentId", commentTable["Id"]);
            var textRating = AdoHelper.GetCellValue("TextRating", "Value", "CommentId", commentTable["Id"]);
            if (((int)rating != 0) && (textRating != DBNull.Value)) return DtoMapper.GetComment(comment);
            if (((int)rating == 0))
            {
                comment.Rating = rating;
            }
            comment.Rating = textRating;
            return DtoMapper.GetComment(comment);
        }

        public List<BaseComment> Get()
        {
            var comments = new List<BaseComment>();
            var commentsTable = AdoHelper.GetData("Comments");
            for (var i = 0; i < commentsTable.Rows.Count; i++)
            {
                var dtoComment = new DtoComment
                {
                    Id = (int)commentsTable.Rows[i]["Id"],
                    Content = commentsTable.Rows[i]["Content"].ToString(),
                    ArticleId = (int)commentsTable.Rows[i]["ArticleId"],
                    UserId = (int)commentsTable.Rows[i]["UserId"]
                };
                var rating = AdoHelper.GetCellValue("Rating", "Value", "CommentId", commentsTable.Rows[i]["Id"]);
                var textRating = AdoHelper.GetCellValue("TextRating", "Value", "CommentId", commentsTable.Rows[i]["Id"]);
                if (rating != null || textRating != DBNull.Value)
                {
                    dtoComment.Rating = rating ?? textRating;
                }
                comments.Add(DtoMapper.GetComment(dtoComment));
            }
            return comments;
        }

        public void Delete(int commentId)
        {
            var entity = GetById(commentId);
            if(!(entity is Review))
            {
                var cmdText = string.Format("DELETE FROM [dbo].[Comments] WHERE Id={0}", commentId);
                AdoHelper.CrudOperation(cmdText, "Comments");
            }
            else
            {
                if(entity is ReviewText)
                {
                    m_reviewTextRepository.Delete(commentId);
                    return;
                }
                m_reviewRepository.Delete(commentId);
            }
        }

        private bool Exists(int id)
        {
            var comment = AdoHelper.GetCellValue("Comments", "Id", id);
            return comment != null;
        }
    }

    public class DbReviewRepository : DbCommentRepository
    {
        private AdoHelper reviewAdoHelper = new AdoHelper();

        public int SaveReview(Review review)
        {
            var cmdText = new StringBuilder();
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) OUTPUT Inserted.Id VALUES({0},{1},'{2}') ",
                review.User.Id, review.Article.Id, review.Content);
            cmdText.AppendLine("COMMIT");
            var commentId = reviewAdoHelper.CrudOperation(cmdText.ToString(), "Comments");
            var ratingCommand = string.Format("INSERT INTO [dbo].[Rating](ArticleId,Value,CommentId) VALUES({0},{1},{2}) ", review.Article.Id, review.Rating.Value, commentId);
            reviewAdoHelper.CrudOperation(ratingCommand, "Rating");
            return (int)commentId;
        }

        public int UpdateReview(int oldReviewId, Review newReview)
        {
            var cmd = new StringBuilder();
            cmd.AppendLine("BEGIN TRANSACTION");
            cmd.AppendFormat("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1} ",
                newReview.Content, oldReviewId);           
            cmd.AppendFormat("UPDATE [dbo].[Rating] SET Value={0} WHERE CommentId={1} ", newReview.Rating.Value, oldReviewId);
            cmd.AppendLine("COMMIT");
            return Convert.ToInt32(reviewAdoHelper.CrudOperation(cmd.ToString(), "Comment"));
        }

        public void DeleteReview(int id)
        {
            var cmdText = new StringBuilder();
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendFormat("DELETE FROM [dbo].[Comments] WHERE Id={0} ", id);
            cmdText.AppendFormat("DELETE FROM [dbo].[Rating] WHERE CommentId={0} ", id);
            cmdText.AppendLine("COMMIT");
            reviewAdoHelper.CrudOperation(cmdText.ToString(), "Comments");
        }
    }

    public class DbReviewTextRepository : DbCommentRepository
    {
        private AdoHelper reviewTextAdoHelper = new AdoHelper();

        public int SaveReviewText(ReviewText reviewText)
        {
            var cmdText = new StringBuilder();
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendFormat(
                "INSERT INTO [dbo].[Comments](UserId,ArticleId,Content) OUTPUT Inserted.Id VALUES('{0}','{1}','{2}') ",
                reviewText.User.Id, reviewText.Article.Id, reviewText.Content);
            cmdText.AppendLine("COMMIT");
            var reviewTextId = reviewTextAdoHelper.CrudOperation(cmdText.ToString(), "Comments");
            var ratingCmd = string.Format("INSERT INTO [dbo].[TextRating](ArticleId,Value,CommentId) VALUES({0},{1},{2}) ", reviewText.Article.Id, reviewText.Rating.Value, reviewTextId);
            reviewTextAdoHelper.CrudOperation(ratingCmd, "Rating");
            return (int)reviewTextId;
        }

        public int UpdateReviewText(int oldReviewTextId, ReviewText reviewText)
        {
            var cmd = new StringBuilder();
            cmd.AppendLine("BEGIN TRANSACTION");
            cmd.AppendFormat("UPDATE [dbo].[Comments] SET Content='{0}' WHERE Id={1} ",
                reviewText.Content, oldReviewTextId);
            cmd.AppendFormat("UPDATE [dbo].[TextRating] SET Value={0} WHERE CommentId={1} ", reviewText.Rating.Value, oldReviewTextId);
            cmd.AppendLine("COMMIT");
            return Convert.ToInt32(reviewTextAdoHelper.CrudOperation(cmd.ToString(), "Comment"));
        }

        public void DeleteReviewText(int id)
        {
            var cmdText = new StringBuilder();
            cmdText.AppendLine("BEGIN TRANSACTION");
            cmdText.AppendFormat("DELETE FROM [dbo].[Comments] WHERE Id={0} ", id);
            cmdText.AppendFormat("DELETE FROM [dbo].[TextRating] WHERE CommentId={0} ", id);
            cmdText.AppendLine("COMMIT");
            reviewTextAdoHelper.CrudOperation(cmdText.ToString(), "Comments");
        }
    }
}
