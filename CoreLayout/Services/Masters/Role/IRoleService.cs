using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Role
{
    public interface IRoleService
    {
        public Task<List<RoleModel>> GetAllRoleAsync();
        public Task<RoleModel> GetRoleByIdAsync(int id);
        public Task<int> CreateRoleAsync(RoleModel roleModel);
        public Task<int> UpdateRoleAsync(RoleModel roleModel);
        public Task<int> DeleteRoleAsync(RoleModel roleModel);
        public Task<List<RoleModel>> GetRoleToRoleMappingByRoleAsync(int FromRoleId);
    }
}
