using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using CoreLayout.Repositories.UserManagement.Menu;
using CoreLayout.Repositories.UserManagement.ParentMenu;
using CoreLayout.Repositories.WRN;
using CoreLayout.Repositories.WRN.WRNCourseDetails;
using CoreLayout.Repositories.WRN.WRNQualification;
using CoreLayout.Repositories.WRN.WRNRegistration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.WRN.WRNCourseDetails
{
    public class WRNCourseDetailsService : IWRNCourseDetailsService
    {
        private readonly IWRNCourseDetailsRepository _wRNCourseDetailsRepository;

        public WRNCourseDetailsService(IWRNCourseDetailsRepository wRNCourseDetailsRepository)
        {
            _wRNCourseDetailsRepository = wRNCourseDetailsRepository;
        }

        public async Task<List<WRNCourseDetailsModel>> GetAllWRNCourseDetailsAsync()
        {
            return await _wRNCourseDetailsRepository.GetAllAsync();
        }

        public async Task<WRNCourseDetailsModel> GetWRNCourseDetailsByIdAsync(int id)
        {
            return await _wRNCourseDetailsRepository.GetByIdAsync(id);
        }
        public async Task<int> CreateWRNCourseDetailsAsync(WRNCourseDetailsModel wRNCourseDetailsModel)
        {
            return await _wRNCourseDetailsRepository.CreateAsync(wRNCourseDetailsModel);
        }

        public async Task<int> UpdateWRNCourseDetailsAsync(WRNCourseDetailsModel wRNCourseDetailsModel)
        {
            return await _wRNCourseDetailsRepository.UpdateAsync(wRNCourseDetailsModel);
        }

        public async Task<int> DeleteWRNCourseDetailsAsync(WRNCourseDetailsModel wRNCourseDetailsModel)
        {
            return await _wRNCourseDetailsRepository.DeleteAsync(wRNCourseDetailsModel);
        }
        public async Task<List<WRNCourseDetailsModel>> Check3CourseListAsync(string RegistrationNo)
        {
            return await _wRNCourseDetailsRepository.Check3CourseListAsync(RegistrationNo);
        }
        public async Task<WRNCourseDetailsModel> Check3CourseCountAsync(string RegistrationNo)
        {
            return await _wRNCourseDetailsRepository.Check3CourseCountAsync(RegistrationNo);
        }
        //public async Task<List<EducationalQualificationModel>> GetAllEducationalQualification()
        //{
        //    return await _wRNQualificationRepository.GetAllEducationalQualification();
        //}
        //public async Task<List<BoardUniversityModel>> GetAllBoardUniversityByType(string type)
        //{
        //    return await _wRNQualificationRepository.GetAllBoardUniversityByType(type);
        //}
        //public async Task<List<BoardUniversityModel>> GetAllBoardUniversityType()
        //{
        //    return await _wRNQualificationRepository.GetAllBoardUniversityType();
        //}
        //public async Task<List<WRNQualificationModel>> GetAllByIdForDetailsAsync(int id)
        //{
        //    return await _wRNQualificationRepository.GetAllByIdForDetailsAsync(id);
        //}
    }
}
