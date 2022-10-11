using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.UserManagement.Registration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }
        public async Task<List<RegistrationModel>> GetAllRegistrationAsync()
        {
            return await _registrationRepository.GetAllAsync();
        }

        public async Task<RegistrationModel> GetRegistrationByIdAsync(int id)
        {
            return await _registrationRepository.GetByIdAsync(id);
        }
        public async Task<int> CreateRegistrationAsync(RegistrationModel registrationModel)
        {
            return await _registrationRepository.CreateAsync(registrationModel);
        }

        public async Task<int> UpdateRegistrationAsync(RegistrationModel registrationModel)
        {
            return await _registrationRepository.UpdateAsync(registrationModel);
        }

        public async Task<int> DeleteRegistrationAsync(RegistrationModel registrationModel)
        {
            return await _registrationRepository.DeleteAsync(registrationModel);
        }

        public async Task<List<RegistrationModel>> GetAllInstituteAsync()
        {
            return await _registrationRepository.GetAllInstituteAsync();
        }

        public async Task<int> ChangePassword(RegistrationModel registrationModel)
        {
            return await _registrationRepository.ChangePassword(registrationModel);
        }
        public async Task<int> InsertEmailSMSHistory(RegistrationModel registrationModel)
        {
            return await _registrationRepository.InsertEmailSMSHistory(registrationModel);
        }

        public async Task<RegistrationModel> ForgetPassword(string emailid, string mobileno, string loginid)
        {
            return await _registrationRepository.ForgetPassword(emailid, mobileno, loginid);
        }
    }
}
