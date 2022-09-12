using CoreLayout.Models.PCP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.PCP.PCPApproval
{
    public interface IPCPApprovalRepository : IRepository<PCPRegistrationModel>
    {
        Task<List<PCPRegistrationModel>> GetReminderById(int UserID);

        Task<int> CreateReminderAsync(PCPRegistrationModel pCPRegistrationModel);
    }
}
