﻿using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPSendReminder
{
    public interface IPCPSendReminderRepository
    {
        Task<List<PCPRegistrationModel>> GetAllAssingedQP();
        Task<List<PCPRegistrationModel>> GetReminderById(int UserID);

        Task<int> CreateReminderAsync(PCPRegistrationModel pCPRegistrationModel);
    }
}