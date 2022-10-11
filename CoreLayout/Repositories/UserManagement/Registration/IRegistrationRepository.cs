using CoreLayout.Models;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.UserManagement.Registration
{
    public interface IRegistrationRepository : IRepository<RegistrationModel>
    {
        Task<List<RegistrationModel>> GetAllInstituteAsync();

        Task<int> ChangePassword(RegistrationModel entity);

        Task<int> InsertEmailSMSHistory(RegistrationModel entity);

        Task<RegistrationModel> ForgetPassword(string emailid, string mobileno, string loginid);
    }
}
