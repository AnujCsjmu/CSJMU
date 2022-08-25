using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Country
{
    public interface IButtonService
    {
        public Task<List<ButtonModel>> GetAllButton();

        public Task<List<ButtonModel>> GetAllButtonMapping(int buttonid);
        public Task<ButtonModel> GetButtonById(int id);
        public Task<int> CreateButtonAsync(ButtonModel buttonModel);
        public Task<int> UpdateButtonAsync(ButtonModel buttonModel);
        public Task<int> DeleteButtonAsync(ButtonModel buttonModel);
    }
}
