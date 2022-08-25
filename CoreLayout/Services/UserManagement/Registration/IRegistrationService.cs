using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Registration
{
    public interface IRegistrationService
    {
        public Task<List<RegistrationModel>> GetAllRegistrationAsync();
        public Task<RegistrationModel> GetRegistrationByIdAsync(int id);
        public Task<int> CreateRegistrationAsync(RegistrationModel registrationModel);
        public Task<int> UpdateRegistrationAsync(RegistrationModel roleModel);
        public Task<int> DeleteRegistrationAsync(RegistrationModel roleModel);

        public Task<List<RegistrationModel>> GetAllInstituteAsync();
    }
}
