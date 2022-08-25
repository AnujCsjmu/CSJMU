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
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<CountryModel>> GetAllCountry()
        {
            return await _countryRepository.GetAllAsync();
        }

        public async Task<CountryModel> GetCountryById(int id)
        {
            return await _countryRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateCountryAsync(CountryModel countryModel)
        {
            return await _countryRepository.CreateAsync(countryModel);
        }

        public async Task<int> UpdateCountryAsync(CountryModel countryModel)
        {
            return await _countryRepository.UpdateAsync(countryModel);
        }

        public async Task<int> DeleteCountryAsync(CountryModel countryModel)
        {
            return await _countryRepository.DeleteAsync(countryModel);
        }
    }
}
