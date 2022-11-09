using CoreLayout.Models.Exam;
using CoreLayout.Repositories.Exam.ExamCourseMapping;
using CoreLayout.Repositories.Exam.ExamMaster;
using CoreLayout.Repositories.Exam.Student;
using CoreLayout.Repositories.Exam.StudentAcademics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.StudentAcademics
{
    public class StudentAcademicsService : IStudentAcademicsService
    {
        private readonly IStudentAcademicsRepository _studentAcademicsRepository;

        public StudentAcademicsService(IStudentAcademicsRepository studentAcademicsRepository)
        {
            _studentAcademicsRepository = studentAcademicsRepository;
        }
        public async Task<List<StudentAcademicsModel>> GetAllStudentAcademicsAsync()
        {
            return await _studentAcademicsRepository.GetAllAsync();
        }
       
        public async Task<StudentAcademicsModel> GetStudentAcademicsByIdAsync(int id)
        {
            return await _studentAcademicsRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateStudentAcademicsAsync(StudentAcademicsModel studentAcademicsModel)
        {
            return await _studentAcademicsRepository.CreateAsync(studentAcademicsModel);
        }

        public async Task<int> UpdateStudentAcademicsAsync(StudentAcademicsModel studentAcademicsModel)
        {
            return await _studentAcademicsRepository.UpdateAsync(studentAcademicsModel);
        }

        public async Task<int> DeleteStudentAcademicsAsync(StudentAcademicsModel studentAcademicsModel)
        {
            return await _studentAcademicsRepository.DeleteAsync(studentAcademicsModel);
        }

        public async Task<List<StudentAcademicsModel>> GetFilterStudentAcademicsData(int? hdnInstituteID, int? hdnCourseId, int? hdnSubjectId, int? hdnSemYearId,string rollno)
        {
            return await _studentAcademicsRepository.GetFilterStudentAcademicsData(hdnInstituteID, hdnCourseId, hdnSubjectId, hdnSemYearId, rollno);
        }
        public async Task<int> InsertUpdateApprovalAsync(StudentAcademicsModel studentAcademicsModel)
        {
            return await _studentAcademicsRepository.InsertUpdateApprovalAsync(studentAcademicsModel);
        }
    }
}
