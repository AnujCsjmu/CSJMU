using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Masters.Deparment
{
    public interface IDepartmentRepository
    {
        Task<List<DepartmentModel>> GetAllAsync();
    }
}
