using CoreLayout.Models.Exam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.SubjectProfile
{
    public interface ISubjectProfileService
    {
        public Task<List<SubjectProfileModel>> GetAllSubjectProfileAsync();
        public Task<SubjectProfileModel> GetSubjectProfileByIdAsync(int id);
        public Task<int> CreateSubjectProfileAsync(SubjectProfileModel subjectProfileModel);
        public Task<int> UpdateSubjectProfileAsync(SubjectProfileModel subjectProfileModel);
        public Task<int> DeleteSubjectProfileAsync(SubjectProfileModel subjectProfileModel);
        public Task<List<SubjectProfileModel>> GetCourseFromAff_SubjectProfile(int sessioninstituteid, int sessionid);
        public Task<List<SubjectProfileModel>> GetFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid, int courseid);
        public  Task<List<SubjectProfileModel>> GetOtherFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid);
        public Task<List<SubjectProfileModel>> GetSubjectFromAff_SubjectProfile(int sessioninstituteid, int sessionid, int courseid);
        public Task<List<SubjectProfileModel>> GetMinorFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid);
        public Task<List<SubjectProfileModel>> GetSubjectFromSubjectProfileMapping();
        
    }
}
