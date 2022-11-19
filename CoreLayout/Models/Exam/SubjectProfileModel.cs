using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Exam
{
    public class SubjectProfileModel : SubjectProfileMappingModel
    {
        [Key]
        public int SubjectProfileId { get; set; }

        [Display(Name = "Exam Name")]
        [Required(ErrorMessage = "Please enter exam name")]
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public List<ExamMasterModel> ExamList { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Please enter course name")]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<InstituteModel> CourseList { get; set; }

        [Display(Name = "Faculty Name")]
        [Required(ErrorMessage = "Please enter faculty name")]
        public int FacultyID { get; set; }
        public string FacultyName { get; set; }
        public List<InstituteModel> FacultyList { get; set; }

        [Display(Name = "IsOtherFaculty")]
        public bool IsOtherFaculty { get; set; }

        [Display(Name = "Other Faculty Name")]
        //[Required(ErrorMessage = "Please enter other faculty name")]
        public int? OtherFacultyId { get; set; }
        public string OtherFacultyName { get; set; }
        public List<InstituteModel> OtherFacultyList { get; set; }

        [Display(Name = "IsAddMinor")]
        public bool IsAddMinor { get; set; }

        [Display(Name = "Minor Faculty Name")]
        //[Required(ErrorMessage = "Please enter minor faculty name")]
        public int? MinorFacultyId { get; set; }
        public string MinorFacultyName { get; set; }
        public List<InstituteModel> MinorFacultyList { get; set; }

        [Display(Name = "Minor Subject Name")]
        //[Required(ErrorMessage = "Please enter minor subject name")]
        public int? MinorSubjectId { get; set; }
        public string MinorSubjectName { get; set; }
        public List<InstituteModel> MinorSubjectList { get; set; }

        [Display(Name = "Vocational Subject Name")]
        //[Required(ErrorMessage = "Please enter minor subject name")]
        public int? VocationalSubjectId { get; set; }
        public string VocationalSubjectName { get; set; }
        public List<BranchModel> VocationalSubjectList { get; set; }

        [Display(Name = "Co-curricular Subject Name")]
        //[Required(ErrorMessage = "Please enter minor subject name")]
        public int? CoCurricularSubjectId { get; set; }
        public string CoCurricularSubjectName { get; set; }
        public List<BranchModel> CoCurricularSubjectList { get; set; }

        public string SubjectType { get; set; }

        [Display(Name = "Major Subject Name")]
        //[Required(ErrorMessage = "Please enter minor subject name")]
        public int? MajorSubjectId { get; set; }
        public string MajorSubjectName { get; set; }

        [Display(Name = "Collage")]
        public int? InstituteId { get; set; }
        public string InstituteName { get; set; }

        public string Status { get; set; }
    }
}
