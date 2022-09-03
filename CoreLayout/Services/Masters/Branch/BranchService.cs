using CoreLayout.Models.Masters;
using CoreLayout.Repositories.Masters.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Branch
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<List<BranchModel>> GetAllBranch()
        {
            return await _branchRepository.GetAllAsync();
        }

        public async Task<BranchModel> GetBranchById(int id)
        {
            return await _branchRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateBranchAsync(BranchModel branchModel)
        {
            return await _branchRepository.CreateAsync(branchModel);
        }

        public async Task<int> UpdateBranchAsync(BranchModel branchModel)
        {
            return await _branchRepository.UpdateAsync(branchModel);
        }

        public async Task<int> DeleteBranchAsync(BranchModel branchModel)
        {
            return await _branchRepository.DeleteAsync(branchModel);
        }
    }
}
