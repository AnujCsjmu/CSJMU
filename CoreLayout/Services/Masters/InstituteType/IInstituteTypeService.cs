using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.InstituteType
{
    public interface IInstituteTypeService
    {
        public Task<List<InstituteTypeModel>> GetAllInstituteType();
        public Task<InstituteTypeModel> GetInstituteTypeById(int id);
        public Task<int> CreateInstituteTypeAsync(InstituteTypeModel instituteTypeModel);
        public Task<int> UpdateInstituteTypeAsync(InstituteTypeModel instituteTypeModel);
        public Task<int> DeleteInstituteTypeAsync(InstituteTypeModel instituteTypeModel);
    }
}
