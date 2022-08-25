using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.AssignMenuByUser
{
    public interface IAssignMenuByUserService
    {
        public Task<List<AssignMenuByUserModel>> GetAllMenuAssignByUserAsync();

        public Task<List<AssignMenuByUserModel>> AlreadyExitAsync(int menuid, int userid);
        public Task<AssignMenuByUserModel> GetMenuAssignByUserByIdAsync(int id);
        public Task<int> CreateMenuAssignByUserAsync(AssignMenuByUserModel assignMenuByUserModel);
        public Task<int> UpdateMenuAssignByUserAsync(AssignMenuByUserModel assignMenuByUserModel);
        public Task<int> DeleteMenuAssignByUserAsync(AssignMenuByUserModel assignMenuByUserModel);
    }
}
