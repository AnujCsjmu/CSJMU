using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.Login
{
    public interface ILoginService
    {
        public Task<RegistrationModel> GetUserDetails(String loginid, String password);
        public Task<RegistrationModel> GetUserDetailByLoginId(String loginid);
        //public Task<RegistrationModel> GetUserDetails(string Salthash);

        public Task<int> InsertLoginInformation(int registrationId, string LoginStatus);

        public Task<int> InsertLogoutInformation(int registrationId, string LoginStatus);

        public Task<ButtonModel> GetButtonsDetails();
    }
}
