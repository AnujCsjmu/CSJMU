using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Institute
{
    public interface IInstituteService
    {
        public Task<List<InstituteModel>> GetAllInstitute();
        public Task<InstituteModel> GetInstituteById(int id);
        public Task<int> CreateInstituteAsync(InstituteModel instituteModel);
        public Task<int> UpdateInstituteAsync(InstituteModel instituteModel);
        public Task<int> DeleteInstituteAsync(InstituteModel instituteModel);

        public Task<List<InstituteModel>> AffiliationInstituteIntakeData();
        public Task<List<InstituteModel>> All_AffiliationInstituteIntakeData();
    }
}
