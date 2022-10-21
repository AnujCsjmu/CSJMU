using CoreLayout.Models.Circular;
using CoreLayout.Repositories.Circular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Circular
{
    public class CircularService : ICircularService
    {
        private readonly ICircularRepository _circularRepository;

        public CircularService(ICircularRepository circularRepository)
        {
            _circularRepository = circularRepository;
        }

        public async Task<List<CircularModel>> GetAllCircular()
        {
            return await _circularRepository.GetAllAsync();
        }

        public async Task<CircularModel> GetCircularById(int id)
        {
            return await _circularRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateCircularAsync(CircularModel circularModel)
        {
            return await _circularRepository.CreateAsync(circularModel);
        }

        public async Task<int> UpdateCircularAsync(CircularModel circularModel)
        {
            return await _circularRepository.UpdateAsync(circularModel);
        }

        public async Task<int> DeleteCircularAsync(CircularModel circularModel)
        {
            return await _circularRepository.DeleteAsync(circularModel);
        }
    }
}
