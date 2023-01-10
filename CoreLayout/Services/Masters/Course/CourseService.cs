using CoreLayout.Models.Masters;
using CoreLayout.Repositories.Masters.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Course
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<CourseModel>> GetAllCourse()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task<CourseModel> GetCourseById(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateCourseAsync(CourseModel courseModel)
        {
            return await _courseRepository.CreateAsync(courseModel);
        }

        public async Task<int> UpdateCourseAsync(CourseModel courseModel)
        {
            return await _courseRepository.UpdateAsync(courseModel);
        }

        public async Task<int> DeleteCourseAsync(CourseModel courseModel)
        {
            return await _courseRepository.DeleteAsync(courseModel);
        }
        public async Task<List<CourseTypeModel>> GetAllCourseType()
        {
            return await _courseRepository.GetAllCourseType();
        }
        public async Task<List<CourseModel>> GetAllCourseByInstitute(int instituteId)
        {
            return await _courseRepository.GetAllCourseByInstitute(instituteId);
        }
    }
}
