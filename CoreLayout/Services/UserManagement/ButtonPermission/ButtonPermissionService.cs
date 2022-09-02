using CoreLayout.Enum;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.ButtonPermission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.ButtonPermission
{
    public class ButtonPermissionService : IButtonPermissionService
    {
        private readonly IButtonPermissionRepository _buttonPermissionRepository;

        public ButtonPermissionService(IButtonPermissionRepository buttonPermissionRepository)
        {
            _buttonPermissionRepository = buttonPermissionRepository;
        }

        public async Task<List<ButtonPermissionModel>> GetAllButtonPermissionAsync()
        {
            return await _buttonPermissionRepository.GetAllAsync();
        }

        public async Task<ButtonPermissionModel> GetButtonPermissionByIdAsync(int id)
        {
            return await _buttonPermissionRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel)
        {
            return await _buttonPermissionRepository.CreateAsync(buttonPermissionModel);
        }

        public async Task<int> UpdateButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel)
        {
            return await _buttonPermissionRepository.UpdateAsync(buttonPermissionModel);
        }

        public async Task<int> DeleteButtonPermissionAsync(ButtonPermissionModel buttonPermissionModel)
        {
            return await _buttonPermissionRepository.DeleteAsync(buttonPermissionModel);
        }
        public async Task<List<RegistrationModel>> GetAllUsersAsync(int roleid)
        {
            return await _buttonPermissionRepository.GetAllUserAsync(roleid);
        }
        public async Task<List<ButtonPermissionModel>> GetAllButtonPermissionUserWiseAsync(int userid)
        {
            return await _buttonPermissionRepository.GetAllButtonPermissionUserWiseAsync(userid);
        }
        public async Task<List<ButtonPermissionModel>> GetAllButtonPermissionMenuWiseAsync(int menuid)
        {
            return await _buttonPermissionRepository.GetAllButtonPermissionMenuWiseAsync(menuid);
        }
        public async Task<List<ButtonPermissionModel>> DistinctButtonPermissionAsync()
        {
            return await _buttonPermissionRepository.DistinctButtonPermissionAsync();
        }
        public async Task<List<ButtonPermissionModel>> GetAllButtonActionPermissionAsync(ViewAction viewAction, int userid, int roleid, string controller)
        {
            return await _buttonPermissionRepository.GetAllButtonActionPermissionAsync(viewAction, userid, roleid, controller);
        }
        public async Task<List<ButtonPermissionModel>> CheckAllButtonActionPermissionAsync(int buttonid, int userid, int roleid, string controller, string index)
        {
            return await _buttonPermissionRepository.CheckAllButtonActionPermissionAsync(buttonid, userid, roleid, controller, index);
        }

        public async Task<List<ButtonPermissionModel>> AlreadyExit(int buttonid, int userid, int roleid, int menuid)
        {
            return await _buttonPermissionRepository.AlreadyExit(buttonid, userid, roleid, menuid);
        }
    }
}
