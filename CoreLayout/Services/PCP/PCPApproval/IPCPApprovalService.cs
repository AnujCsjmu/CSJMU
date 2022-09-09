using CoreLayout.Models.PCP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPApproval
{
    public interface IPCPApprovalService
    {
        public Task<List<PCPRegistrationModel>> GetAllPCPApproval();
        public Task<PCPRegistrationModel> GetPCPApprovalById(int id);
        public Task<int> CreatePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
        public Task<int> UpdatePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
        public Task<int> DeletePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
    }
}
