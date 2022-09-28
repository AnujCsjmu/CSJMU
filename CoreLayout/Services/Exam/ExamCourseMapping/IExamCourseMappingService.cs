using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.ExamCourseMapping
{
    public interface IExamCourseMappingService
    {
        public Task<List<ExamCourseMappingModel>> GetAllExamCourseMappingAsync();

        //public Task<List<ExamCourseMappingModel>> AlreadyExitAsync(int menuid,int roleid);
        public Task<ExamCourseMappingModel> GetExamCourseMappingByIdAsync(int id);
        public Task<int> CreateExamCourseMappingAsync(ExamCourseMappingModel examCourseMappingModel);
        public Task<int> UpdateExamCourseMappingAsync(ExamCourseMappingModel examCourseMappingModel);
        public Task<int> DeleteExamCourseMappingAsync(ExamCourseMappingModel examCourseMappingModel);
    }
}
