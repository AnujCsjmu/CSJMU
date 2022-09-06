using CoreLayout.Models.QPDetails;
using CoreLayout.Repositories.QPDetails.QPMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.QPDetails.QPMaster
{
    public class QPMasterService : IQPMasterService
    {
        private readonly IQPMasterRepository _qPMasterRepository;

        public QPMasterService(IQPMasterRepository qPMasterRepository)
        {
            _qPMasterRepository = qPMasterRepository;
        }

        public async Task<List<QPMasterModel>> GetAllQPMaster()
        {
            return await _qPMasterRepository.GetAllAsync();
        }

        public async Task<QPMasterModel> GetQPMasterById(int id)
        {
            return await _qPMasterRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateQPMasterAsync(QPMasterModel qPMasterModel)
        {
            return await _qPMasterRepository.CreateAsync(qPMasterModel);
        }

        public async Task<int> UpdateQPMasterAsync(QPMasterModel qPMasterModel)
        {
            return await _qPMasterRepository.UpdateAsync(qPMasterModel);
        }

        public async Task<int> DeleteQPMasterAsync(QPMasterModel qPMasterModel)
        {
            return await _qPMasterRepository.DeleteAsync(qPMasterModel);
        }
    }
}
