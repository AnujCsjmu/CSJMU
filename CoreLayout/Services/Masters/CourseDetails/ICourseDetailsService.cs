using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.CourseDetails
{
    public interface ICourseDetailsService
    {
        public Task<List<CourseDetailsModel>> GetAllCourseDetail();
        public Task<CourseDetailsModel> GetCourseDetailById(int id);
        public Task<int> CreateCourseDetailAsync(CourseDetailsModel courseDetailsModel);
        public Task<int> UpdateCourseDetailAsync(CourseDetailsModel courseDetailsModel);
        public Task<int> DeleteCourseDetailAsync(CourseDetailsModel courseDetailsModel);

        public Task<List<SessionModel>> GetAllSession();
    }
}
