using CoreLayout.Models.Masters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Dashboard
{
    public interface IDashboardService
    {
        public Task<List<DashboardModel>> GetDashboardByRole(string Role);

        public Task<List<DashboardModel>> GetDashboardByRoleAndUser(int roleid,int userid);
    }
}
