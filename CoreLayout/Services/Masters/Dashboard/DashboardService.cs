using CoreLayout.Models;
using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<List<DashboardModel>> GetDashboardByRole(string Role)
        {
            return await _dashboardRepository.GetByRoleAsync(Role);
        }
        public async Task<List<DashboardModel>> GetDashboardByRoleAndUser(int roleid, int userid)
        {
            return await _dashboardRepository.GetByRoleAndUserAsync(roleid, userid);
        }
    }
}
