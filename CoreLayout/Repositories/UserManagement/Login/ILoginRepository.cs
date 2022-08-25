using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.Login
{
    public interface ILoginRepository
    {
        Task<RegistrationModel> GetUserDetail(string loginid, string password);
        Task<RegistrationModel> GetUserDetailByLoginId(string loginid);
        //Task<RegistrationModel> GetUserDetail(string Salthash);

        Task<int> CreateAsync(int registrationId, string LoginStatus);

        Task<int> UpdateAsync(int registrationId, string LoginStatus);
        Task<ButtonModel> GetButtonDetail();
    }
}
