using CoreLayout.Models;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Program
{
    public interface IProgramService
    {
        public Task<List<ProgramModel>> GetAllProgram();
        public Task<ProgramModel> GetProgramById(int id);
        public Task<int> CreateProgramAsync(ProgramModel programModel);
        public Task<int> UpdateProgramAsync(ProgramModel programModel);
        public Task<int> DeleteProgramAsync(ProgramModel programModel);
    }
}
