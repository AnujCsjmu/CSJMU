using CoreLayout.Models.Common;
using CoreLayout.Repositories.Common.SequenceGenerate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Common.SequenceGenerate
{
    public class SequenceGenerateService : ISequenceGenerateService
    {
        private readonly ISequenceGenerateRepository _sequenceGenerateRepository;

        public SequenceGenerateService(ISequenceGenerateRepository sequenceGenerateRepository)
        {
            _sequenceGenerateRepository = sequenceGenerateRepository;
        }

        public async Task<List<SequenceGenerateModel>> GetAllSequenceGenerateAsync()
        {
            return await _sequenceGenerateRepository.GetAllAsync();
        }

        public async Task<SequenceGenerateModel> GetSequenceGenerateByIdAsync(int id)
        {
            return await _sequenceGenerateRepository.GetByIdAsync(id);
        }
        public async Task<int> CreateSequenceGenerateAsync(SequenceGenerateModel sequenceGenerateModel)
        {
            return await _sequenceGenerateRepository.CreateAsync(sequenceGenerateModel);
        }
        public async Task<int> UpdateSequenceGenerateAsync(SequenceGenerateModel sequenceGenerateModel)
        {
            return await _sequenceGenerateRepository.UpdateAsync(sequenceGenerateModel);
        }
        public async Task<int> DeleteSequenceGenerateAsync(SequenceGenerateModel sequenceGenerateModel)
        {
            return await _sequenceGenerateRepository.DeleteAsync(sequenceGenerateModel);
        }
    }
}
