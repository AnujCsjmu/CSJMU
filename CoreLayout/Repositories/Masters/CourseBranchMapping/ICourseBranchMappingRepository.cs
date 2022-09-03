using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.CourseBranchMapping
{
    public interface ICourseBranchMappingRepository : IRepository<CourseBranchMappingModel>
    {
        Task<List<CourseBranchMappingModel>> alreadyExit(int courseid, int branchid);
    }
}
