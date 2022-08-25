using CoreLayout.Models.Masters;
using CoreLayout.Repositories.Masters.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<List<RoleModel>> GetAllRoleAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<RoleModel> GetRoleByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateRoleAsync(RoleModel roleModel)
        {
            return await _roleRepository.CreateAsync(roleModel);
        }

        public async Task<int> UpdateRoleAsync(RoleModel roleModel)
        {
            return await _roleRepository.UpdateAsync(roleModel);
        }

        public async Task<int> DeleteRoleAsync(RoleModel roleModel)
        {
            return await _roleRepository.DeleteAsync(roleModel);
        }
    }
}
