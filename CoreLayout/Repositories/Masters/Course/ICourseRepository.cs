using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Course
{
    public interface ICourseRepository : IRepository<CourseModel>
    {
        Task<List<CourseTypeModel>> GetAllCourseType();
        Task<List<CourseModel>> GetAllCourseByInstitute(int instituteId);
    }
}
