using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CoreLayout.Repositories.PCP.PCPDetailsReport
{
    public class PCPDetailsReportService : IPCPDetailsReportService
    {
        private readonly IPCPDetailsReportRepository _pCPDetailsReportRepository;

        public PCPDetailsReportService(IPCPDetailsReportRepository pCPDetailsReportRepository)
        {
            _pCPDetailsReportRepository = pCPDetailsReportRepository;
        }
        public async Task<List<PCPDetailsReportModel>> GetAllAsync()
        {
            return await _pCPDetailsReportRepository.GetAllAsync();
        }
    }
}
