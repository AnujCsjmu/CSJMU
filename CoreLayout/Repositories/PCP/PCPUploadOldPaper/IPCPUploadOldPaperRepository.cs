using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPUploadOldPaper
{
    public interface IPCPUploadOldPaperRepository : IRepository<PCPUploadOldPaperModel>
    {
        Task<int> FinalSubmitAsync(PCPUploadOldPaperModel pCPUploadPaperModel);
    }
}
