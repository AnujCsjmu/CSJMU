﻿using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPAssignedQP
{
    public interface IPCPAssignedQPRepository : IRepository<PCPAssignedQPModel>
    {
        Task<List<PCPAssignedQPModel>> GetAllQPByUserAsync(int PCPRegID);
    }
}
