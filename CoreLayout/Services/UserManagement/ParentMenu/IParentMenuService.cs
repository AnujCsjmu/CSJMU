using CoreLayout.Models.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.ParentMenu
{
    public interface IParentMenuService
    {
        public Task<List<ParentMenuModel>> GetAllParentMenuAsync();
        public Task<ParentMenuModel> GetParentMenuByIdAsync(int id);
        public Task<int> CreateParentMenuAsync(ParentMenuModel parentMenuModel);
        public Task<int> UpdateParentMenuAsync(ParentMenuModel parentMenuModel);
        public Task<int> DeleteParentMenuAsync(ParentMenuModel parentMenuModel);
    }
}
