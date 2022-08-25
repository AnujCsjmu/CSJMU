using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.AssignRole
{
    public interface IAssignRoleService
    {
        public Task<List<RegistrationRoleMapping>> GetAllRoleAssignAsync();
        public Task<RegistrationRoleMapping> GetRoleAssignByIdAsync(int id);
        public Task<int> CreateRoleAssignAsync(RegistrationRoleMapping roleModel);
        public Task<int> UpdateRoleAssignAsync(RegistrationRoleMapping roleModel);
        public Task<int> DeleteRoleAssignAsync(RegistrationRoleMapping roleModel);
    }
}
