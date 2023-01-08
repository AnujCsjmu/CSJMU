using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.WRN.WRNQualification
{
    public interface IWRNQualificationService
    {
        public Task<List<WRNQualificationModel>> GetAllWRNQualificationAsync();
        public Task<WRNQualificationModel> GetWRNQualificationByIdAsync(int id);
        public Task<int> CreateWRNQualificationAsync(WRNQualificationModel wRNQualificationModel);
        public Task<int> UpdateWRNQualificationAsync(WRNQualificationModel wRNQualificationModel);
        public Task<int> DeleteWRNQualificationAsync(WRNQualificationModel wRNQualificationModel);

        public Task<List<EducationalQualificationModel>> GetAllEducationalQualification();
        public Task<List<BoardUniversityModel>> GetAllBoardUniversityByType(string type);
        public Task<List<BoardUniversityModel>> GetAllBoardUniversityType();

        public Task<List<WRNQualificationModel>> GetAllByIdForDetailsAsync(int id);
    }
}
