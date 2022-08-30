using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class DashboardModel :MenuModel
    {

        public List<DashboardModel> level1 { get; set; }
        public List<DashboardModel> level2 { get; set; }
        public List<DashboardModel> level3 { get; set; }

        public List<DashboardModel> alllevels { get; set; }
    }
}
