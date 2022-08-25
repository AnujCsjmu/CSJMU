using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.State
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<List<StateModel>> GetAllState()
        {
            return await _stateRepository.GetAllAsync();
        }

        public async Task<StateModel> GetStateById(int id)
        {
            return await _stateRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateStateAsync(StateModel stateModel)
        {
            return await _stateRepository.CreateAsync(stateModel);
        }

        public async Task<int> UpdateStateAsync(StateModel stateModel)
        {
            return await _stateRepository.UpdateAsync(stateModel);
        }

        public async Task<int> DeleteStateAsync(StateModel stateModel)
        {
            return await _stateRepository.DeleteAsync(stateModel);
        }
    }
}
