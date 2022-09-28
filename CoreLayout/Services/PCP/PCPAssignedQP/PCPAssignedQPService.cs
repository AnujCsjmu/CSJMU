using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using CoreLayout.Repositories.PCP.PCPAssignedQP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPAssignedQP
{
    public class PCPAssignedQPService : IPCPAssignedQPService
    {
        private readonly IPCPAssignedQPRepository _pCPAssignedQPRepository;
        public PCPAssignedQPService(IPCPAssignedQPRepository pCPAssignedQPRepository)
        {
            _pCPAssignedQPRepository = pCPAssignedQPRepository;
        }

        public async Task<List<PCPAssignedQPModel>> GetAllPCPAssignedQP()
        {
            return await _pCPAssignedQPRepository.GetAllAsync();
        }

        public async Task<PCPAssignedQPModel> GetPCPAssignedQPById(int id)
        {
            return await _pCPAssignedQPRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePCPAssignedQPAsync(PCPAssignedQPModel pCPAssignedQPModel)
        {
            return await _pCPAssignedQPRepository.CreateAsync(pCPAssignedQPModel);
        }

        public async Task<int> UpdatePCPAssignedQPAsync(PCPAssignedQPModel pCPAssignedQPModel)
        {
            return await _pCPAssignedQPRepository.UpdateAsync(pCPAssignedQPModel);
        }

        public async Task<int> DeletePCPAssignedQPAsync(PCPAssignedQPModel pCPAssignedQPModel)
        {
            return await _pCPAssignedQPRepository.DeleteAsync(pCPAssignedQPModel);
        }
        public async Task<PCPAssignedQPModel> alreadyAssignedQP(int userid, int QPId)
        {
            return await _pCPAssignedQPRepository.alreadyAssignedQP(userid, QPId);
        }
        //public async Task<List<PCPAssignedQPModel>> GetAllUserByQPIdAsync(int qpid)
        //{
        //    return await _pCPAssignedQPRepository.GetAllUserByQPIdAsync(qpid);
        //}

        //public async Task<List<PCPAssignedQPModel>> GetAllQPByUserIdAsync(int Userid)
        //{
        //    return await _pCPAssignedQPRepository.GetAllQPByUserIdAsync(Userid);
        //}
    }
}
