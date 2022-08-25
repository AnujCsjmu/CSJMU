using CoreLayout.Models.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.SubMenu
{
    public interface ISubMenuService
    {
        public Task<List<SubMenuModel>> GetAllSubMenuAsync();
        public Task<SubMenuModel> GetSubMenuByIdAsync(int id);
        public Task<int> CreateSubMenuAsync(SubMenuModel subMenuModel);
        public Task<int> UpdateSubMenuAsync(SubMenuModel subMenuModel);
        public Task<int> DeleteSubMenuAsync(SubMenuModel subMenuModel);
    }
}
