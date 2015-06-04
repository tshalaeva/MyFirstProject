using System;
using System.Collections.Generic;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;
using StructureMap;

namespace DataAccessLayer.Repositories
{
    public class DbAdminRepository : DbUserRepository, IRepository<Admin>
    {
        public DbAdminRepository()
        {
        }

        [DefaultConstructor]
        public DbAdminRepository(AdoHelper adoHelper, DtoMapper dto) : base(adoHelper, dto)
        {
        }

        public void DeleteAdmin(int adminId)
        {
            var privilegyId = (Guid)AdoHelper.GetCellValue("User", "PrivilegiesId", adminId);
            var command1 = string.Format("DELETE FROM [dbo].[User] WHERE Id={0})", adminId);
            var command2 = string.Format("DELETE FROM [dbo].[Privilegies] WHERE Id='{0}'", privilegyId);
            AdoHelper.CrudOperation(command1, command2);
        }

        public new List<Admin> Get()
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

        public new List<Admin> Get(int @from, int count)
        {
            throw new NotImplementedException();
        }

        public new List<Admin> Get(int filteredById)
        {
            throw new NotImplementedException();
        }

        public int Save(Admin entity)
        {
            var privilegiesId = Guid.NewGuid();

            var cmdText1 =
                string.Format(
                    "INSERT INTO [dbo].[User] (FirstName,LastName,Age,PrivilegiesId) OUTPUT Inserted.Id VALUES ('{0}','{1}',{2},'{3}')",
                    entity.FirstName, entity.LastName, entity.Age, privilegiesId);
            var privilegyList = entity.Privilegies;
            var cmdText2 = string.Format("INSERT INTO Privilegies (Id,List) VALUES ('{0}', '{1}'); ",
                privilegiesId, GetPrivilegiesString(privilegyList));
            return (int)AdoHelper.CrudOperation(cmdText1, cmdText2);
        }

        public int Update(int existingEntityId, Admin newAdmin)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newAdmin.FirstName, newAdmin.LastName, newAdmin.Age, existingEntityId);
            var privilegiesId = (Guid)AdoHelper.GetCellValue("User", "PrivilegiesId", existingEntityId);
            var cmd = string.Format("UPDATE [dbo].[Privilegies] SET List='{0}' WHERE Id='{1}' ", GetPrivilegiesString(newAdmin.Privilegies), privilegiesId);
            AdoHelper.CrudOperation(cmd);
            return Convert.ToInt32(AdoHelper.CrudOperation(commandText));
        }

        public new Admin GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public new Admin GetRandom()
        {
            throw new NotImplementedException();
        }
    }
}
