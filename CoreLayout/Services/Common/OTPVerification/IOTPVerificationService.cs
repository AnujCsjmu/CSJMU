using CoreLayout.Models.Common;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Common.OTPVerification
{
    public interface IOTPVerificationService
    {
        public Task<List<OTPVerificationModel>> GetAllOTPVerificationAsync();
        public Task<OTPVerificationModel> GetOTPVerificationByIdAsync(int id);
        public Task<OTPVerificationModel> GetOTPVerificationByMobileAsync(string mobileno);
        public Task<OTPVerificationModel> GetOTPVerificationByMobileAndOTPAsync(string mobileno,string OTP);
        public Task<int> CreateOTPVerificationAsync(OTPVerificationModel oTPVerificationModel);
        public Task<int> UpdateOTPVerificationAsync(OTPVerificationModel oTPVerificationModel);
        public Task<int> UpdateOTPVerificationByMobileAsync(OTPVerificationModel oTPVerificationModel);
        public Task<int> DeleteOTPVerificationAsync(OTPVerificationModel oTPVerificationModel);
    }
}
