using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Repositories.Exam.SubjectProfile
{
    public interface ISubjectProfileRepository : IRepository<SubjectProfileModel>
    {
       Task<List<SubjectProfileModel>> GetCourseFromAff_SubjectProfile(int sessioninstituteid, int sessionid);
       Task<List<SubjectProfileModel>> GetFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid, int courseid);
       Task<List<SubjectProfileModel>> GetOtherFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid);
       Task<List<SubjectProfileModel>> GetSubjectFromAff_SubjectProfile(int sessioninstituteid, int sessionid, int courseid);
    }
}
