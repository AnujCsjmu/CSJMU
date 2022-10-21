using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.District;
using CoreLayout.Repositories.Masters.Institute;
using CoreLayout.Repositories.Masters.State;
using CoreLayout.Repositories.Masters.Tehsil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Institute
{
    public class InstituteService : IInstituteService
    {
        private readonly IInstituteRepository _instituteRepository;

        public InstituteService(IInstituteRepository instituteRepository)
        {
            _instituteRepository = instituteRepository;
        }

        public async Task<List<InstituteModel>> GetAllInstitute()
        {
            return await _instituteRepository.GetAllAsync();
        }

        public async Task<InstituteModel> GetInstituteById(int id)
        {
            return await _instituteRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateInstituteAsync(InstituteModel instituteModel)
        {
            return await _instituteRepository.CreateAsync(instituteModel);
        }

        public async Task<int> UpdateInstituteAsync(InstituteModel instituteModel)
        {
            return await _instituteRepository.UpdateAsync(instituteModel);
        }

        public async Task<int> DeleteInstituteAsync(InstituteModel instituteModel)
        {
            return await _instituteRepository.DeleteAsync(instituteModel);
        }
        public async Task<List<InstituteModel>> AffiliationInstituteIntakeData()
        {
            return await _instituteRepository.AffiliationInstituteIntakeData();
        }
    }
}
