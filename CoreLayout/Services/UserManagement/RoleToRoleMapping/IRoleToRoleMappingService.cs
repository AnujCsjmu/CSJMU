using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.RoleToRoleMapping
{
    public interface IRoleToRoleMappingService
    {
        public Task<List<RoleToRoleMappingModel>> GetAllRoleToRoleMappingAsync();
        public Task<RoleToRoleMappingModel> GetRoleToRoleMappingByIdAsync(int id);
        public Task<int> CreateRoleToRoleMappingAsync(RoleToRoleMappingModel roleToRoleMappingModel);
        public Task<int> UpdateRoleToRoleMappingAsync(RoleToRoleMappingModel roleToRoleMappingModel);
        public Task<int> DeleteRoleToRoleMappingAsync(RoleToRoleMappingModel roleToRoleMappingModel);

    }
}
