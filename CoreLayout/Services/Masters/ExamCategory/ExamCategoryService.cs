using CoreLayout.Models;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.District;
using CoreLayout.Repositories.Masters.ExamCategory;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.ExamCategory
{
    public class ExamCategoryService : IExamCategoryService
    {
        private readonly IExamCategoryRepository _examCategoryRepository;

        public ExamCategoryService(IExamCategoryRepository examCategoryRepository)
        {
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<List<ExamCategoryModel>> GetExamCategoryAsync()
        {
            return await _examCategoryRepository.GetExamCategoryAsync();
        }

        //public async Task<DistrictModel> GetDistrictById(int id)
        //{
        //    return await _districtRepository.GetByIdAsync(id);
        //}

        //public async Task<int> CreateDistrictAsync(DistrictModel districtModel)
        //{
        //    return await _districtRepository.CreateAsync(districtModel);
        //}

        //public async Task<int> UpdateDistrictAsync(DistrictModel districtModel)
        //{
        //    return await _districtRepository.UpdateAsync(districtModel);
        //}

        //public async Task<int> DeleteDistrictAsync(DistrictModel districtModel)
        //{
        //    return await _districtRepository.DeleteAsync(districtModel);
        //}
        //public async Task<List<DistrictModel>> Get7DistrictAsync()
        //{
        //    return await _districtRepository.Get7DistrictAsync();
        //}
    }
}
