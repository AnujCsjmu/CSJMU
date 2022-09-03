using CoreLayout.Models.Masters;
using CoreLayout.Repositories.Masters.CourseBranchMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.CourseBranchMapping
{
    public class CourseBranchMappingService : ICourseBranchMappingService
    {
        private readonly ICourseBranchMappingRepository _courseBranchMappingRepository;

        public CourseBranchMappingService(ICourseBranchMappingRepository courseBranchMappingRepository)
        {
            _courseBranchMappingRepository = courseBranchMappingRepository;
        }
        public async Task<int> CreateCourseBranchMappingAsync(CourseBranchMappingModel courseBranchMappingModel)
        {
            return await _courseBranchMappingRepository.CreateAsync(courseBranchMappingModel);
        }

        public async Task<int> DeleteCourseBranchMappingAsync(CourseBranchMappingModel courseBranchMappingModel)
        {
            return await _courseBranchMappingRepository.DeleteAsync(courseBranchMappingModel);
        }

        public async Task<List<CourseBranchMappingModel>> GetAllCourseBranchMapping()
        {
            return await _courseBranchMappingRepository.GetAllAsync();
        }

        public async Task<CourseBranchMappingModel> GetCourseBranchMappingById(int id)
        {
            return await _courseBranchMappingRepository.GetByIdAsync(id);
        }

        public async Task<int> UpdateCourseBranchMappingAsync(CourseBranchMappingModel courseBranchMappingModel)
        {
            return await _courseBranchMappingRepository.UpdateAsync(courseBranchMappingModel);
        }
        public async Task<List<CourseBranchMappingModel>> alreadyExit(int courseid, int branchid)
        {
            return await _courseBranchMappingRepository.alreadyExit(courseid, branchid);
        }
    }
}
