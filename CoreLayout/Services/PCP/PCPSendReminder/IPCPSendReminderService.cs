using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPSendReminder
{
   public  interface IPCPSendReminderService
    {
        public Task<List<PCPRegistrationModel>> GetAllAssingedQP();
        //public Task<PCPRegistrationModel> GetPCPApprovalById(int id);

        public Task<List<PCPRegistrationModel>> GetReminderById(int id);
        //public Task<int> CreatePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
        public Task<int> CreateReminderAsync(PCPRegistrationModel pCPRegistrationModel);
        //public Task<int> UpdatePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
        //public Task<int> DeletePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
    }
}
