using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.InstituteCategory
{
    public interface IInstituteCategoryService
    {
        public Task<List<InstituteCategoryModel>> GetAllInstituteCategory();
        public Task<InstituteCategoryModel> GetInstituteCategoryById(int id);
        public Task<int> CreateInstituteCategoryAsync(InstituteCategoryModel instituteCategoryModel);
        public Task<int> UpdateInstituteCategoryAsync(InstituteCategoryModel instituteCategoryModel);
        public Task<int> DeleteInstituteCategoryAsync(InstituteCategoryModel instituteCategoryModel);
    }
}
