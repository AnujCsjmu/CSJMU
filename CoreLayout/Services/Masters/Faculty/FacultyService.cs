using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.Faculty;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Faculty
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;

        public FacultyService(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        public async Task<List<FacultyModel>> GetAllFaculty()
        {
            return await _facultyRepository.GetAllAsync();
        }

        public async Task<FacultyModel> GetFacultyById(int id)
        {
            return await _facultyRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateFacultyAsync(FacultyModel facultyModel)
        {
            return await _facultyRepository.CreateAsync(facultyModel);
        }

        public async Task<int> UpdateFacultyAsync(FacultyModel facultyModel)
        {
            return await _facultyRepository.UpdateAsync(facultyModel);
        }

        public async Task<int> DeleteFacultyAsync(FacultyModel facultyModel)
        {
            return await _facultyRepository.DeleteAsync(facultyModel);
        }
    }
}
