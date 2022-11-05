using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Exam
{
    public class StudentAcademicQPDetailsModel :BaseEntity
    {
        [Key]
        public int StudentAcademicQPId { get; set; }

        [Display(Name = "AcademicId")]
        [Required(ErrorMessage = "Please enter academic id")]
        public int AcademicId { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select course")]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
         public List<CourseModel> CourseList { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please select subject")]
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public List<BranchModel> SubjectList { get; set; }

        [Display(Name = "SemYear")]
        [Required(ErrorMessage = "Please enter sem year")]
        public int SemYearId { get; set; }
       // public List<BranchModel> SemYearList { get; set; }

        [Display(Name = "Syllabus Session")]
        [Required(ErrorMessage = "Please select syllabus session")]
        public int SyllabusSessionId { get; set; }
        public string SyllabusSessionName { get; set; }
        public List<SessionModel> SyllabusSessionList { get; set; }

        [Display(Name = "QP Name")]
        [Required(ErrorMessage = "Please select qp name")]
        public int QPId { get; set; }
        [Display(Name = "QP Name")]
        public string QPName { get; set; }
        public List<QPMasterModel> QPList { get; set; }
        public List<int> QPListForInsert { get; set; }
        [Display(Name = "QP Code")]
        public string QPCode { get; set; }

        [Display(Name = "Exam")]
        [Required(ErrorMessage = "Please select exam")]
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public List<ExamMasterModel> ExamList { get; set; }

        public List<StudentAcademicQPDetailsModel> DataList { get; set; }
    }
}
