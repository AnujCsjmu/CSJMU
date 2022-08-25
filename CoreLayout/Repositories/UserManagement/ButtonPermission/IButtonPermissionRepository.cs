using CoreLayout.Enum;
using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.ButtonPermission
{
    public interface IButtonPermissionRepository : IRepository<ButtonPermissionModel>
    {
        Task<List<RegistrationModel>> GetAllUserAsync(int roleid);

        Task<List<ButtonPermissionModel>> GetAllButtonPermissionUserWiseAsync(int userid);
        Task<List<ButtonPermissionModel>> DistinctButtonPermissionAsync();

        Task<List<ButtonPermissionModel>> GetAllButtonActionPermissionAsync(ViewAction viewAction, int userid, int roleid, string controller);

        Task<List<ButtonPermissionModel>> CheckAllButtonActionPermissionAsync(int buttonid, int userid, int roleid, string controller, string index);
    }
}
