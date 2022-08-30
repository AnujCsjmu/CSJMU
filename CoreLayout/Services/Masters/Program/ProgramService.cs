using CoreLayout.Models;
using CoreLayout.Models.Masters;
using CoreLayout.Repositories;
using CoreLayout.Repositories.Masters.Program;
using CoreLayout.Repositories.Masters.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Masters.Program
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _programRepository;

        public ProgramService(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }

        public async Task<List<ProgramModel>> GetAllProgram()
        {
            return await _programRepository.GetAllAsync();
        }

        public async Task<ProgramModel> GetProgramById(int id)
        {
            return await _programRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateProgramAsync(ProgramModel programModel)
        {
            return await _programRepository.CreateAsync(programModel);
        }

        public async Task<int> UpdateProgramAsync(ProgramModel programModel)
        {
            return await _programRepository.UpdateAsync(programModel);
        }

        public async Task<int> DeleteProgramAsync(ProgramModel programModel)
        {
            return await _programRepository.DeleteAsync(programModel);
        }
    }
}
