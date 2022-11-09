using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayout.Models.Exam
{
    public class StudentAcademicsModel : BaseEntity
    {
        [Key]
        public int AcademicId { get; set; }

        [Display(Name = "Student")]
        [Required(ErrorMessage = "Please select student")]
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public List<StudentModel> StudentList { get; set; }

        //[Remote(action: "VerifyRollNo", controller: "StudentAcademics")]
        [Display(Name = "Roll No")]
        [Required(ErrorMessage = "Please select roll no")]
        [StringLength(20)]
        public string RollNo { get; set; }

        [Display(Name = "Institute")]
        [Required(ErrorMessage = "Please select institute")]
        public int InstituteID { get; set; }
        public string InstituteName { get; set; }
        public List<InstituteModel> InstituteList { get; set; }
        public string InstituteCodeWithName { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select course")]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<InstituteModel> CourseList { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please select subject")]
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public List<InstituteModel> SubjectList { get; set; }

        [Display(Name = "SemYear")]
        [Required(ErrorMessage = "Please select sem year")]
        public int SemYearId { get; set; }
        public List<BranchModel> SemYearList { get; set; }

        [Display(Name = "Syllabus Session")]
        [Required(ErrorMessage = "Please select syllabus session")]
        public int SyllabusSessionId { get; set; }
        public string SyllabusSessionName { get; set; }
        public List<SessionModel> SyllabusSessionList { get; set; }

        [Display(Name = "Exam Center")]
        [Required(ErrorMessage = "Please select exam center")]
        public int ExamCenterId { get; set; }
        public string ExamCenterName { get; set; }
        public List<InstituteModel> ExamCenterList { get; set; }

        [Display(Name = "Exam Category")]
        //public string ExamCategoryId { get; set; }
        public string ExamCategoryName { get; set; }
        [Display(Name = "Exam Category Code")]
        public string ExamCategoryCode { get; set; }
        public List<ExamCategoryModel> ExamCategoryList { get; set; }

        //[Required(ErrorMessage = "Please select approval letter")]
        [Display(Name = "Approval Letter")]
        public IFormFile FUApprovalLetter { get; set; }
        [Display(Name = "Approval Letter")]
        public string ApprovalLetterPath { get; set; }

        [Display(Name = "Academic Session")]
        [Required(ErrorMessage = "Please select academic session")]
        public int AcademicSessionId { get; set; }
        public string AcademicSessionName { get; set; }
        public List<SessionModel> AcademicSessionList { get; set; }

        [Display(Name = "Exam")]
        [Required(ErrorMessage = "Please enter exam")]
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public List<ExamMasterModel> ExamList { get; set; }

        [Display(Name = "Previous Session")]
        [Required(ErrorMessage = "Please select previous session")]
        public int PreviousSessionId { get; set; }

        [Display(Name = "Previous Session Name")]
        public string PreviousSessionName { get; set; }

        [Display(Name = "Previous Result Status")]
        public string PreviousResultStatus { get; set; }
        public List<SessionModel> PreviousSessionIdList { get; set; }

        [Display(Name = "Current Exam Month")]
        [Required(ErrorMessage = "Please select current exam month")]
        public int CurrentExamMonth { get; set; }

        [Display(Name = "Old Exam Month")]
        [Required(ErrorMessage = "Please select old exam month")]
        public int OldExamMonth { get; set; }

        public string Batch { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Approved Status")]
        public string ApprovedStatus { get; set; }

        [Display(Name = "Approved By")]
        public int? ApprovedBy { get; set; }

        [Display(Name = "Approved Date")]
        public string ApprovedDate { get; set; }

        [Display(Name = "Approved Remarks")]
        public string ApprovedRemarks { get; set; }

        //edit file upload
        //[Display(Name = "Approval Letter")]
        //public IFormFile FUEditApprovalLetter { get; set; }
        //[Display(Name = "Approval Letter")]
        //public string EditApprovalLetterPath { get; set; }

    }
}
