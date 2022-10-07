using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPSendReminder
{
    public interface IPCPSendReminderRepository
    {
        Task<List<PCPSendPaperModel>> GetAllAssingedQP();

        //Task<List<PCPRegistrationModel>> GetAllAssingedQPButPaperNotUploaded();

        //Task<List<PCPRegistrationModel>> GetAllAssingedQPButPaperUploaded();
        Task<List<PCPRegistrationModel>> GetReminderById(int UserID,int AssignedQPId);

        Task<int> CreateReminderAsync(PCPRegistrationModel pCPRegistrationModel);
    }
}
