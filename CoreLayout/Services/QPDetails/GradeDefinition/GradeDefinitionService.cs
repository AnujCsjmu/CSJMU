using CoreLayout.Models.QPDetails;
using CoreLayout.Repositories.QPDetails.GradeDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.QPDetails.GradeDefinition
{
    public class GradeDefinitionService : IGradeDefinitionService
    {
        private readonly IGradeDefinitionRepository _gradeDefinitionRepository;

        public GradeDefinitionService(IGradeDefinitionRepository gradeDefinitionRepository)
        {
            _gradeDefinitionRepository = gradeDefinitionRepository;
        }

        public async Task<List<GradeDefinitionModel>> GetAllGradeDefinition()
        {
            return await _gradeDefinitionRepository.GetAllAsync();
        }

        public async Task<GradeDefinitionModel> GetGradeDefinitionById(int id)
        {
            return await _gradeDefinitionRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateGradeDefinitionAsync(GradeDefinitionModel gradeDefinitionModel)
        {
            return await _gradeDefinitionRepository.CreateAsync(gradeDefinitionModel);
        }

        public async Task<int> UpdateGradeDefinitionAsync(GradeDefinitionModel gradeDefinitionModel)
        {
            return await _gradeDefinitionRepository.UpdateAsync(gradeDefinitionModel);
        }

        public async Task<int> DeleteGradeDefinitionAsync(GradeDefinitionModel gradeDefinitionModel)
        {
            return await _gradeDefinitionRepository.DeleteAsync(gradeDefinitionModel);
        }
    }
}
