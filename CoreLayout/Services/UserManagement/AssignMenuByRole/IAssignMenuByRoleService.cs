using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.AssignMenuByRole
{
    public interface IAssignMenuByRoleService
    {
        public Task<List<AssignMenuByRoleModel>> GetAllMenuAssignByRoleAsync();

        public Task<List<AssignMenuByRoleModel>> AlreadyExitAsync(int menuid,int roleid);
        public Task<AssignMenuByRoleModel> GetMenuAssignByRoleByIdAsync(int id);
        public Task<int> CreateMenuAssignByRoleAsync(AssignMenuByRoleModel assignMenuByRoleModel);
        public Task<int> UpdateMenuAssignByRoleAsync(AssignMenuByRoleModel assignMenuByRoleModel);
        public Task<int> DeleteMenuAssignByRoleAsync(AssignMenuByRoleModel assignMenuByRoleModel);
    }
}
