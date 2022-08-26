using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.District;
using CoreLayout.Repositories.Masters.Institute;
using CoreLayout.Repositories.Masters.InstituteCategory;
using CoreLayout.Repositories.Masters.InstituteType;
using CoreLayout.Repositories.Masters.State;
using CoreLayout.Repositories.Masters.Tehsil;
using CoreLayout.Services.Masters.InstituteCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.InstituteCategory
{
    public class InstituteCategoryService : IInstituteCategoryService
    {
        private readonly IInstituteCategoryRepository _instituteCategoryRepository;

        public InstituteCategoryService(IInstituteCategoryRepository instituteCategoryRepository)
        {
            _instituteCategoryRepository = instituteCategoryRepository;
        }

        public async Task<List<InstituteCategoryModel>> GetAllInstituteCategory()
        {
            return await _instituteCategoryRepository.GetAllAsync();
        }

        public async Task<InstituteCategoryModel> GetInstituteCategoryById(int id)
        {
            return await _instituteCategoryRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateInstituteCategoryAsync(InstituteCategoryModel instituteCategoryModel)
        {
            return await _instituteCategoryRepository.CreateAsync(instituteCategoryModel);
        }

        public async Task<int> UpdateInstituteCategoryAsync(InstituteCategoryModel instituteCategoryModel)
        {
            return await _instituteCategoryRepository.UpdateAsync(instituteCategoryModel);
        }

        public async Task<int> DeleteInstituteCategoryAsync(InstituteCategoryModel instituteCategoryModel)
        {
            return await _instituteCategoryRepository.DeleteAsync(instituteCategoryModel);
        }
    }
}
