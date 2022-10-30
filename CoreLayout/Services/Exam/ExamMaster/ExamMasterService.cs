using CoreLayout.Models.Exam;
using CoreLayout.Repositories.Exam.ExamCourseMapping;
using CoreLayout.Repositories.Exam.ExamMaster;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.ExamMaster
{
    public class ExamMasterService : IExamMasterService
    {
        private readonly IExamMasterRepository _examMasterRepository ;

        public ExamMasterService(IExamMasterRepository examMasterRepository)
        {
            _examMasterRepository = examMasterRepository;
        }
        public async Task<List<ExamMasterModel>> GetAllExamMasterAsync()
        {
            return await _examMasterRepository.GetAllAsync();
        }
        //public async Task<List<ExamCourseMappingModel>> AlreadyExitAsync(int menuid,int roleid)
        //{
        //    return await _examCourseMappingRepository.CheckAlreadyAsync(menuid, roleid);
        //}
        public async Task<ExamMasterModel> GetExamMasterByIdAsync(int id)
        {
            return await _examMasterRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateExamMasterAsync(ExamMasterModel examMasterModel)
        {
            return await _examMasterRepository.CreateAsync(examMasterModel);
        }

        public async Task<int> UpdateExamMasterAsync(ExamMasterModel examMasterModel)
        {
            return await _examMasterRepository.UpdateAsync(examMasterModel);
        }

        public async Task<int> DeleteExamMasterAsync(ExamMasterModel examMasterModel)
        {
            return await _examMasterRepository.DeleteAsync(examMasterModel);
        }
    }
}
