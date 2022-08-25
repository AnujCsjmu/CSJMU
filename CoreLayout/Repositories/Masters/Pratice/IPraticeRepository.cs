using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Pratice
{
    public interface IPraticeRepository : IRepository<PraticeModel>
    {
        Task<List<PraticeChildModel>> GetAllChildAsync();
    }
}
