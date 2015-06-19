using System;
using System.Collections.Generic;
using System.Data;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;
using StructureMap;

namespace DataAccessLayer.Repositories
{
    public class DbArticleRepository : IRepository<Article>
    {
        private readonly AdoHelper m_adoHelper;

        private readonly DtoMapper m_dtoMapper;

        [DefaultConstructor]
        public DbArticleRepository()
        {
            m_adoHelper = new AdoHelper();
            m_dtoMapper = new DtoMapper();
        }

        public bool Initialized
        {
            get
            {
                return m_adoHelper.GetData("Article").Rows.Count != 0;
            }
        }

        public List<Article> Get(int from, int count)
        {
            var articles = new List<Article>();
            var articlesTable = m_adoHelper.GetData("Article", from, count);
            for (var i = 0; i < articlesTable.Rows.Count; i++)
            {
                var dtoArticle = new DtoArticle
                {
                    Id = (int)articlesTable.Rows[i]["Id"],
                    Title = articlesTable.Rows[i]["Title"].ToString(),
                    Content = articlesTable.Rows[i]["Content"].ToString(),
                    AuthorId = Convert.ToInt32(m_adoHelper.GetCellValue("User", "Id", "AuthorId", articlesTable.Rows[i]["AuthorId"])),
                    Ratings = new List<object>()
                };
                var rating = m_adoHelper.GetData("Rating").Rows;
                for (var j = 0; j < rating.Count; j++)
                {
                    if ((int)rating[j]["ArticleId"] == (int)articlesTable.Rows[i]["Id"])
                    {
                        dtoArticle.Ratings.Add((int)rating[j]["Value"]);
                    }
                }
                var textRating = m_adoHelper.GetData("TextRating").Rows;
                for (var j = 0; j < textRating.Count; j++)
                {
                    if ((int)textRating[j]["ArticleId"] == (int)articlesTable.Rows[i]["Id"])
                    {
                        dtoArticle.Ratings.Add(textRating[j]["Value"]);
                    }
                }
                var currentArticle = m_dtoMapper.GetArticle(dtoArticle);
                currentArticle.Author = new Author
                {
                    Id = dtoArticle.AuthorId,
                    FirstName = m_adoHelper.GetCellValue("User", "FirstName", dtoArticle.AuthorId).ToString(),
                    LastName = m_adoHelper.GetCellValue("User", "LastName", dtoArticle.AuthorId).ToString(),
                    Age = Convert.ToInt32(m_adoHelper.GetCellValue("User", "Age", dtoArticle.AuthorId)),
                    NickName = m_adoHelper.GetCellValue("Author", "NickName", "Id", m_adoHelper.GetCellValue("User", "AuthorId", dtoArticle.AuthorId)).ToString(),
                    Popularity = Convert.ToDecimal(m_adoHelper.GetCellValue("Author", "Popularity", "Id", m_adoHelper.GetCellValue("User", "AuthorId", dtoArticle.AuthorId)))
                };

                articles.Add(currentArticle);
            }
            return articles;
        }

        public List<Article> Get()
        {
            var articles = new List<Article>();
            var articlesTable = m_adoHelper.GetData("Article");
            for (var i = 0; i < articlesTable.Rows.Count; i++)
            {
                var dtoArticle = new DtoArticle
                {
                    Id = (int)articlesTable.Rows[i]["Id"],
                    Title = articlesTable.Rows[i]["Title"].ToString(),
                    Content = articlesTable.Rows[i]["Content"].ToString(),
                    AuthorId = Convert.ToInt32(m_adoHelper.GetCellValue("User", "Id", "AuthorId", articlesTable.Rows[i]["AuthorId"])),
                    Ratings = new List<object>()
                };
                var rating = m_adoHelper.GetData("Rating").Rows;
                for (int j = 0; j < rating.Count; j++)
                {
                    if ((int)rating[j]["ArticleId"] == (int)articlesTable.Rows[i]["Id"])
                    {
                        dtoArticle.Ratings.Add((int)rating[j]["Value"]);
                    }
                }
                var textRating = m_adoHelper.GetData("TextRating").Rows;
                for (var j = 0; j < textRating.Count; j++)
                {
                    if ((int)textRating[j]["ArticleId"] == (int)articlesTable.Rows[i]["Id"])
                    {
                        dtoArticle.Ratings.Add(textRating[j]["Value"]);
                    }
                }
                var currentArticle = m_dtoMapper.GetArticle(dtoArticle);
                currentArticle.Author = new Author()
                {
                    Id = dtoArticle.AuthorId,
                    FirstName = m_adoHelper.GetCellValue("User", "FirstName", dtoArticle.AuthorId).ToString(),
                    LastName = m_adoHelper.GetCellValue("User", "LastName", dtoArticle.AuthorId).ToString(),
                    Age = Convert.ToInt32(m_adoHelper.GetCellValue("User", "Age", dtoArticle.AuthorId)),
                    NickName = m_adoHelper.GetCellValue("Author", "NickName", "Id", m_adoHelper.GetCellValue("User", "AuthorId", dtoArticle.AuthorId)).ToString(),
                    Popularity = Convert.ToDecimal(m_adoHelper.GetCellValue("Author", "Popularity", "Id", m_adoHelper.GetCellValue("User", "AuthorId", dtoArticle.AuthorId)))
                };

                articles.Add(currentArticle);
            }
            return articles;
        }

        public List<Article> GetSorted(string sortBy, int from, int count, string order)
        {
            var articles = new List<Article>();
            DataTable articlesTable;
            if (sortBy != "" && order != "")
            {
                articlesTable = m_adoHelper.GetSortedData("Article", sortBy, from, count, order);
            }
            else
            {
                articlesTable = m_adoHelper.GetData("Article", from, count);
            }
            for (var i = 0; i < articlesTable.Rows.Count; i++)
            {
                var dtoArticle = new DtoArticle
                {
                    Id = (int)articlesTable.Rows[i]["Id"],
                    Title = articlesTable.Rows[i]["Title"].ToString(),
                    Content = articlesTable.Rows[i]["Content"].ToString(),
                    AuthorId = Convert.ToInt32(m_adoHelper.GetCellValue("User", "Id", "AuthorId", articlesTable.Rows[i]["AuthorId"])),
                    Ratings = new List<object>()
                };
                var rating = m_adoHelper.GetData("Rating").Rows;
                for (int j = 0; j < rating.Count; j++)
                {
                    if ((int)rating[j]["ArticleId"] == (int)articlesTable.Rows[i]["Id"])
                    {
                        dtoArticle.Ratings.Add((int)rating[j]["Value"]);
                    }
                }
                var textRating = m_adoHelper.GetData("TextRating").Rows;
                for (var j = 0; j < textRating.Count; j++)
                {
                    if ((int)textRating[j]["ArticleId"] == (int)articlesTable.Rows[i]["Id"])
                    {
                        dtoArticle.Ratings.Add(textRating[j]["Value"]);
                    }
                }
                var currentArticle = m_dtoMapper.GetArticle(dtoArticle);
                currentArticle.Author = new Author()
                {
                    Id = dtoArticle.AuthorId,
                    FirstName = m_adoHelper.GetCellValue("User", "FirstName", dtoArticle.AuthorId).ToString(),
                    LastName = m_adoHelper.GetCellValue("User", "LastName", dtoArticle.AuthorId).ToString(),
                    Age = Convert.ToInt32(m_adoHelper.GetCellValue("User", "Age", dtoArticle.AuthorId)),
                    NickName = m_adoHelper.GetCellValue("Author", "NickName", "Id", m_adoHelper.GetCellValue("User", "AuthorId", dtoArticle.AuthorId)).ToString(),
                    Popularity = Convert.ToDecimal(m_adoHelper.GetCellValue("Author", "Popularity", "Id", m_adoHelper.GetCellValue("User", "AuthorId", dtoArticle.AuthorId)))
                };

                articles.Add(currentArticle);
            }
            return articles;
        } 

        public List<Article> Get(int id)
        {
            //
            return Get();
        }

        public int GetCount()
        {
            return m_adoHelper.GetCount("Article");
        }

        public int Save(Article entity)
        {
            if (Exists(entity.Id)) return Update(entity.Id, entity);
            var authorId = (Guid)m_adoHelper.GetCellValue("User", "AuthorId", entity.Author.Id);
            var command = string.Format("INSERT INTO [dbo].[Article](Title,Content,AuthorId) OUTPUT Inserted.Id VALUES('{0}','{1}','{2}') ", entity.Title, entity.Content, authorId);
            return (int) m_adoHelper.CrudOperation(command);
        }

        public void Delete(int articleId)
        {
            var cmdText = string.Format("DELETE FROM [dbo].[Article] WHERE Id={0}", articleId);
            m_adoHelper.CrudOperation(cmdText);
        }

        public int Update(int oldArticleId, Article newArticle)
        {
            var commandText = string.Format("UPDATE [dbo].[Article] SET Title='{0}',Content='{1}' WHERE Id={2}", newArticle.Title, newArticle.Content, oldArticleId);
            return Convert.ToInt32(m_adoHelper.CrudOperation(commandText));
        }

        public Article GetById(int? id)
        {
            var articleTable = m_adoHelper.GetData("Article", id).Rows[0];
            var dtoArticle = new DtoArticle
            {
                Id = Convert.ToInt32(articleTable["Id"]),
                Title = articleTable["Title"].ToString(),
                Content = articleTable["Content"].ToString(),
                AuthorId = Convert.ToInt32(m_adoHelper.GetCellValue("User", "Id", "AuthorId", articleTable["AuthorId"])),
                AuthorFirstName = m_adoHelper.GetCellValue("User", "FirstName", "AuthorId", articleTable["AuthorId"]).ToString(),
                AuthorLastName = m_adoHelper.GetCellValue("User", "LastName", "AuthorId", articleTable["AuthorId"]).ToString(),
                AuthorAge = (int)m_adoHelper.GetCellValue("User", "Age", "AuthorId", articleTable["AuthorId"]),
                AuthorNickName = m_adoHelper.GetCellValue("Author", "NickName", "Id", articleTable["AuthorId"]).ToString(),
                AuthorPopularity = Convert.ToDecimal(m_adoHelper.GetCellValue("Author", "Popularity", "Id", articleTable["AuthorId"])),
                Ratings = new List<object>()
            };
            var rating = m_adoHelper.GetData("Rating").Rows;
            for (var i = 0; i < rating.Count; i++)
            {
                if (rating[i]["ArticleId"] == articleTable["Id"])
                {
                    dtoArticle.Ratings.Add((int)rating[i]["Value"]);
                }
            }
            var textRating = m_adoHelper.GetData("TextRating").Rows;
            for (var i = 0; i < textRating.Count; i++)
            {
                if (textRating[i]["ArticleId"] == articleTable["Id"])
                {
                    dtoArticle.Ratings.Add(textRating[i]["Value"]);
                }
            }
            var article = m_dtoMapper.GetArticle(dtoArticle);
            return article;
        }

        public Article GetRandom()
        {
            var random = new Random();
            var table = m_adoHelper.GetData("Article");
            var articleTable = table.Rows[random.Next(0, table.Rows.Count - 1)];
            var article = new DtoArticle
            {
                Id = (int)articleTable["Id"],
                Title = articleTable["Title"].ToString(),
                Content = articleTable["Content"].ToString(),
                AuthorId = Convert.ToInt32(m_adoHelper.GetCellValue("User", "Id", "AuthorId", articleTable["AuthorId"])),
                Ratings = new List<object>()
            };
            var rating = m_adoHelper.GetData("Rating").Rows;
            for (var i = 0; i < rating.Count; i++)
            {
                if (rating[i]["ArticleId"] == articleTable["Id"])
                {
                    article.Ratings.Add((int)rating[i]["Value"]);
                }
            }
            var textRating = m_adoHelper.GetData("TextRating").Rows;
            for (var i = 0; i < textRating.Count; i++)
            {
                if (textRating[i]["ArticleId"] == articleTable["Id"])
                {
                    article.Ratings.Add(textRating[i]["Value"]);
                }
            }
            return m_dtoMapper.GetArticle(article);
        }

        private bool Exists(int id)
        {
            var article = m_adoHelper.GetCellValue("Article", "Id", id);
            return article != null;
        }
    }          
}