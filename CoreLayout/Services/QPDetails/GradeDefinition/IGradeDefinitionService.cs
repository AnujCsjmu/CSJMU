using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.QPDetails.GradeDefinition
{
    public interface IGradeDefinitionService
    {
        public Task<List<GradeDefinitionModel>> GetAllGradeDefinition();
        public Task<GradeDefinitionModel> GetGradeDefinitionById(int id);
        public Task<int> CreateGradeDefinitionAsync(GradeDefinitionModel gradeDefinitionModel);
        public Task<int> UpdateGradeDefinitionAsync(GradeDefinitionModel gradeDefinitionModel);
        public Task<int> DeleteGradeDefinitionAsync(GradeDefinitionModel gradeDefinitionModel);
    }
}
