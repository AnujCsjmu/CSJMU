using CoreLayout.Models.PCP;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.PCP.PCPSendPaper
{
    public interface IPCPSendPaperService
    {
        public Task<List<PCPSendPaperModel>> GetAllPCPSendPaper();
        public Task<PCPSendPaperModel> GetPCPSendPaperById(int id);
        public Task<int> CreatePCPSendPaperAsync(PCPSendPaperModel pCPSendPaperModel);
        public Task<int> UpdatePCPSendPaperAsync(PCPSendPaperModel pCPSendPaperModel);
        public Task<int> DeletePCPSendPaperAsync(PCPSendPaperModel pCPSendPaperModel);

        public Task<List<RegistrationModel>> GetAllPCPUser_UploadPaperAsync();
    }
}
