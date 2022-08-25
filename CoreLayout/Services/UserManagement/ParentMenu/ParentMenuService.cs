using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.Menu;
using CoreLayout.Repositories.UserManagement.ParentMenu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.ParentMenu
{
    public class ParentMenuService : IParentMenuService
    {
        private readonly IParentMenuRepository _parentMenuRepository;

        public ParentMenuService(IParentMenuRepository parentMenuRepository)
        {
            _parentMenuRepository = parentMenuRepository;
        }

        public async Task<List<ParentMenuModel>> GetAllParentMenuAsync()
        {
            return await _parentMenuRepository.GetAllAsync();
        }

        public async Task<ParentMenuModel> GetParentMenuByIdAsync(int id)
        {
            return await _parentMenuRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateParentMenuAsync(ParentMenuModel parentMenuModel)
        {
            return await _parentMenuRepository.CreateAsync(parentMenuModel);
        }

        public async Task<int> UpdateParentMenuAsync(ParentMenuModel parentMenuModel)
        {
            return await _parentMenuRepository.UpdateAsync(parentMenuModel);
        }

        public async Task<int> DeleteParentMenuAsync(ParentMenuModel parentMenuModel)
        {
            return await _parentMenuRepository.DeleteAsync(parentMenuModel);
        }
    }
}
