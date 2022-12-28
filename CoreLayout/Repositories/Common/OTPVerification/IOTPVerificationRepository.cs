using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Common.OTPVerification
{
    public interface IOTPVerificationRepository : IRepository<OTPVerificationModel>
    {
       Task<OTPVerificationModel> GetOTPVerificationByMobileAsync(string mobileno);
        Task<OTPVerificationModel> GetOTPVerificationByMobileAndOTPAsync(string mobileno,string OTP);
        Task<int> UpdateOTPVerificationByMobileAsync(OTPVerificationModel entity);
    }
}
