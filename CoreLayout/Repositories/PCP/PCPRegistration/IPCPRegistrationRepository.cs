using CoreLayout.Models.PCP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPRegistration
{
    public interface IPCPRegistrationRepository : IRepository<PCPRegistrationModel>
    {
        Task<List<PCPRegistrationModel>> GetReportQPAndPaperWise();
    }
}
