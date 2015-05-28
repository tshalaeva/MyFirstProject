using System;
using System.Collections.Generic;
using DataAccessLayer.DtoEntities;
using ObjectRepository.Entities;

namespace DataAccessLayer.Repositories
{
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
            var privilegiesId = Guid.NewGuid();

            var cmdText1 =
                string.Format(
                    "INSERT INTO [dbo].[User] (FirstName,LastName,Age,PrivilegiesId) OUTPUT Inserted.Id VALUES ('{0}','{1}',{2},'{3}')",
                    entity.FirstName, entity.LastName, entity.Age, privilegiesId);
            var privilegyList = entity.Privilegies;
            var cmdText2 = string.Format("INSERT INTO Privilegies (Id,List) VALUES ('{0}', '{1}'); ",
                privilegiesId, GetPrivilegiesString(privilegyList));
            return (int)adminAdoHelper.CrudOperation(cmdText1, cmdText2);
        }

        public void DeleteAdmin(int adminId)
        {
            var privilegyId = (Guid)AdoHelper.GetCellValue("User", "PrivilegiesId", adminId);
            var command1 = string.Format("DELETE FROM [dbo].[User] WHERE Id={0})", adminId);
            var command2 = string.Format("DELETE FROM [dbo].[Privilegies] WHERE Id='{0}'", privilegyId);
            adminAdoHelper.CrudOperation(command1, command2);
        }

        public int UpdateAdmin(int oldAdminId, Admin newAdmin)
        {
            var commandText = string.Format("UPDATE [dbo].[User] SET FirstName='{0}',LastName='{1}',Age={2} WHERE Id={3}", newAdmin.FirstName, newAdmin.LastName, newAdmin.Age, oldAdminId);
            var privilegiesId = (Guid)adminAdoHelper.GetCellValue("User", "PrivilegiesId", oldAdminId);
            var cmd = string.Format("UPDATE [dbo].[Privilegies] SET List='{0}' WHERE Id='{1}' ", GetPrivilegiesString(newAdmin.Privilegies), privilegiesId);
            adminAdoHelper.CrudOperation(cmd);
            return Convert.ToInt32(adminAdoHelper.CrudOperation(commandText));
        }
    }
}
