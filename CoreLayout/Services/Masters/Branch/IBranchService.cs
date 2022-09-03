using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Branch
{
    public interface IBranchService
    {
        public Task<List<BranchModel>> GetAllBranch();
        public Task<BranchModel> GetBranchById(int id);
        public Task<int> CreateBranchAsync(BranchModel branchModel);
        public Task<int> UpdateBranchAsync(BranchModel branchModel);
        public Task<int> DeleteBranchAsync(BranchModel branchModel);
    }
}
