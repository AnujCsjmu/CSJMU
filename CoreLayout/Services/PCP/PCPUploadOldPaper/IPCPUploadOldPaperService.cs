using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPUploadOldPaper
{
    public interface IPCPUploadOldPaperService
    {
        public Task<List<PCPUploadOldPaperModel>> GetAllPCPUploadOldPaper();
        public Task<PCPUploadOldPaperModel> GetPCPUploadOldPaperById(int id);
        public Task<int> CreatePCPUploadOldPaperAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel);
        public Task<int> UpdatePCPUploadOldPaperAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel);
        public Task<int> DeletePCPUploadOldPaperAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel);
        public Task<int> FinalSubmitAsync(PCPUploadOldPaperModel pCPUploadOldPaperModel);

    }
}
