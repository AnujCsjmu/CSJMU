using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Course
{
    public interface ICourseService
    {
        public Task<List<CourseModel>> GetAllCourse();
        public Task<CourseModel> GetCourseById(int id);
        public Task<int> CreateCourseAsync(CourseModel courseModel);
        public Task<int> UpdateCourseAsync(CourseModel courseModel);
        public Task<int> DeleteCourseAsync(CourseModel courseModel);
        public Task<List<CourseTypeModel>> GetAllCourseType();

        public Task<List<CourseModel>> GetAllCourseByInstitute(int instituteId);
    }
}
