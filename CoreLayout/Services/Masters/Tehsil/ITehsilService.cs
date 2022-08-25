using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Tehsil
{
    public interface ITehsilService
    {
        public Task<List<TehsilModel>> GetAllTehsil();
        public Task<TehsilModel> GetTehsilById(int id);
        public Task<int> CreateTehsilAsync(TehsilModel tehsilModel);
        public Task<int> UpdateTehsilAsync(TehsilModel tehsilModel);
        public Task<int> DeleteTehsilAsync(TehsilModel tehsilModel);
    }
}
