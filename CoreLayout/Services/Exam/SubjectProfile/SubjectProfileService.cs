using CoreLayout.Models.Exam;
using CoreLayout.Repositories.Exam.SubjectProfile;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Services.Exam.SubjectProfile
{
    public class SubjectProfileService : ISubjectProfileService
    {
        private readonly ISubjectProfileRepository _examFormRepository ;

        public SubjectProfileService(ISubjectProfileRepository examFormRepository)
        {
            _examFormRepository = examFormRepository;
        }
        public async Task<List<SubjectProfileModel>> GetAllSubjectProfileAsync()
        {
            return await _examFormRepository.GetAllAsync();
        }
      
        public async Task<SubjectProfileModel> GetSubjectProfileByIdAsync(int id)
        {
            return await _examFormRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateSubjectProfileAsync(SubjectProfileModel subjectProfileModel)
        {
            return await _examFormRepository.CreateAsync(subjectProfileModel);
        }

        public async Task<int> UpdateSubjectProfileAsync(SubjectProfileModel subjectProfileModel)
        {
            return await _examFormRepository.UpdateAsync(subjectProfileModel);
        }

        public async Task<int> DeleteSubjectProfileAsync(SubjectProfileModel subjectProfileModel)
        {
            return await _examFormRepository.DeleteAsync(subjectProfileModel);
        }
        public async Task<List<SubjectProfileModel>> GetCourseFromAff_SubjectProfile(int sessioninstituteid, int sessionid)
        {
            return await _examFormRepository.GetCourseFromAff_SubjectProfile(sessioninstituteid, sessionid);
        }
        public async Task<List<SubjectProfileModel>> GetFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid, int courseid)
        {
            return await _examFormRepository.GetFacultyFromAff_SubjectProfile(sessioninstituteid, sessionid, courseid);
        }
        public async Task<List<SubjectProfileModel>> GetOtherFacultyFromAff_SubjectProfile(int sessioninstituteid, int sessionid)
        {
            return await _examFormRepository.GetOtherFacultyFromAff_SubjectProfile(sessioninstituteid, sessionid);
        }
        public async Task<List<SubjectProfileModel>> GetSubjectFromAff_SubjectProfile(int sessioninstituteid, int sessionid, int courseid)
        {
            return await _examFormRepository.GetSubjectFromAff_SubjectProfile(sessioninstituteid, sessionid, courseid);
        }
        
    }
}
