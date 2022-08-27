using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Common
{
    public interface ICommonRepository
    {
        Task<List<DashboardModel>> GetByRoleAndUserAsync(int roleid, int userid);

        Task<List<ButtonPermissionModel>> GetSingleButtonByRoleAndUserAsync(int roleid, int userid, int buttonId);

        Task<List<ButtonPermissionModel>> GetMultipleButtonByRoleAndUserAsync(int roleid, int userid);

        Task<int> CreateSMSLogs(SMSModel sMSModel);
    }
}
