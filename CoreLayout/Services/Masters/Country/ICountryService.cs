using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Country
{
    public interface ICountryService
    {
        public Task<List<CountryModel>> GetAllCountry();
        public Task<CountryModel> GetCountryById(int id);
        public Task<int> CreateCountryAsync(CountryModel countryModel);
        public Task<int> UpdateCountryAsync(CountryModel countryModel);
        public Task<int> DeleteCountryAsync(CountryModel countryModel);
    }
}
