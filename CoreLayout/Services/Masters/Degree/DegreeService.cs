using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.Degree;
using CoreLayout.Repositories.Masters.Program;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Degree
{
    public class DegreeService : IDegreeService
    {
        private readonly IDegreeRepository _degreeRepository;

        public DegreeService(IDegreeRepository degreeRepository)
        {
            _degreeRepository = degreeRepository;
        }

        public async Task<List<DegreeModel>> GetAllDegree()
        {
            return await _degreeRepository.GetAllAsync();
        }

        public async Task<DegreeModel> GetDegreeById(int id)
        {
            return await _degreeRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateDegreeAsync(DegreeModel degreeModel)
        {
            return await _degreeRepository.CreateAsync(degreeModel);
        }

        public async Task<int> UpdateDegreeAsync(DegreeModel degreeModel)
        {
            return await _degreeRepository.UpdateAsync(degreeModel);
        }

        public async Task<int> DeleteDegreeAsync(DegreeModel degreeModel)
        {
            return await _degreeRepository.DeleteAsync(degreeModel);
        }
    }
}
