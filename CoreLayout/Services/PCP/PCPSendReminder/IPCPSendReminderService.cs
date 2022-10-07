using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPSendReminder
{
   public  interface IPCPSendReminderService
    {
        public Task<List<PCPSendPaperModel>> GetAllAssingedQP();

        //public Task<List<PCPRegistrationModel>> GetAllAssingedQPButPaperNotUploaded();

        //public Task<List<PCPRegistrationModel>> GetAllAssingedQPButPaperUploaded();
        //public Task<PCPRegistrationModel> GetPCPApprovalById(int id);

        public Task<List<PCPRegistrationModel>> GetReminderById(int id,int AssignedQPId);
        //public Task<int> CreatePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
        public Task<int> CreateReminderAsync(PCPRegistrationModel pCPRegistrationModel);
        //public Task<int> UpdatePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
        //public Task<int> DeletePCPApprovalAsync(PCPRegistrationModel pCPRegistrationModel);
    }
}
