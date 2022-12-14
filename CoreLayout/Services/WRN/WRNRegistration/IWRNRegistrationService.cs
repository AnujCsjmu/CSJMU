using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.WRN.WRNRegistration
{
    public interface IWRNRegistrationService
    {
        public Task<List<WRNRegistrationModel>> GetAllWRNRegistrationAsync();

        public Task<List<WRNRegistrationModel>> GetPhotoUploadByRegistrationAsync(string RegistrationNo, string MobileNo, string DOB);
        public Task<WRNRegistrationModel> GetWRNRegistrationByIdAsync(int id);
        public Task<WRNRegistrationModel> GetWRNRegistrationByLoginAsync(string RegistrationNo, string MobileNo,string DOB);
        public Task<WRNRegistrationModel> GetWRNRegistrationByMobileAsync(string MobileNo);
        public Task<int> CreateWRNRegistrationAsync(WRNRegistrationModel wRNRegistrationModel);
        public Task<int> UpdateWRNRegistrationAsync(WRNRegistrationModel wRNRegistrationModel);
        public Task<int> DeleteWRNRegistrationAsync(WRNRegistrationModel wRNRegistrationModel);

        public Task<int> DeleteUploadPhotoSignatureAsync(WRNRegistrationModel wRNRegistrationModel);
        public Task<int> UpdateFinalSubmitAsync(WRNRegistrationModel wRNRegistrationModel);
        public Task<int> UpdatePhotoSignatureAsync(WRNRegistrationModel wRNRegistrationModel);
        public Task<int> UpdatePrintRegistration(WRNRegistrationModel wRNRegistrationModel);
    }
}
