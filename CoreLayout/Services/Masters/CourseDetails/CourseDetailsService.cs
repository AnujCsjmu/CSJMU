using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.CourseDetails;
using CoreLayout.Repositories.Masters.Degree;
using CoreLayout.Repositories.Masters.Program;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.CourseDetails
{
    public class CourseDetailsService : ICourseDetailsService
    {
        private readonly ICourseDetailsRepository _courseDetailsRepository;

        public CourseDetailsService(ICourseDetailsRepository courseDetailsRepository)
        {
            _courseDetailsRepository = courseDetailsRepository;
        }

        public async Task<List<CourseDetailsModel>> GetAllCourseDetail()
        {
            return await _courseDetailsRepository.GetAllAsync();
        }

        public async Task<CourseDetailsModel> GetCourseDetailById(int id)
        {
            return await _courseDetailsRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateCourseDetailAsync(CourseDetailsModel courseDetailsModel)
        {
            return await _courseDetailsRepository.CreateAsync(courseDetailsModel);
        }

        public async Task<int> UpdateCourseDetailAsync(CourseDetailsModel courseDetailsModel)
        {
            return await _courseDetailsRepository.UpdateAsync(courseDetailsModel);
        }

        public async Task<int> DeleteCourseDetailAsync(CourseDetailsModel courseDetailsModel)
        {
            return await _courseDetailsRepository.DeleteAsync(courseDetailsModel);
        }
        public async Task<List<SessionModel>> GetAllSession()
        {
            return await _courseDetailsRepository.GetAllSession();
        }
    }
}
