using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.Student
{
    public interface IStudentService
    {
        public Task<List<StudentModel>> GetAllStudentAsync();
        public Task<StudentModel> GetStudentByIdAsync(int id);
        public Task<int> CreateStudentAsync(StudentModel studentModel);
        public Task<int> UpdateStudentAsync(StudentModel studentModel);
        public Task<int> DeleteStudentAsync(StudentModel studentModel);
    }
}
