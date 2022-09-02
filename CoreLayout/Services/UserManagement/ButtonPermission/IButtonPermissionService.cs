using CoreLayout.Enum;
using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.ButtonPermission
{
    public interface IButtonPermissionService
    {
        public Task<List<ButtonPermissionModel>> GetAllButtonPermissionAsync();
        public Task<ButtonPermissionModel> GetButtonPermissionByIdAsync(int id);
        public Task<int> CreateButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel);
        public Task<int> UpdateButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel);
        public Task<int> DeleteButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel);
        public Task<List<RegistrationModel>> GetAllUsersAsync(int roleid);

        public Task<List<ButtonPermissionModel>> GetAllButtonPermissionUserWiseAsync(int userid);
        public Task<List<ButtonPermissionModel>> GetAllButtonPermissionMenuWiseAsync(int menuid);

        public Task<List<ButtonPermissionModel>> DistinctButtonPermissionAsync();
        public Task<List<ButtonPermissionModel>> GetAllButtonActionPermissionAsync(ViewAction viewAction,int userid,int roleid,string controller);

        public Task<List<ButtonPermissionModel>> CheckAllButtonActionPermissionAsync(int buttonid, int userid, int roleid, string controller, string index);

        public Task<List<ButtonPermissionModel>> AlreadyExit(int buttonid, int userid, int roleid, int menuid);


    }
}
