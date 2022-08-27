using CoreLayout.Models;
using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;

        public CommonService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }
      
        public async Task<List<DashboardModel>> GetDashboardByRoleAndUser(int roleid, int userid)
        {
            return await _commonRepository.GetByRoleAndUserAsync(roleid, userid);
        }
        public async Task<List<ButtonPermissionModel>> GetSingleButtonByRoleAndUser(int roleid, int userid, int buttonId)
        {
            return await _commonRepository.GetSingleButtonByRoleAndUserAsync(roleid, userid, buttonId);
        }
        public async Task<List<ButtonPermissionModel>> GetMultiButtonByRoleAndUser(int roleid, int userid)
        {
            return await _commonRepository.GetMultipleButtonByRoleAndUserAsync(roleid, userid);
        }

        public async Task<int> CreateSMSLogs(SMSModel entity)
        {
            return await _commonRepository.CreateSMSLogs(entity);
        }
    }
}
