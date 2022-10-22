using CoreLayout.Models.Circular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Circular
{
    public interface ICircularService
    {
        public Task<List<CircularModel>> GetAllCircular();
        public Task<CircularModel> GetCircularById(int id);
        public Task<int> CreateCircularAsync(CircularModel circularModel);
        public Task<int> UpdateCircularAsync(CircularModel circularModel);
        public Task<int> DeleteCircularAsync(CircularModel circularModel);
        public Task<List<CircularModel>> GetAllCircularByCollageId(int circularid);
        public Task<List<CircularModel>> GetAllInstituteByCircular(int circularid);
    }
}
