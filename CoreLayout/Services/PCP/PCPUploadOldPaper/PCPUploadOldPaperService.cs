using CoreLayout.Models.PCP;
using CoreLayout.Repositories.PCP.PCPUploadOldPaper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPUploadOldPaper
{
    public class PCPUploadOldPaperService : IPCPUploadOldPaperService
    {
        private readonly IPCPUploadOldPaperRepository _pCPUploadOldPaperRepository;
        public PCPUploadOldPaperService(IPCPUploadOldPaperRepository pCPUploadOldPaperRepository)
        {
            _pCPUploadOldPaperRepository = pCPUploadOldPaperRepository;
        }

        public async Task<List<PCPUploadOldPaperModel>> GetAllPCPUploadOldPaper()
        {
            return await _pCPUploadOldPaperRepository.GetAllAsync();
        }

        public async Task<PCPUploadOldPaperModel> GetPCPUploadOldPaperById(int id)
        {
            return await _pCPUploadOldPaperRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePCPUploadOldPaperAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel)
        {
            return await _pCPUploadOldPaperRepository.CreateAsync(pCPUploadOldPaperModel);
        }

        public async Task<int> UpdatePCPUploadOldPaperAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel)
        {
            return await _pCPUploadOldPaperRepository.UpdateAsync(pCPUploadOldPaperModel);
        }

        public async Task<int> DeletePCPUploadOldPaperAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel)
        {
            return await _pCPUploadOldPaperRepository.DeleteAsync(pCPUploadOldPaperModel);
        }
        public async Task<int> FinalSubmitAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel)
        {
            return await _pCPUploadOldPaperRepository.FinalSubmitAsync(pCPUploadOldPaperModel);
        }
    }
}
