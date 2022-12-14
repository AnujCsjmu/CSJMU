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

        public async Task<List<PCPUploadPaperModel>> BothUserPaperUploadAndNotUpload()
        {
            return await _pCPUploadPaperRepository.BothUserPaperUploadAndNotUpload();
        }

        public async Task<int> InsertDownloadLogAsync(PCPUploadPaperModel pCPUploadPaperModel)
        {
            return await _pCPUploadPaperRepository.InsertDownloadLogAsync(pCPUploadPaperModel);
        }

        public async Task<int> FinalSubmitAsync(PCPUploadPaperModel pCPUploadPaperModel)
        {
            return await _pCPUploadPaperRepository.FinalSubmitAsync(pCPUploadPaperModel);
        }

        public async Task<int> RequestQuestionPassword(PCPUploadPaperModel pCPUploadPaperModel)
        {
            return await _pCPUploadPaperRepository.RequestQuestionPassword(pCPUploadPaperModel);
        }

        public async Task<int> RequestAnswerPassword(PCPUploadPaperModel pCPUploadPaperModel)
        {
            return await _pCPUploadPaperRepository.RequestAnswerPassword(pCPUploadPaperModel);
        }
    }
}
