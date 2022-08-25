using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Pratice
{
    public interface IPraticeService
    {
        public Task<List<PraticeModel>> GetAllPratice();
        public Task<List<PraticeChildModel>> GetAllChildAsync();
        public Task<PraticeModel> GetPraticeById(int id);
        public Task<int> CreatePraticeAsync(PraticeModel praticeModel);
        public Task<int> UpdatePraticeAsync(PraticeModel praticeModel);
        public Task<int> DeletePraticeAsync(PraticeModel praticeModel);
    }
}
