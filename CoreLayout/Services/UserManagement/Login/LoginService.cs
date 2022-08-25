using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.Login;
using System.Threading.Tasks;

namespace CoreLayout.Services.UserManagement.Login
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<RegistrationModel> GetUserDetails(string loginid, string password)
        {
            return await _loginRepository.GetUserDetail(loginid, password);

        }
        public async Task<RegistrationModel> GetUserDetailByLoginId(string loginid)
        {
            return await _loginRepository.GetUserDetailByLoginId(loginid);

        }
        //public async Task<RegistrationModel> GetUserDetails(string Salthash)
        //{
        //    return await _loginRepository.GetUserDetail(Salthash);

        //}

        public async Task<int> InsertLoginInformation(int registrationId, string LoginStatus)
        {
            return await _loginRepository.CreateAsync(registrationId, LoginStatus);

        }

        public async Task<int> InsertLogoutInformation(int registrationId, string LoginStatus)
        {
            return await _loginRepository.UpdateAsync(registrationId, LoginStatus);

        }

        public async Task<ButtonModel> GetButtonsDetails()
        {
            return await _loginRepository.GetButtonDetail();

        }
    }
}
