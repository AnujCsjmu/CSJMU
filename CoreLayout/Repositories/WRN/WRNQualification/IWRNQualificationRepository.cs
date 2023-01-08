using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Models.WRN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.WRN.WRNQualification
{
    public interface IWRNQualificationRepository : IRepository<WRNQualificationModel>
    {
       Task<List<EducationalQualificationModel>> GetAllEducationalQualification();
       Task<List<BoardUniversityModel>> GetAllBoardUniversityByType(string Type);
       Task<List<BoardUniversityModel>> GetAllBoardUniversityType();
        Task<List<WRNQualificationModel>> GetAllByIdForDetailsAsync(int id);
    }
}
