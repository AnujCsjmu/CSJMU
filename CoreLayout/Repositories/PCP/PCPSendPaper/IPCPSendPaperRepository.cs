using CoreLayout.Models.PCP;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPSendPaper
{
    public interface IPCPSendPaperRepository : IRepository<PCPSendPaperModel>
    {
        Task<List<RegistrationModel>> GetAllPCPUser_UploadPaperAsync();

        Task<PCPSendPaperModel> GetServerDateTime();
    }
}
