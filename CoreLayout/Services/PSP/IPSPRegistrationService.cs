using CoreLayout.Models.PSP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.PSP
{
    public interface IPSPRegistrationService
    {
        public Task<List<PSPRegistrationModel>> GetAllPSPRegistration();
        public Task<PSPRegistrationModel> GetPSPRegistrationById(int id);
        public Task<int> CreatePSPRegistrationAsync(PSPRegistrationModel pSPRegistrationModel);
        public Task<int> UpdatePSPRegistrationAsync(PSPRegistrationModel pSPRegistrationModel);
        public Task<int> DeletePSPRegistrationAsync(PSPRegistrationModel pSPRegistrationModel);
    }
}
