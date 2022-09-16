using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.AssignRole;
using CoreLayout.Repositories.UserManagement.RoleToRoleMapping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.RoleToRoleMapping
{
    public class RoleToRoleMappingService : IRoleToRoleMappingService
    {
        private readonly IRoleToRoleMappingRepository _roleToRoleMappingRepository;

        public RoleToRoleMappingService(IRoleToRoleMappingRepository roleToRoleMappingRepository)
        {
            _roleToRoleMappingRepository = roleToRoleMappingRepository;
        }
        public async Task<List<RoleToRoleMappingModel>> GetAllRoleToRoleMappingAsync()
        {
            return await _roleToRoleMappingRepository.GetAllAsync();
        }

        public async Task<RoleToRoleMappingModel> GetRoleToRoleMappingByIdAsync(int id)
        {
            return await _roleToRoleMappingRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateRoleToRoleMappingAsync(RoleToRoleMappingModel roleToRoleMappingModel)
        {
            return await _roleToRoleMappingRepository.CreateAsync(roleToRoleMappingModel);
        }

        public async Task<int> UpdateRoleToRoleMappingAsync(RoleToRoleMappingModel roleToRoleMappingModel)
        {
            return await _roleToRoleMappingRepository.UpdateAsync(roleToRoleMappingModel);
        }

        public async Task<int> DeleteRoleToRoleMappingAsync(RoleToRoleMappingModel roleToRoleMappingModel)
        {
            return await _roleToRoleMappingRepository.DeleteAsync(roleToRoleMappingModel);
        }

        
    }
}
