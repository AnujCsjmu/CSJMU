using CoreLayout.Models.PCP;
using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.PCP.PCPSendPaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPSendPaper
{
    public class PCPSendPaperService :IPCPSendPaperService
    {
        private readonly IPCPSendPaperRepository _pCPSendPaperRepository;
        public PCPSendPaperService(IPCPSendPaperRepository pCPSendPaperRepository)
        {
            _pCPSendPaperRepository = pCPSendPaperRepository;
        }

        public async Task<List<PCPSendPaperModel>> GetAllPCPSendPaper()
        {
            return await _pCPSendPaperRepository.GetAllAsync();
        }

        public async Task<PCPSendPaperModel> GetPCPSendPaperById(int id)
        {
            return await _pCPSendPaperRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePCPSendPaperAsync(PCPSendPaperModel pCPSendPaperModel)
        {
            return await _pCPSendPaperRepository.CreateAsync(pCPSendPaperModel);
        }

        public async Task<int> UpdatePCPSendPaperAsync(PCPSendPaperModel pCPSendPaperModel)
        {
            return await _pCPSendPaperRepository.UpdateAsync(pCPSendPaperModel);
        }

        public async Task<int> DeletePCPSendPaperAsync(PCPSendPaperModel pCPSendPaperModel)
        {
            return await _pCPSendPaperRepository.DeleteAsync(pCPSendPaperModel);
        }

        public async Task<List<RegistrationModel>> GetAllPCPUser_UploadPaperAsync()
        {
            return await _pCPSendPaperRepository.GetAllPCPUser_UploadPaperAsync();
        }

    }
}
