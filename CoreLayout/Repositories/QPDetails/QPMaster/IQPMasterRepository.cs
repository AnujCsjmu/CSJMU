using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.QPDetails.QPMaster
{
    public interface IQPMasterRepository : IRepository<QPMasterModel>
    {
        Task<List<QPMasterModel>> GetAllQPByFilter(int CourseId, int SubjectId, int SemYearId, int SyllabusSessionId);
    }
}
