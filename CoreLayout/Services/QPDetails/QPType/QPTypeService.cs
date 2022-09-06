using CoreLayout.Models.QPDetails;
using CoreLayout.Repositories.QPDetails.QPType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.QPDetails.QPType
{
    public class QPTypeService :IQPTypeService
    {
        private readonly IQPTypeRepository _iQPTypeRepository;

        public QPTypeService(IQPTypeRepository iQPTypeRepository)
        {
            _iQPTypeRepository = iQPTypeRepository;
        }

        public async Task<List<QPTypeModel>> GetAllQPType()
        {
            return await _iQPTypeRepository.GetAllAsync();
        }

        public async Task<QPTypeModel> GetQPTypeById(int id)
        {
            return await _iQPTypeRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateQPTypeAsync(QPTypeModel qPTypeModel)
        {
            return await _iQPTypeRepository.CreateAsync(qPTypeModel);
        }

        public async Task<int> UpdateQPTypeAsync(QPTypeModel qPTypeModel)
        {
            return await _iQPTypeRepository.UpdateAsync(qPTypeModel);
        }

        public async Task<int> DeleteQPTypeAsync(QPTypeModel qPTypeModel)
        {
            return await _iQPTypeRepository.DeleteAsync(qPTypeModel);
        }
    }
}
