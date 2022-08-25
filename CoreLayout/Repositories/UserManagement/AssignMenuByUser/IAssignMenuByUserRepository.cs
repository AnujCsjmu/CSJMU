using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.AssignMenuByUser
{
    public interface IAssignMenuByUserRepository : IRepository<AssignMenuByUserModel>
    {
        Task<List<AssignMenuByUserModel>> CheckAlreadyAsync(int menuid, int userid);
    }
}
