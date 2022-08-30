using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Degree
{
    public interface IDegreeService
    {
        public Task<List<DegreeModel>> GetAllDegree();
        public Task<DegreeModel> GetDegreeById(int id);
        public Task<int> CreateDegreeAsync(DegreeModel degreeModel);
        public Task<int> UpdateDegreeAsync(DegreeModel degreeModel);
        public Task<int> DeleteDegreeAsync(DegreeModel degreeModel);
    }
}
