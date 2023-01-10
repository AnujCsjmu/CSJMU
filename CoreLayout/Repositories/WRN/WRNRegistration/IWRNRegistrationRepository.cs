using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.WRN.WRNRegistration
{
    public interface IWRNRegistrationRepository : IRepository<WRNRegistrationModel>
    {
        Task<WRNRegistrationModel> GetWRNRegistrationByLoginAsync(string RegistrationNo, string MobileNo, string DOB);
        Task<WRNRegistrationModel> GetWRNRegistrationByMobileAsync(string MobileNo);
        Task<int> UpdateFinalSubmitAsync(WRNRegistrationModel entity);
        Task<int> UpdatePhotoSignatureAsync(WRNRegistrationModel entity);
        Task<int> UpdatePrintRegistration(WRNRegistrationModel entity);

        Task<List<WRNRegistrationModel>> GetPhotoUploadByRegistrationAsync(string RegistrationNo, string MobileNo, string DOB);
        Task<int> DeleteUploadPhotoSignatureAsync(WRNRegistrationModel wRNRegistrationModel);
    }
}
