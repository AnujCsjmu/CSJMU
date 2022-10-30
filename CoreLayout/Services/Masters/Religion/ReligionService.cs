using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.Religion;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Religion
{
    public class ReligionService : IReligionService
    {
        private readonly IReligionRepository _religionRepository;

        public ReligionService(IReligionRepository religionRepository)
        {
            _religionRepository = religionRepository;
        }

        public async Task<List<ReligionModel>> GetAllReligion()
        {
            return await _religionRepository.GetAllAsync();
        }

        //public async Task<StateModel> GetStateById(int id)
        //{
        //    return await _stateRepository.GetByIdAsync(id);
        //}

        //public async Task<int> CreateStateAsync(StateModel stateModel)
        //{
        //    return await _stateRepository.CreateAsync(stateModel);
        //}

        //public async Task<int> UpdateStateAsync(StateModel stateModel)
        //{
        //    return await _stateRepository.UpdateAsync(stateModel);
        //}

        //public async Task<int> DeleteStateAsync(StateModel stateModel)
        //{
        //    return await _stateRepository.DeleteAsync(stateModel);
        //}
    }
}
