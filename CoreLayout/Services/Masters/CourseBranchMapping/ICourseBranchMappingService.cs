using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.CourseBranchMapping
{
    public interface ICourseBranchMappingService
    {
        public Task<List<CourseBranchMappingModel>> GetAllCourseBranchMapping();
        public Task<CourseBranchMappingModel> GetCourseBranchMappingById(int id);
        public Task<int> CreateCourseBranchMappingAsync(CourseBranchMappingModel courseBranchMappingModel);
        public Task<int> UpdateCourseBranchMappingAsync(CourseBranchMappingModel courseBranchMappingModel);
        public Task<int> DeleteCourseBranchMappingAsync(CourseBranchMappingModel courseBranchMappingModel);
        public Task<List<CourseBranchMappingModel>> alreadyExit(int courseid,int branchid);
    }
}
