using CoreLayout.Models.Exam;
using CoreLayout.Repositories.Exam.ExamCourseMapping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.ExamCourseMapping
{
    public class ExamCourseMappingService : IExamCourseMappingService
    {
        private readonly IExamCourseMappingRepository _examCourseMappingRepository ;

        public ExamCourseMappingService(IExamCourseMappingRepository examCourseMappingRepository)
        {
            _examCourseMappingRepository = examCourseMappingRepository;
        }
        public async Task<List<ExamCourseMappingModel>> GetAllExamCourseMappingAsync()
        {
            return await _examCourseMappingRepository.GetAllAsync();
        }
        //public async Task<List<ExamCourseMappingModel>> AlreadyExitAsync(int menuid,int roleid)
        //{
        //    return await _examCourseMappingRepository.CheckAlreadyAsync(menuid, roleid);
        //}
        public async Task<ExamCourseMappingModel> GetExamCourseMappingByIdAsync(int id)
        {
            return await _examCourseMappingRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateExamCourseMappingAsync(ExamCourseMappingModel examCourseMappingModel)
        {
            return await _examCourseMappingRepository.CreateAsync(examCourseMappingModel);
        }

        public async Task<int> UpdateExamCourseMappingAsync(ExamCourseMappingModel examCourseMappingModel)
        {
            return await _examCourseMappingRepository.UpdateAsync(examCourseMappingModel);
        }

        public async Task<int> DeleteExamCourseMappingAsync(ExamCourseMappingModel examCourseMappingModel)
        {
            return await _examCourseMappingRepository.DeleteAsync(examCourseMappingModel);
        }
    }
}
