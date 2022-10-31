using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.Category;
using CoreLayout.Repositories.Masters.Religion;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryModel>> GetAllCategory()
        {
            return await _categoryRepository.GetAllAsync();
        }

        //public async Task<StateModel> GetStateById(int id)
        //{
        //    return await _stateRepository.GetByIdAsync(id);
        //}

        //public async Task<int> CreateStateAsync(StateModel stateModel)
        //{
        //    return await _stateRepository.CreateAsync(stateModel);
        //}

        //public async Task<int> UpdateStateAsync(StateModel stateModel)
        //{
        //    return await _stateRepository.UpdateAsync(stateModel);
        //}

        //public async Task<int> DeleteStateAsync(StateModel stateModel)
        //{
        //    return await _stateRepository.DeleteAsync(stateModel);
        //}
    }
}
