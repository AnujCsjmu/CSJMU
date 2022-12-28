using CoreLayout.Models.Common;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Common.SequenceGenerate
{
    public interface ISequenceGenerateService
    {
        public Task<List<SequenceGenerateModel>> GetAllSequenceGenerateAsync();
        public Task<SequenceGenerateModel> GetSequenceGenerateByIdAsync(int id);
        public Task<int> CreateSequenceGenerateAsync(SequenceGenerateModel sequenceGenerateModel);
        public Task<int> UpdateSequenceGenerateAsync(SequenceGenerateModel sequenceGenerateModel);
        public Task<int> DeleteSequenceGenerateAsync(SequenceGenerateModel sequenceGenerateModel);
    }
}
