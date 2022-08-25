using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.AssignMenuByUser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.AssignMenuByUser
{
    public class AssignMenuByUserService : IAssignMenuByUserService
    {
        private readonly IAssignMenuByUserRepository _assignMenuByUserRepository;

        public AssignMenuByUserService(IAssignMenuByUserRepository assignMenuByUserRepository)
        {
            _assignMenuByUserRepository = assignMenuByUserRepository;
        }
        public async Task<List<AssignMenuByUserModel>> GetAllMenuAssignByUserAsync()
        {
            return await _assignMenuByUserRepository.GetAllAsync();
        }
        public async Task<List<AssignMenuByUserModel>> AlreadyExitAsync(int menuid, int userid)
        {
            return await _assignMenuByUserRepository.CheckAlreadyAsync(menuid, userid);
        }
        public async Task<AssignMenuByUserModel> GetMenuAssignByUserByIdAsync(int id)
        {
            return await _assignMenuByUserRepository.GetByIdAsync(id);
        }
       
        public async Task<int> CreateMenuAssignByUserAsync(AssignMenuByUserModel assignMenuByUserModel)
        {
            return await _assignMenuByUserRepository.CreateAsync(assignMenuByUserModel);
        }

        public async Task<int> UpdateMenuAssignByUserAsync(AssignMenuByUserModel assignMenuByUserModel)
        {
            return await _assignMenuByUserRepository.UpdateAsync(assignMenuByUserModel);
        }

        public async Task<int> DeleteMenuAssignByUserAsync(AssignMenuByUserModel assignMenuByUserModel)
        {
            return await _assignMenuByUserRepository.DeleteAsync(assignMenuByUserModel);
        }
    }
}
