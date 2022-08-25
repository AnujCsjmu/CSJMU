using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.City
{
    public interface ICityService
    {
        public Task<List<CityModel>> GetAllCity();
        public Task<CityModel> GetCityById(int id);
        public Task<int> CreateCityAsync(CityModel cityModel);
        public Task<int> UpdateCityAsync(CityModel cityModel);
        public Task<int> DeleteCityAsync(CityModel cityModel);
    }
}
