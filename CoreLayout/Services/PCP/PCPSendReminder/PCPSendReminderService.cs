using CoreLayout.Models.PCP;
using CoreLayout.Repositories.PCP.PCPSendReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPSendReminder
{
    public class PCPSendReminderService : IPCPSendReminderService
    {
        private readonly IPCPSendReminderRepository _pCPSendReminderRepository;
        public PCPSendReminderService(IPCPSendReminderRepository pCPSendReminderRepository)
        {
            _pCPSendReminderRepository = pCPSendReminderRepository;
        }

        public async Task<List<PCPSendPaperModel>> GetAllAssingedQP()
        {
            return await _pCPSendReminderRepository.GetAllAssingedQP();
        }
        //public async Task<List<PCPRegistrationModel>> GetAllAssingedQPButPaperNotUploaded()
        //{
        //    return await _pCPSendReminderRepository.GetAllAssingedQPButPaperNotUploaded();
        //}
        //public async Task<List<PCPRegistrationModel>> GetAllAssingedQPButPaperUploaded()
        //{
        //    return await _pCPSendReminderRepository.GetAllAssingedQPButPaperUploaded();
        //}
        public async Task<List<PCPRegistrationModel>> GetReminderById(int id,int QPId)
        {
            return await _pCPSendReminderRepository.GetReminderById(id, QPId);
        }
        public async Task<int> CreateReminderAsync(PCPRegistrationModel pCPRegistrationModel)
        {
            return await _pCPSendReminderRepository.CreateReminderAsync(pCPRegistrationModel);
        }
    }
}
