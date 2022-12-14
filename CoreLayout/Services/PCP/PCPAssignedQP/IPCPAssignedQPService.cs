using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPAssignedQP
{
    public interface IPCPAssignedQPService
    {
        public Task<List<PCPAssignedQPModel>> GetAllPCPAssignedQP();
        public Task<PCPAssignedQPModel> GetPCPAssignedQPById(int id);
        public Task<int> CreatePCPAssignedQPAsync(PCPAssignedQPModel pCPAssignedQPModel);
        public Task<int> UpdatePCPAssignedQPAsync(PCPAssignedQPModel pCPAssignedQPModel);
        public Task<int> DeletePCPAssignedQPAsync(PCPAssignedQPModel pCPAssignedQPModel);

        public Task<PCPAssignedQPModel> alreadyAssignedQP(int userid, int QPId);

        //public Task<List<PCPAssignedQPModel>> GetAllUserByQPIdAsync(int qpid);

        //public Task<List<PCPAssignedQPModel>> GetAllQPByUserIdAsync(int Userid);
    }
}
