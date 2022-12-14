using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPDetailsReport
{
    public interface IPCPDetailsReportService
    {
        public Task<List<PCPDetailsReportModel>> GetAllAsync();
    }
}
