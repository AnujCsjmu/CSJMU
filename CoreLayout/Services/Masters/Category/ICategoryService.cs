using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Category
{
    public interface ICategoryService
    {
        public Task<List<CategoryModel>> GetAllCategory();
        //public Task<StateModel> GetStateById(int id);
        //public Task<int> CreateStateAsync(StateModel stateModel);
        //public Task<int> UpdateStateAsync(StateModel stateModel);
        //public Task<int> DeleteStateAsync(StateModel stateModel);
    }
}
