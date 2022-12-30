using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.WRN
{
    public interface IWRNRegistrationRepository : IRepository<WRNRegistrationModel>
    {
        Task<WRNRegistrationModel> GetWRNRegistrationByLoginAsync(string RegistrationNo, string MobileNo, string DOB);
        Task<WRNRegistrationModel> GetWRNRegistrationByMobileAsync(string MobileNo);
        Task<int> UpdateFinalSubmitAsync(WRNRegistrationModel entity);
    }
}
