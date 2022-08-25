using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Department
{
    public interface IDepartmentService
    {
        public Task<List<DepartmentModel>> GetAllDepartment();
    }
}
