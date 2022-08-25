using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.District;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.District
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;

        public DistrictService(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public async Task<List<DistrictModel>> GetAllDistrict()
        {
            return await _districtRepository.GetAllAsync();
        }

        public async Task<DistrictModel> GetDistrictById(int id)
        {
            return await _districtRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateDistrictAsync(DistrictModel districtModel)
        {
            return await _districtRepository.CreateAsync(districtModel);
        }

        public async Task<int> UpdateDistrictAsync(DistrictModel districtModel)
        {
            return await _districtRepository.UpdateAsync(districtModel);
        }

        public async Task<int> DeleteDistrictAsync(DistrictModel districtModel)
        {
            return await _districtRepository.DeleteAsync(districtModel);
        }
    }
}
