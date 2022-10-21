using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.District
{
    public interface IDistrictRepository : IRepository<DistrictModel>
    {
       Task<List<DistrictModel>> Get7DistrictAsync();
    }
}
