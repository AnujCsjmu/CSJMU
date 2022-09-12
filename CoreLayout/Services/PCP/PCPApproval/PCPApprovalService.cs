using CoreLayout.Models.PCP;
using CoreLayout.Repositories.PCP.PCPApproval;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPApproval
{
    public class PCPApprovalService : IPCPApprovalService
    {
        private readonly IPCPApprovalRepository _pCPApprovalRepository;
        public PCPApprovalService(IPCPApprovalRepository pCPApprovalRepository)
        {
            _pCPApprovalRepository = pCPApprovalRepository;
        }

        public async Task<List<PCPRegistrationModel>> GetAllPCPApproval()
        {
            return await _pCPApprovalRepository.GetAllAsync();
        }

        public async Task<PCPRegistrationModel> GetPCPApprovalById(int id)
        {
            return await _pCPApprovalRepository.GetByIdAsync(id);
        }

        public async Task<List<PCPRegistrationModel>> GetReminderById(int id)
        {
            return await _pCPApprovalRepository.GetReminderById(id);
        }

        public async Task<int> CreatePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel)
        {
            return await _pCPApprovalRepository.CreateAsync(pCPRegistrationModel);
        }

        public async Task<int> CreateReminderAsync(PCPRegistrationModel pCPRegistrationModel)
        {
            return await _pCPApprovalRepository.CreateReminderAsync(pCPRegistrationModel);
        }

        public async Task<int> UpdatePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel)
        {
            return await _pCPApprovalRepository.UpdateAsync(pCPRegistrationModel);
        }

        public async Task<int> DeletePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel)
        {
            return await _pCPApprovalRepository.DeleteAsync(pCPRegistrationModel);
        }
    }
}
