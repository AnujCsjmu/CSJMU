using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPAssignedQP
{
    public interface IPCPAssignedQPRepository : IRepository<PCPAssignedQPModel>
    {
        Task<List<PCPAssignedQPModel>> GetAllQPByPCPRegIdAsync(int PCPRegID);

        Task<List<PCPAssignedQPModel>> GetAllQPByUserIdAsync(int Userid);
    }
}
