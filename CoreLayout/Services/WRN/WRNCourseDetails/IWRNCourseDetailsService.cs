using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.WRN.WRNCourseDetails
{
    public interface IWRNCourseDetailsService
    {
        public Task<List<WRNCourseDetailsModel>> GetAllWRNCourseDetailsAsync();
        public Task<WRNCourseDetailsModel> GetWRNCourseDetailsByIdAsync(int id);
        public Task<int> CreateWRNCourseDetailsAsync(WRNCourseDetailsModel wRNCourseDetailsModel);
        public Task<int> UpdateWRNCourseDetailsAsync(WRNCourseDetailsModel wRNCourseDetailsModel);
        public Task<int> DeleteWRNCourseDetailsAsync(WRNCourseDetailsModel wRNCourseDetailsModel);

        //public Task<List<EducationalQualificationModel>> GetAllEducationalQualification();
        //public Task<List<BoardUniversityModel>> GetAllBoardUniversityByType(string type);
        //public Task<List<BoardUniversityModel>> GetAllBoardUniversityType();

        //public Task<List<WRNQualificationModel>> GetAllByIdForDetailsAsync(int id);
    }
}
