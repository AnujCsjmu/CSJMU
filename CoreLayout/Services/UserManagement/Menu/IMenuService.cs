using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.Menu
{
    public interface IMenuService
    {
        public Task<List<MenuModel>> GetAllMenuAsync();
        public Task<MenuModel> GetMenuByIdAsync(int id);
        public Task<int> CreateMenuAsync(MenuModel menuModel);
        public Task<int> UpdateMenuAsync(MenuModel menuModel);
        public Task<int> DeleteMenuAsync(MenuModel menuModel);
    }
}
