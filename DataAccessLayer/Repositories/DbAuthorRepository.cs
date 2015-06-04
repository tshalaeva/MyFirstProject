using System;
using System.Collections.Generic;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;
using StructureMap;

namespace DataAccessLayer.Repositories
{
    public class DbAuthorRepository : DbUserRepository, IRepository<Author>
    {
        public DbAuthorRepository()
        {
        }

        [DefaultConstructor]
        public DbAuthorRepository(AdoHelper adoHelper, DtoMapper dtoMapper) : base(adoHelper, dtoMapper)
        {}

        public void DeleteAuthor(int authorId)
        {
            var authorGuid = (Guid)AdoHelper.GetCellValue("User", "AuthorId", authorId);
            var command1 = string.Format("DELETE FROM [dbo].[User] WHERE Id={0}", authorId);
            var command2 = string.Format("DELETE FROM [dbo].[Author] WHERE Id='{0}'", authorGuid);
            AdoHelper.CrudOperation(command1, command2);
        }

        public new List<Author> Get()
        {
            var authorTable = AdoHelper.GetData("Author");
            var authors = new List<Author>();
            for (var i = 0; i < authorTable.Rows.Count; i++)
            {
                var tmpUser = new DtoUser
                {
                    Id = (int)AdoHelper.GetCellValue("User", "Id", "AuthorId", authorTable.Rows[i]["Id"]),
                    FirstName = AdoHelper.GetCellValue("User", "FirstName", "AuthorId", authorTable.Rows[i]["Id"]).ToString(),
                    LastName = AdoHelper.GetCellValue("User", "LastName", "AuthorId", authorTable.Rows[i]["Id"]).ToString(),
                    Age = (int)AdoHelper.GetCellValue("User", "Age", "AuthorId", authorTable.Rows[i]["Id"]),
                    NickName = authorTable.Rows[i]["NickName"].ToString(),
                    Popularity = Convert.ToDecimal(authorTable.Rows[i]["Popularity"])
                };
                authors.Add(DtoMapper.GetAuthor(tmpUser));
            }
            return authors;
        }

        public new List<Author> Get(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public new List<Author> Get(int filteredById)
        {
            throw new NotImplementedException();
        }

        public int Save(Author entity)
        {
            var authorId = Guid.NewGuid();
            var command1 = string.Format("INSERT INTO [dbo].[User](FirstName,LastName,Age,AuthorId) OUTPUT Inserted.Id VALUES ('{0}','{1}',{2},'{3}')", entity.FirstName, entity.LastName, entity.Age, authorId);
            var command2 = string.Format("INSERT INTO [dbo].[Author](Id,NickName,Popularity) VALUES ('{0}','{1}','{2}')", authorId, entity.NickName, entity.Popularity);
            return (int)AdoHelper.CrudOperation(command1, command2);
        }

        public int Update(int oldAuthorId, Author newAuthor)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newAuthor.FirstName, newAuthor.LastName, newAuthor.Age, oldAuthorId);
            var authorId = AdoHelper.GetCellValue("User", "AuthorId", oldAuthorId);
            var authorCmd = string.Format("UPDATE [dbo].[Author] SET NickName='{0}',Popularity='{1}' WHERE Id='{2}'; ", newAuthor.NickName, newAuthor.Popularity, authorId);
            AdoHelper.CrudOperation(authorCmd);
            return Convert.ToInt32(AdoHelper.CrudOperation(commandText));
        }

        public new Author GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public new Author GetRandom()
        {
            throw new NotImplementedException();
        }
    }
}
