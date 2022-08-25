using CoreLayout.Models.Masters;
using CoreLayout.Repositories.Masters.Pratice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Pratice
{
    public class PraticeService : IPraticeService
    {
        private readonly IPraticeRepository _praticeRepository;

        public PraticeService(IPraticeRepository praticeRepository)
        {
            _praticeRepository = praticeRepository;
        }

        public async Task<List<PraticeModel>> GetAllPratice()
        {
            return await _praticeRepository.GetAllAsync();
        }
        public async Task<List<PraticeChildModel>> GetAllChildAsync()
        {
            return await _praticeRepository.GetAllChildAsync();
        }

        public async Task<PraticeModel> GetPraticeById(int id)
        {
            return await _praticeRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePraticeAsync(PraticeModel praticeModel)
        {
            return await _praticeRepository.CreateAsync(praticeModel);
        }

        public async Task<int> UpdatePraticeAsync(PraticeModel praticeModel)
        {
            return await _praticeRepository.UpdateAsync(praticeModel);
        }

        public async Task<int> DeletePraticeAsync(PraticeModel praticeModel)
        {
            return await _praticeRepository.DeleteAsync(praticeModel);
        }
    }
}
