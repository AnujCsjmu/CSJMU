using CoreLayout.Models;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.ExamCategory
{
    public interface IExamCategoryService
    {
        public Task<List<ExamCategoryModel>> GetExamCategoryAsync();
        //public Task<DistrictModel> GetDistrictById(int id);
        //public Task<int> CreateDistrictAsync(DistrictModel districtModel);
        //public Task<int> UpdateDistrictAsync(DistrictModel districtModel);
        //public Task<int> DeleteDistrictAsync(DistrictModel districtModel);
        //public Task<List<DistrictModel>> Get7DistrictAsync();
    }
}
