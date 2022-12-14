using CoreLayout.Models.PCP;
using CoreLayout.Repositories.PCP.PCPRegistration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPRegistration
{
    public class PCPRegistrationService: IPCPRegistrationService
    {
        private readonly IPCPRegistrationRepository _pCPRegistrationRepository;
        public PCPRegistrationService(IPCPRegistrationRepository pCPRegistrationRepository)
        {
            _pCPRegistrationRepository = pCPRegistrationRepository;
        }

        public async Task<List<PCPRegistrationModel>> GetAllPCPRegistration()
        {
            return await _pCPRegistrationRepository.GetAllAsync();
        }

        public async Task<PCPRegistrationModel> GetPCPRegistrationById(int id)
        {
            return await _pCPRegistrationRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePCPRegistrationAsync(PCPRegistrationModel pCPRegistrationModel)
        {
            return await _pCPRegistrationRepository.CreateAsync(pCPRegistrationModel);
        }

        public async Task<int> UpdatePCPRegistrationAsync(PCPRegistrationModel pCPRegistrationModel)
        {
            return await _pCPRegistrationRepository.UpdateAsync(pCPRegistrationModel);
        }

        public async Task<int> DeletePCPRegistrationAsync(PCPRegistrationModel pCPRegistrationModel)
        {
            return await _pCPRegistrationRepository.DeleteAsync(pCPRegistrationModel);
        }

        public async Task<List<PCPRegistrationModel>> GetReportQPAndPaperWise()
        {
            return await _pCPRegistrationRepository.GetReportQPAndPaperWise();
        }

        public async Task<List<PCPRegistrationModel>> GetSetterList()
        {
            return await _pCPRegistrationRepository.GetSetterList();
        }
        public async Task<PCPRegistrationModel> ForSendReminderGetUseByIdAsync(int AssignedQPId)
        {
            return await _pCPRegistrationRepository.ForSendReminderGetUseByIdAsync(AssignedQPId);
        }
    }
}
