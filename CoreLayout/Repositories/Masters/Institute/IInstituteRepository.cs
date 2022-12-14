using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Institute
{
    public interface IInstituteRepository : IRepository<InstituteModel>
    {
        Task<List<InstituteModel>> AffiliationInstituteIntakeData();
        Task<List<InstituteModel>> All_AffiliationInstituteIntakeData();
        Task<List<InstituteModel>> DistinctAffiliationInstituteIntakeData();
    }
}
