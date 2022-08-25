using CoreLayout.Models.Masters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Dashboard
{
    public interface IDashboardRepository
    {

        Task<List<DashboardModel>> GetByRoleAsync(string role);
        Task<List<DashboardModel>> GetByRoleAndUserAsync(int roleid, int userid);
    }
}
