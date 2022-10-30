using CoreLayout.Models.Exam;
using CoreLayout.Repositories.Exam.ExamCourseMapping;
using CoreLayout.Repositories.Exam.ExamMaster;
using CoreLayout.Repositories.Exam.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.Student
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository ;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<List<StudentModel>> GetAllStudentAsync()
        {
            return await _studentRepository.GetAllAsync();
        }
       
        public async Task<StudentModel> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateStudentAsync(StudentModel studentModel)
        {
            return await _studentRepository.CreateAsync(studentModel);
        }

        public async Task<int> UpdateStudentAsync(StudentModel studentModel)
        {
            return await _studentRepository.UpdateAsync(studentModel);
        }

        public async Task<int> DeleteStudentAsync(StudentModel studentModel)
        {
            return await _studentRepository.DeleteAsync(studentModel);
        }
    }
}
