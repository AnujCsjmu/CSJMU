using CoreLayout.Models.Circular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Circular
{
    public interface ICircularRepository : IRepository<CircularModel>
    {
          Task<List<CircularModel>> GetAllCircularByCollageId(int instituteid);
    }
}
