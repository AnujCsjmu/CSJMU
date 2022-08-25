using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.AssignRole;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.AssignRole
{
    public class AssignRoleService : IAssignRoleService
    {
        private readonly IAssignRoleRepository _assignRoleRepository;

        public AssignRoleService(IAssignRoleRepository assignRoleRepository)
        {
            _assignRoleRepository = assignRoleRepository;
        }
        public async Task<List<RegistrationRoleMapping>> GetAllRoleAssignAsync()
        {
            return await _assignRoleRepository.GetAllAsync();
        }

        public async Task<RegistrationRoleMapping> GetRoleAssignByIdAsync(int id)
        {
            return await _assignRoleRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateRoleAssignAsync(RegistrationRoleMapping registrationRoleMapping)
        {
            return await _assignRoleRepository.CreateAsync(registrationRoleMapping);
        }

        public async Task<int> UpdateRoleAssignAsync(RegistrationRoleMapping registrationRoleMapping)
        {
            return await _assignRoleRepository.UpdateAsync(registrationRoleMapping);
        }

        public async Task<int> DeleteRoleAssignAsync(RegistrationRoleMapping registrationRoleMapping)
        {
            return await _assignRoleRepository.DeleteAsync(registrationRoleMapping);
        }
    }
}
