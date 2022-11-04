using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Exam.StudentAcademicQPDetails
{
    public interface IStudentAcademicQPDetailsRepository : IRepository<StudentAcademicQPDetailsModel>
    {
        Task<List<StudentAcademicQPDetailsModel>> GetFilterStudentAcademicsQPData(int courseid, int subjectid, int semyearid, int syllabussessionid, int examid);
    }
}
