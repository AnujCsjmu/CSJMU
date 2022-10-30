using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.ExamMaster
{
    public interface IExamMasterService
    {
        public Task<List<ExamMasterModel>> GetAllExamMasterAsync();

        //public Task<List<ExamCourseMappingModel>> AlreadyExitAsync(int menuid,int roleid);
        public Task<ExamMasterModel> GetExamMasterByIdAsync(int id);
        public Task<int> CreateExamMasterAsync(ExamMasterModel examMasterModel);
        public Task<int> UpdateExamMasterAsync(ExamMasterModel examMasterModel);
        public Task<int> DeleteExamMasterAsync(ExamMasterModel examMasterModel);
    }
}
