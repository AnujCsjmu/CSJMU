using CoreLayout.Models.PCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPUploadPaper
{
    public interface IPCPUploadPaperRepository : IRepository<PCPUploadPaperModel>
    {
        Task<List<PCPUploadPaperModel>> BothUserPaperUploadAndNotUpload();

        Task<int> InsertDownloadLogAsync(PCPUploadPaperModel entity);

        Task<int> FinalSubmitAsync(PCPUploadPaperModel pCPUploadPaperModel);

        Task<int> RequestQuestionPassword(PCPUploadPaperModel pCPUploadPaperModel);
        Task<int> RequestAnswerPassword(PCPUploadPaperModel pCPUploadPaperModel);
    }
}
