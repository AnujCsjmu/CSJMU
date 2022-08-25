using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Country
{
    public class ButtonService : IButtonService
    {
        private readonly IButtonRepository _buttonRepository;

        public ButtonService(IButtonRepository buttonRepository)
        {
            _buttonRepository = buttonRepository;
        }

        public async Task<List<ButtonModel>> GetAllButton()
        {
            return await _buttonRepository.GetAllAsync();
        }
        public async Task<List<ButtonModel>> GetAllButtonMapping(int buttonid)
        {
            return await _buttonRepository.GetAllButtonMapping(buttonid);
        }
        public async Task<ButtonModel> GetButtonById(int id)
        {
            return await _buttonRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateButtonAsync(ButtonModel buttonModel)
        {
            return await _buttonRepository.CreateAsync(buttonModel);
        }

        public async Task<int> UpdateButtonAsync(ButtonModel buttonModel)
        {
            return await _buttonRepository.UpdateAsync(buttonModel);
        }

        public async Task<int> DeleteButtonAsync(ButtonModel buttonModel)
        {
            return await _buttonRepository.DeleteAsync(buttonModel);
        }
    }
}
