using CoreLayout.Models.Common;
using CoreLayout.Repositories.Common.OTPVerification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Common.OTPVerification
{
    public class OTPVerificationService : IOTPVerificationService
    {
        private readonly IOTPVerificationRepository _oTPVerificationRepository;

        public OTPVerificationService(IOTPVerificationRepository oTPVerificationRepository)
        {
            _oTPVerificationRepository = oTPVerificationRepository;
        }

        public async Task<List<OTPVerificationModel>> GetAllOTPVerificationAsync()
        {
            return await _oTPVerificationRepository.GetAllAsync();
        }

        public async Task<OTPVerificationModel> GetOTPVerificationByIdAsync(int id)
        {
            return await _oTPVerificationRepository.GetByIdAsync(id);
        }

        public async Task<OTPVerificationModel> GetOTPVerificationByMobileAsync(string mobileno)
        {
            return await _oTPVerificationRepository.GetOTPVerificationByMobileAsync(mobileno);
        }
        public async Task<OTPVerificationModel> GetOTPVerificationByMobileAndOTPAsync(string mobileno,string OTP)
        {
            return await _oTPVerificationRepository.GetOTPVerificationByMobileAndOTPAsync(mobileno, OTP);
        }
        public async Task<int> CreateOTPVerificationAsync(OTPVerificationModel oTPVerificationModel)
        {
            return await _oTPVerificationRepository.CreateAsync(oTPVerificationModel);
        }
        public async Task<int> UpdateOTPVerificationAsync(OTPVerificationModel oTPVerificationModel)
        {
            return await _oTPVerificationRepository.UpdateAsync(oTPVerificationModel);
        }
        public async Task<int> UpdateOTPVerificationByMobileAsync(OTPVerificationModel oTPVerificationModel)
        {
            return await _oTPVerificationRepository.UpdateOTPVerificationByMobileAsync(oTPVerificationModel);
        }
        public async Task<int> DeleteOTPVerificationAsync(OTPVerificationModel oTPVerificationModel)
        {
            return await _oTPVerificationRepository.DeleteAsync(oTPVerificationModel);
        }
    }
}
