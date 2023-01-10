using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using CoreLayout.Repositories.UserManagement.Menu;
using CoreLayout.Repositories.UserManagement.ParentMenu;
using CoreLayout.Repositories.WRN;
using CoreLayout.Repositories.WRN.WRNQualification;
using CoreLayout.Repositories.WRN.WRNRegistration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.WRN.WRNQualification
{
    public class WRNQualificationService : IWRNQualificationService
    {
        private readonly IWRNQualificationRepository _wRNQualificationRepository;

        public WRNQualificationService(IWRNQualificationRepository wRNQualificationRepository)
        {
            _wRNQualificationRepository = wRNQualificationRepository;
        }

        public async Task<List<WRNQualificationModel>> GetAllWRNQualificationAsync()
        {
            return await _wRNQualificationRepository.GetAllAsync();
        }
        public async Task<List<WRNQualificationModel>> GetAllWRNQualificationByRegistration(string RegistrationNo)
        {
            return await _wRNQualificationRepository.GetAllWRNQualificationByRegistration(RegistrationNo);
        }
        public async Task<WRNQualificationModel> GetWRNQualificationByIdAsync(int id)
        {
            return await _wRNQualificationRepository.GetByIdAsync(id);
        }
        public async Task<int> CreateWRNQualificationAsync(WRNQualificationModel wRNQualificationModel)
        {
            return await _wRNQualificationRepository.CreateAsync(wRNQualificationModel);
        }

        public async Task<int> UpdateWRNQualificationAsync(WRNQualificationModel wRNQualificationModel)
        {
            return await _wRNQualificationRepository.UpdateAsync(wRNQualificationModel);
        }

        public async Task<int> DeleteWRNQualificationAsync(WRNQualificationModel wRNQualificationModel)
        {
            return await _wRNQualificationRepository.DeleteAsync(wRNQualificationModel);
        }

        public async Task<List<EducationalQualificationModel>> GetAllEducationalQualification()
        {
            return await _wRNQualificationRepository.GetAllEducationalQualification();
        }
        public async Task<List<BoardUniversityModel>> GetAllBoardUniversityByType(string type)
        {
            return await _wRNQualificationRepository.GetAllBoardUniversityByType(type);
        }
        public async Task<List<BoardUniversityModel>> GetAllBoardUniversityType()
        {
            return await _wRNQualificationRepository.GetAllBoardUniversityType();
        }
        public async Task<List<WRNQualificationModel>> GetAllByIdForDetailsAsync(int id)
        {
            return await _wRNQualificationRepository.GetAllByIdForDetailsAsync(id);
        }
    }
}
