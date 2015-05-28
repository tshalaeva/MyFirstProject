using System;
using System.Collections.Generic;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
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
            var authorId = Guid.NewGuid();
            var command1 = string.Format("INSERT INTO [dbo].[User](FirstName,LastName,Age,AuthorId) OUTPUT Inserted.Id VALUES ('{0}','{1}',{2},'{3}')", entity.FirstName, entity.LastName, entity.Age, authorId);
            var command2 = string.Format("INSERT INTO [dbo].[Author](Id,NickName,Popularity) VALUES ('{0}','{1}','{2}')", authorId, entity.NickName, entity.Popularity);
            return (int)authorAdoHelper.CrudOperation(command1, command2);
        }

        public void DeleteAuthor(int authorId)
        {
            var authorGuid = (Guid)authorAdoHelper.GetCellValue("User", "AuthorId", authorId);
            var command1 = string.Format("DELETE FROM [dbo].[User] WHERE Id={0}", authorId);
            var command2 = string.Format("DELETE FROM [dbo].[Author] WHERE Id='{0}'", authorGuid);
            authorAdoHelper.CrudOperation(command1, command2);
        }

        public int UpdateAuthor(int oldAuthorId, Author newAuthor)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newAuthor.FirstName, newAuthor.LastName, newAuthor.Age, oldAuthorId);
            var authorId = authorAdoHelper.GetCellValue("User", "AuthorId", oldAuthorId);
            var authorCmd = string.Format("UPDATE [dbo].[Author] SET NickName='{0}',Popularity='{1}' WHERE Id='{2}'; ", newAuthor.NickName, newAuthor.Popularity, authorId);
            authorAdoHelper.CrudOperation(authorCmd);
            return Convert.ToInt32(authorAdoHelper.CrudOperation(commandText));
        }
    }
}
