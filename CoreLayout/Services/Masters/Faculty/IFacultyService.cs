using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Faculty
{
    public interface IFacultyService
    {
        public Task<List<FacultyModel>> GetAllFaculty();
        public Task<FacultyModel> GetFacultyById(int id);
        public Task<int> CreateFacultyAsync(FacultyModel facultyModel);
        public Task<int> UpdateFacultyAsync(FacultyModel facultyModel);
        public Task<int> DeleteFacultyAsync(FacultyModel facultyModel);
    }
}
