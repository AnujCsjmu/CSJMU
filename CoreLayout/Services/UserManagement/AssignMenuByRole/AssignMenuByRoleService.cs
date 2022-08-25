using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.AssignMenuByRole;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.AssignMenuByRole
{
    public class AssignMenuByRoleService : IAssignMenuByRoleService
    {
        private readonly IAssignMenuByRoleRepository _assignMenuByRoleRepository ;

        public AssignMenuByRoleService(IAssignMenuByRoleRepository assignMenuByRoleRepository)
        {
            _assignMenuByRoleRepository = assignMenuByRoleRepository;
        }
        public async Task<List<AssignMenuByRoleModel>> GetAllMenuAssignByRoleAsync()
        {
            return await _assignMenuByRoleRepository.GetAllAsync();
        }
        public async Task<List<AssignMenuByRoleModel>> AlreadyExitAsync(int menuid,int roleid)
        {
            return await _assignMenuByRoleRepository.CheckAlreadyAsync(menuid, roleid);
        }
        public async Task<AssignMenuByRoleModel> GetMenuAssignByRoleByIdAsync(int id)
        {
            return await _assignMenuByRoleRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateMenuAssignByRoleAsync(AssignMenuByRoleModel assignMenuByRoleModel)
        {
            return await _assignMenuByRoleRepository.CreateAsync(assignMenuByRoleModel);
        }

        public async Task<int> UpdateMenuAssignByRoleAsync(AssignMenuByRoleModel assignMenuByRoleModel)
        {
            return await _assignMenuByRoleRepository.UpdateAsync(assignMenuByRoleModel);
        }

        public async Task<int> DeleteMenuAssignByRoleAsync(AssignMenuByRoleModel assignMenuByRoleModel)
        {
            return await _assignMenuByRoleRepository.DeleteAsync(assignMenuByRoleModel);
        }
    }
}
