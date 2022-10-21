using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.District
{
    public interface IDistrictService
    {
        public Task<List<DistrictModel>> GetAllDistrict();
        public Task<DistrictModel> GetDistrictById(int id);
        public Task<int> CreateDistrictAsync(DistrictModel districtModel);
        public Task<int> UpdateDistrictAsync(DistrictModel districtModel);
        public Task<int> DeleteDistrictAsync(DistrictModel districtModel);
        public Task<List<DistrictModel>> Get7DistrictAsync();
    }
}
