using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.QPDetails.QPType
{
    public interface IQPTypeService
    {
        public Task<List<QPTypeModel>> GetAllQPType();
        public Task<QPTypeModel> GetQPTypeById(int id);
        public Task<int> CreateQPTypeAsync(QPTypeModel qPTypeModel);
        public Task<int> UpdateQPTypeAsync(QPTypeModel qPTypeModel);
        public Task<int> DeleteQPTypeAsync(QPTypeModel qPTypeModel);
    }
}
