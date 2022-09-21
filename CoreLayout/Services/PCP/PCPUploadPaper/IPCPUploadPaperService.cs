using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPUploadPaper
{
    public interface IPCPUploadPaperService
    {
        public Task<List<PCPUploadPaperModel>> GetAllPCPUploadPaper();
        public Task<PCPUploadPaperModel> GetPCPUploadPaperById(int id);
        public Task<int> CreatePCPUploadPaperAsync(PCPUploadPaperModel pCPUploadPaperModel);
        public Task<int> UpdatePCPUploadPaperAsync(PCPUploadPaperModel pCPUploadPaperModel);
        public Task<int> DeletePCPUploadPaperAsync(PCPUploadPaperModel pCPUploadPaperModel);

        public Task<List<PCPUploadPaperModel>> BothUserPaperUploadAndNotUpload();
    }
}
