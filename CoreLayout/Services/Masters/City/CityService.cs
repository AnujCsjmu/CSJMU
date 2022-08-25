using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.City
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<List<CityModel>> GetAllCity()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<CityModel> GetCityById(int id)
        {
            return await _cityRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateCityAsync(CityModel cityModel)
        {
            return await _cityRepository.CreateAsync(cityModel);
        }

        public async Task<int> UpdateCityAsync(CityModel cityModel)
        {
            return await _cityRepository.UpdateAsync(cityModel);
        }

        public async Task<int> DeleteCityAsync(CityModel cityModel )
        {
            return await _cityRepository.DeleteAsync(cityModel);
        }
    }
}
