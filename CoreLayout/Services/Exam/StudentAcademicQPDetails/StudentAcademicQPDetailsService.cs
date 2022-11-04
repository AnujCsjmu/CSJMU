using CoreLayout.Models.Exam;
using CoreLayout.Repositories.Exam.ExamCourseMapping;
using CoreLayout.Repositories.Exam.ExamMaster;
using CoreLayout.Repositories.Exam.Student;
using CoreLayout.Repositories.Exam.StudentAcademicQPDetails;
using CoreLayout.Repositories.Exam.StudentAcademics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.StudentAcademicQPDetails
{
    public class StudentAcademicQPDetailsService : IStudentAcademicQPDetailsService
    {
        private readonly IStudentAcademicQPDetailsRepository _studentAcademicQPDetailsRepository;

        public StudentAcademicQPDetailsService(IStudentAcademicQPDetailsRepository studentAcademicQPDetailsRepository)
        {
            _studentAcademicQPDetailsRepository = studentAcademicQPDetailsRepository;
        }
        public async Task<List<StudentAcademicQPDetailsModel>> GetAllStudentAcademicsQPDetailsAsync()
        {
            return await _studentAcademicQPDetailsRepository.GetAllAsync();
        }

        public async Task<StudentAcademicQPDetailsModel> GetStudentAcademicsQPDetailsByIdAsync(int id)
        {
            return await _studentAcademicQPDetailsRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateStudentAcademicsQPDetailsAsync(StudentAcademicQPDetailsModel studentAcademicQPDetailsModel)
        {
            return await _studentAcademicQPDetailsRepository.CreateAsync(studentAcademicQPDetailsModel);
        }

        public async Task<int> UpdateStudentAcademicsQPDetailsAsync(StudentAcademicQPDetailsModel studentAcademicQPDetailsModel)
        {
            return await _studentAcademicQPDetailsRepository.UpdateAsync(studentAcademicQPDetailsModel);
        }

        public async Task<int> DeleteStudentAcademicsQPDetailsAsync(StudentAcademicQPDetailsModel studentAcademicQPDetailsModel)
        {
            return await _studentAcademicQPDetailsRepository.DeleteAsync(studentAcademicQPDetailsModel);
        }

        public async Task<List<StudentAcademicQPDetailsModel>> GetFilterStudentAcademicsQPData(int courseid, int subjectid, int semyearid, int syllabussessionid, int examid)
        {
            return await _studentAcademicQPDetailsRepository.GetFilterStudentAcademicsQPData(courseid, subjectid, semyearid,syllabussessionid, examid);
        }
    }
}
