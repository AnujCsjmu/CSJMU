using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.StudentAcademics
{
    public interface IStudentAcademicsService
    {
        public Task<List<StudentAcademicsModel>> GetAllStudentAcademicsAsync();
        public Task<StudentAcademicsModel> GetStudentAcademicsByIdAsync(int id);
        public Task<int> CreateStudentAcademicsAsync(StudentAcademicsModel studentAcademicsModel);
        public Task<int> UpdateStudentAcademicsAsync(StudentAcademicsModel studentAcademicsModel);
        public Task<int> DeleteStudentAcademicsAsync(StudentAcademicsModel studentAcademicsModel);
        public Task<List<StudentAcademicsModel>> GetFilterStudentAcademicsData(int? hdnInstituteID, int? hdnCourseId, int? hdnSubjectId, int? hdnSemYearId);
        public Task<int> InsertUpdateApprovalAsync(StudentAcademicsModel studentAcademicsModel);
    }
}
