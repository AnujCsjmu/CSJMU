﻿using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using CoreLayout.Repositories.UserManagement.Menu;
using CoreLayout.Repositories.UserManagement.ParentMenu;
using CoreLayout.Repositories.WRN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.WRN
{
    public class WRNRegistrationService : IWRNRegistrationService
    {
        private readonly IWRNRegistrationRepository _wRNRegistrationRepository;

        public WRNRegistrationService(IWRNRegistrationRepository wRNRegistrationRepository)
        {
            _wRNRegistrationRepository = wRNRegistrationRepository;
        }

        public async Task<List<WRNRegistrationModel>> GetAllWRNRegistrationAsync()
        {
            return await _wRNRegistrationRepository.GetAllAsync();
        }

        public async Task<WRNRegistrationModel> GetWRNRegistrationByIdAsync(int id)
        {
            return await _wRNRegistrationRepository.GetByIdAsync(id);
        }

        public async Task<WRNRegistrationModel> GetWRNRegistrationByLoginAsync(string RegistrationNo, string MobileNo, string DOB)
        {
            return await _wRNRegistrationRepository.GetWRNRegistrationByLoginAsync(RegistrationNo, MobileNo, DOB);
        }
        public async Task<WRNRegistrationModel> GetWRNRegistrationByMobileAsync(string MobileNo)
        {
            return await _wRNRegistrationRepository.GetWRNRegistrationByMobileAsync(MobileNo);
        }
        public async Task<int> CreateWRNRegistrationAsync(WRNRegistrationModel wRNRegistrationModel)
        {
            return await _wRNRegistrationRepository.CreateAsync(wRNRegistrationModel);
        }

        public async Task<int> UpdateWRNRegistrationAsync(WRNRegistrationModel wRNRegistrationModel)
        {
            return await _wRNRegistrationRepository.UpdateAsync(wRNRegistrationModel);
        }

        public async Task<int> DeleteWRNRegistrationAsync(WRNRegistrationModel wRNRegistrationModel)
        {
            return await _wRNRegistrationRepository.DeleteAsync(wRNRegistrationModel);
        }
    }
}
