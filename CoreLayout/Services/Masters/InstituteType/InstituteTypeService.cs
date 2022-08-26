using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.District;
using CoreLayout.Repositories.Masters.Institute;
using CoreLayout.Repositories.Masters.InstituteType;
using CoreLayout.Repositories.Masters.State;
using CoreLayout.Repositories.Masters.Tehsil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.InstituteType
{
    public class InstituteTypeService : IInstituteTypeService
    {
        private readonly IInstituteTypeRepository _instituteTypeRepository;

        public InstituteTypeService(IInstituteTypeRepository instituteTypeRepository)
        {
            _instituteTypeRepository = instituteTypeRepository;
        }

        public async Task<List<InstituteTypeModel>> GetAllInstituteType()
        {
            return await _instituteTypeRepository.GetAllAsync();
        }

        public async Task<InstituteTypeModel> GetInstituteTypeById(int id)
        {
            return await _instituteTypeRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateInstituteTypeAsync(InstituteTypeModel instituteTypeModel)
        {
            return await _instituteTypeRepository.CreateAsync(instituteTypeModel);
        }

        public async Task<int> UpdateInstituteTypeAsync(InstituteTypeModel instituteTypeModel)
        {
            return await _instituteTypeRepository.UpdateAsync(instituteTypeModel);
        }

        public async Task<int> DeleteInstituteTypeAsync(InstituteTypeModel instituteTypeModel)
        {
            return await _instituteTypeRepository.DeleteAsync(instituteTypeModel);
        }
    }
}
