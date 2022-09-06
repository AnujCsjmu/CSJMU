using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.QPDetails.QPMaster
{
    public interface IQPMasterService
    {
        public Task<List<QPMasterModel>> GetAllQPMaster();
        public Task<QPMasterModel> GetQPMasterById(int id);
        public Task<int> CreateQPMasterAsync(QPMasterModel qPMasterModel);
        public Task<int> UpdateQPMasterAsync(QPMasterModel qPMasterModel);
        public Task<int> DeleteQPMasterAsync(QPMasterModel qPMasterModel);
    }
}
