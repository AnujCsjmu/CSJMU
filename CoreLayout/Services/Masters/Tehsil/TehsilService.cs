using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.District;
using CoreLayout.Repositories.Masters.State;
using CoreLayout.Repositories.Masters.Tehsil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Tehsil
{
    public class TehsilService : ITehsilService
    {
        private readonly ITehsilRepository _tehsilRepository;

        public TehsilService(ITehsilRepository tehsilRepository)
        {
            _tehsilRepository = tehsilRepository;
        }

        public async Task<List<TehsilModel>> GetAllTehsil()
        {
            return await _tehsilRepository.GetAllAsync();
        }

        public async Task<TehsilModel> GetTehsilById(int id)
        {
            return await _tehsilRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateTehsilAsync(TehsilModel tehsilModel)
        {
            return await _tehsilRepository.CreateAsync(tehsilModel);
        }

        public async Task<int> UpdateTehsilAsync(TehsilModel tehsilModel)
        {
            return await _tehsilRepository.UpdateAsync(tehsilModel);
        }

        public async Task<int> DeleteTehsilAsync(TehsilModel tehsilModel)
        {
            return await _tehsilRepository.DeleteAsync(tehsilModel);
        }
    }
}
