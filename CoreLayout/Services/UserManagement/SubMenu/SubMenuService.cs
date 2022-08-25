using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.Menu;
using CoreLayout.Repositories.UserManagement.ParentMenu;
using CoreLayout.Repositories.UserManagement.SubMenu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.SubMenu
{
    public class SubMenuService : ISubMenuService
    {
        private readonly ISubMenuRepository _subMenuRepository;

        public SubMenuService(ISubMenuRepository subMenuRepository)
        {
            _subMenuRepository = subMenuRepository;
        }

        public async Task<List<SubMenuModel>> GetAllSubMenuAsync()
        {
            return await _subMenuRepository.GetAllAsync();
        }

        public async Task<SubMenuModel> GetSubMenuByIdAsync(int id)
        {
            return await _subMenuRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateSubMenuAsync(SubMenuModel subMenuModel)
        {
            return await _subMenuRepository.CreateAsync(subMenuModel);
        }

        public async Task<int> UpdateSubMenuAsync(SubMenuModel subMenuModel)
        {
            return await _subMenuRepository.UpdateAsync(subMenuModel);
        }

        public async Task<int> DeleteSubMenuAsync(SubMenuModel subMenuModel)
        {
            return await _subMenuRepository.DeleteAsync(subMenuModel);
        }
    }
}
