using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Exam.StudentAcademics
{
    public interface IStudentAcademicsRepository : IRepository<StudentAcademicsModel>
    {
        Task<List<StudentAcademicsModel>> GetFilterStudentAcademicsData(int? hdnInstituteID, int? hdnCourseId, int? hdnSubjectId, int? hdnSemYearId);
    }
}
