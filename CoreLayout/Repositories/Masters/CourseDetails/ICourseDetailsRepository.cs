using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.CourseDetails
{
    public interface ICourseDetailsRepository : IRepository<CourseDetailsModel>
    {
        Task<List<SessionModel>> GetAllSession();
    }
}
