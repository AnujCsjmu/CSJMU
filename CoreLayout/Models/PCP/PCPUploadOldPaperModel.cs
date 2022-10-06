using CoreLayout.Models.Common;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using CoreLayout.Models.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.PCP
{
    public class PCPUploadOldPaperModel : BaseEntity
    {
        [Key]
        public int OldPaperId { get; set; }
        [Display(Name = "QP Name")]
        [Required(ErrorMessage = "Please enter qp name")]
        //[StringLength(50)]
        public int QPId { get; set; }
        public List<QPMasterModel> QPList { get; set; }
        [Display(Name = "QP Name")]
        public string QPName { get; set; }
        [Display(Name = "QP Code")]
        public string QPCode { get; set; }

        [Display(Name = "Setter Name")]
        [Required(ErrorMessage = "Please enter paper setter name")]
        // [StringLength(50)]
        public int UserId { get; set; }
        public List<int> UserList { get; set; }
        public string UserName { get; set; }

        public int? PCPRegID { get; set; }
        public List<List<PCPAssignedQPModel>> QPListForGrid { get; set; }

        //display qp details
        public string QPCreatedDate { get; set; }
        public string QPCreatedBy { get; set; }

        public List<PCPRegistrationModel> PCPUserList { get; set; }


        [Display(Name = "Session")]
        [Required(ErrorMessage = "Please select syllabus")]
        public int SessionId { get; set; }
        public string Session { get; set; }//not session table is used as syllabus name
        public List<ExamCourseMappingModel> SessionList { get; set; }



        [Display(Name = "Subject Type")]
        [StringLength(2)]
        public string SubjectType { get; set; }

        //add new 
        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select course")]
        public int CourseId { get; set; }
        public List<ExamCourseMappingModel> CourseList { get; set; }
        public string CourseName { get; set; }


        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please select subject")]
        public int BranchId { get; set; }
        public List<CourseBranchMappingModel> BranchList { get; set; }
        public string BranchName { get; set; }//note branch table is used as subjectname



        [NotMapped]
        public List<QPTypeModel> QPTypeList { get; set; }
        public string QPTypeName { get; set; }

        [Display(Name = "QP Type")]
        [Required(ErrorMessage = "Please select qp type")]
        public int QPTypeId { get; set; }

        [Display(Name = "SemYear")]
        [Required(ErrorMessage = "Please select Semester year")]
        public int SemYearId { get; set; }
        public List<ExamCourseMappingModel> SemYearList { get; set; }


        [Display(Name = "Exam")]
        [Required(ErrorMessage = "Please select exam")]
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public List<ExamMasterModel> ExamList { get; set; }



        //fileupload
        [Display(Name = "Previous Question Paper")]
        public string OldPaperPath { set; get; }
        [Required(ErrorMessage = "Please choose old paper")]
        [Display(Name = "Upload Previous Session Paper")]
        public IFormFile FUOldPape { get; set; }


        //fileupload
        [Display(Name = "Previous Syllabus")]
        public string OldSyllabusPath { set; get; }
        [Required(ErrorMessage = "Please choose old syllabus")]
        [Display(Name = "Upload Syllabus")]
        public IFormFile FUOldSyllabus { get; set; }

        //fileupload
        [Display(Name = "Previous Pattern")]
        public string OldPatternPath { set; get; }
        [Required(ErrorMessage = "Please choose old pattern")]
        [Display(Name = "Upload Paper Pattern")]
        public IFormFile FUOldPattern { get; set; }

        public string FinalSubmit { get; set; }

        public string oldpaperids { get; set; }

        [Required(ErrorMessage = "Please select paper type")]
        [Display(Name = "Paper Type")]
        public string PaperType { get; set; }

        public int? QpAssignedUserId { get; set; }
    }
}
