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

        [DefaultConstructor]
        public DbUserRepository(AdoHelper ado, DtoMapper dtoMapper)
        {
            AdoHelper = ado;
            DtoMapper = dtoMapper;
        }

        public DbUserRepository()
        {
                        
        }

        public bool Initialized
        {
            get
            {
                var table = AdoHelper.GetData("User").Rows.Count;
                return table != 0;
            }
        }

        public int GetCount()
        {
            return AdoHelper.GetCount("User");
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

        public List<User> Get(int from, int count)
        {
        //
            return Get();
        }

        public List<User> Get(int id)
        {
            //
            return new List<User>();
        }

        public int Save(User entity)
        {
            if (Exists(entity.Id)) return Update(entity.Id, entity);
            var cmdText = new StringBuilder();
            //if (!(entity is Admin) && !(entity is Author))
            //{
                cmdText.AppendFormat(
                    "INSERT INTO [dbo].[User] (FirstName,LastName,Age) OUTPUT Inserted.Id VALUES('{0}','{1}',{2})",
                    entity.FirstName, entity.LastName, entity.Age);
                return (int)AdoHelper.CrudOperation(cmdText.ToString());
            //}
            //if (entity is Admin)
            //{
            //    return m_adminRepository.SaveAdmin((Admin)entity);
            //}
            //return m_authorRepository.SaveAuthor((Author)entity);
        }

        public void Delete(int userId)
        {
            var data = GetById(userId);
            var cmdText = new StringBuilder();
            //if (!(data is Admin) && !(data is Author))
            //{
                cmdText.AppendFormat("DELETE FROM [dbo].[User] WHERE Id='{0}'", userId);

                AdoHelper.CrudOperation(cmdText.ToString());
            //}
            //else
            //{
            //    if (data is Admin)
            //    {
            //        m_adminRepository.DeleteAdmin(userId);
            //    }
            //    else
            //    {
            //        m_authorRepository.DeleteAuthor(userId);
            //    }
            //}
        }

        public int Update(int oldUserId, User newUser)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newUser.FirstName, newUser.LastName, newUser.Age, oldUserId);
            //if (!(newUser is Admin) && !(newUser is Author))
            //{
                return Convert.ToInt32(AdoHelper.CrudOperation(commandText));
            //}
            //if (newUser is Admin)
            //{
            //    return m_adminRepository.UpdateAdmin(oldUserId, (Admin)newUser);
            //}
            //return m_authorRepository.UpdateAuthor(oldUserId, (Author)newUser);
        }

        public User GetById(int? id)
        {
            var userData = AdoHelper.GetData("User", (int)id).Rows[0];

            var dtoUser = new DtoUser
            {
                Id = (int)userData["Id"],
                FirstName = userData["FirstName"].ToString(),
                LastName = userData["LastName"].ToString(),
                Age = (int)userData["Age"], 
                Privilegies = (!userData.IsNull("PrivilegiesId")) ? AdoHelper.GetCellValue("Privilegies", "List", "Id", userData["PrivilegiesId"]).ToString() : string.Empty,
                NickName = (!userData.IsNull("AuthorId")) ? AdoHelper.GetCellValue("Author", "NickName", "Id", userData["AuthorId"]).ToString() : string.Empty,
                Popularity = (!userData.IsNull("AuthorId")) ?
                    Convert.ToDecimal(AdoHelper.GetCellValue("Author", "Popularity", "Id", userData["AuthorId"])) : 0
            };

            if (userData.IsNull("PrivilegiesId") && userData.IsNull("AuthorId"))
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
                Id = (int) userTable["Id"],
                FirstName = userTable["FirstName"].ToString(),
                LastName = userTable["LastName"].ToString(),
                Age = (int) userTable["Age"],
                Privilegies = AdoHelper.GetCellValue("Privilegies", "List", "Id", userTable["PrivilegiesId"]).ToString(),
                NickName = AdoHelper.GetCellValue("Author", "NickName", "Id", userTable["AuthorId"]).ToString(),
                Popularity =
                    Convert.ToDecimal(AdoHelper.GetCellValue("Author", "Popularity", "Id", userTable["AuthorId"]))
            };
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
}
