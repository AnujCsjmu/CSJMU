using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.Menu
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuModel>> GetAllMenuAsync()
        {
            return await _menuRepository.GetAllAsync();
        }

        public async Task<MenuModel> GetMenuByIdAsync(int id)
        {
            return await _menuRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateMenuAsync(MenuModel menuModel)
        {
            return await _menuRepository.CreateAsync(menuModel);
        }

        public async Task<int> UpdateMenuAsync(MenuModel menuModel)
        {
            return await _menuRepository.UpdateAsync(menuModel);
        }

        public async Task<int> DeleteMenuAsync(MenuModel menuModel)
        {
            return await _menuRepository.DeleteAsync(menuModel);
        }

        public async Task<List<MenuModel>> GetMenuByRoleAndUserForInstitute(int roleid, int userid)
        {
            return await _menuRepository.GetMenuByRoleAndUserForInstitute(roleid, userid);
        }
    }
}
