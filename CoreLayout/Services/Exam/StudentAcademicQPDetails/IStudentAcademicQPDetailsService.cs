using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.StudentAcademicQPDetails
{
    public interface IStudentAcademicQPDetailsService
    {
        public Task<List<StudentAcademicQPDetailsModel>> GetAllStudentAcademicsQPDetailsAsync();
        public Task<StudentAcademicQPDetailsModel> GetStudentAcademicsQPDetailsByIdAsync(int id);
        public Task<int> CreateStudentAcademicsQPDetailsAsync(StudentAcademicQPDetailsModel studentAcademicQPDetailsModel);
        public Task<int> UpdateStudentAcademicsQPDetailsAsync(StudentAcademicQPDetailsModel studentAcademicQPDetailsModel);
        public Task<int> DeleteStudentAcademicsQPDetailsAsync(StudentAcademicQPDetailsModel studentAcademicQPDetailsModel);
        public Task<List<StudentAcademicQPDetailsModel>> GetFilterStudentAcademicsQPData(int academicid,int courseid, int subjectid, int semyearid, int syllabussessionid, int examid);
    }
}
