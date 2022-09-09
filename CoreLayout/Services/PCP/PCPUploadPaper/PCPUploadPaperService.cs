using CoreLayout.Models.PCP;
using CoreLayout.Repositories.PCP.PCPUploadPaper;
using CoreLayout.Services.PCP.PCPUploadPaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPUploadPaper
{
    public class PCPUploadPaperService : IPCPUploadPaperService
    {
        private readonly IPCPUploadPaperRepository _pCPUploadPaperRepository;
        public PCPUploadPaperService(IPCPUploadPaperRepository pCPUploadPaperRepository)
        {
            _pCPUploadPaperRepository = pCPUploadPaperRepository;
        }

        public async Task<List<PCPUploadPaperModel>> GetAllPCPUploadPaper()
        {
            return await _pCPUploadPaperRepository.GetAllAsync();
        }

        public async Task<PCPUploadPaperModel> GetPCPUploadPaperById(int id)
        {
            return await _pCPUploadPaperRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePCPUploadPaperAsync(PCPUploadPaperModel pCPUploadPaperModel)
        {
            return await _pCPUploadPaperRepository.CreateAsync(pCPUploadPaperModel);
        }

        public async Task<int> UpdatePCPUploadPaperAsync(PCPUploadPaperModel pCPUploadPaperModel)
        {
            return await _pCPUploadPaperRepository.UpdateAsync(pCPUploadPaperModel);
        }

        public async Task<int> DeletePCPUploadPaperAsync(PCPUploadPaperModel pCPUploadPaperModel)
        {
            return await _pCPUploadPaperRepository.DeleteAsync(pCPUploadPaperModel);
        }
    }
}
