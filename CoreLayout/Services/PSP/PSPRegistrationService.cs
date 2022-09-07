using CoreLayout.Models.PSP;
using CoreLayout.Repositories.PSP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.PSP
{
    public class PSPRegistrationService: IPSPRegistrationService
    {
        private readonly IPSPRegistrationRepository _pSPRegistrationRepository;
        public PSPRegistrationService(IPSPRegistrationRepository pSPRegistrationRepository)
        {
            _pSPRegistrationRepository = pSPRegistrationRepository;
        }

        public async Task<List<PSPRegistrationModel>> GetAllPSPRegistration()
        {
            return await _pSPRegistrationRepository.GetAllAsync();
        }

        public async Task<PSPRegistrationModel> GetPSPRegistrationById(int id)
        {
            return await _pSPRegistrationRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePSPRegistrationAsync(PSPRegistrationModel pSPRegistrationModel)
        {
            return await _pSPRegistrationRepository.CreateAsync(pSPRegistrationModel);
        }

        public async Task<int> UpdatePSPRegistrationAsync(PSPRegistrationModel pSPRegistrationModel)
        {
            return await _pSPRegistrationRepository.UpdateAsync(pSPRegistrationModel);
        }

        public async Task<int> DeletePSPRegistrationAsync(PSPRegistrationModel pSPRegistrationModel)
        {
            return await _pSPRegistrationRepository.DeleteAsync(pSPRegistrationModel);
        }
    }
}
