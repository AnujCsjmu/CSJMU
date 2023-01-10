using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.WRN.WRNCourseDetails
{
    public interface IWRNCourseDetailsRepository : IRepository<WRNCourseDetailsModel>
    {
       Task<List<WRNCourseDetailsModel>> Check3CourseListAsync(string RegistrationNo);
        Task<WRNCourseDetailsModel> Check3CourseCountAsync(string RegistrationNo);
        //Task<List<WRNCourseDetailsModel>> GetAllCourseDetailByType(string Type);
        //Task<List<WRNCourseDetailsModel>> GetAllBoardUniversityType();
        // Task<List<WRNCourseDetailsModel>> GetAllByIdForDetailsAsync(int id);
    }
}
