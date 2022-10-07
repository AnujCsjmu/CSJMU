using CoreLayout.Models.PCP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPRegistration
{
    public interface IPCPRegistrationService
    {
        public Task<List<PCPRegistrationModel>> GetAllPCPRegistration();
        public Task<PCPRegistrationModel> GetPCPRegistrationById(int id);
        public Task<int> CreatePCPRegistrationAsync(PCPRegistrationModel pCPRegistrationModel);
        public Task<int> UpdatePCPRegistrationAsync(PCPRegistrationModel pCPRegistrationModel);
        public Task<int> DeletePCPRegistrationAsync(PCPRegistrationModel pCPRegistrationModel);

        public Task<List<PCPRegistrationModel>> GetReportQPAndPaperWise();
        public Task<List<PCPRegistrationModel>> GetSetterList();
    }
}
