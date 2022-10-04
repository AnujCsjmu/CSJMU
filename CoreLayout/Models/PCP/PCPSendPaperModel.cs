using CoreLayout.Models.Common;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreLayout.Models.PCP
{
    public class PCPSendPaperModel :BaseEntity
    {
        [Key]
        public int SendPaperId { get; set; }

        [Display(Name = "Paper Name")]
        [Required(ErrorMessage = "Please enter paper name")]
        
        public int PaperId { get; set; }

        [Display(Name = "Paper Name")]
        public string PaperName { get; set; }

        [Display(Name = "Paper Code")]
        public string PaperCode { get; set; }

        [Display(Name = "Agency")]
        [Required(ErrorMessage = "Please enter agency name")]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<RegistrationModel> AgencyList { get; set; }


        [Display(Name = "Setter Name")]
        //[Required(ErrorMessage = "Please enter paper setter name")]
        public int? PaperSetterId { get; set; }
        public string PaperSetterName { get; set; }
        public List<RegistrationModel> PaperSetterList { get; set; }


        public List<int> PaperList { get; set; }
        public List<string> SelectedPaperList { get; set; }

      

        //public int? RoleId { get; set; }
        public string CreatedUserName { get; set; }

        //public List<CourseModel> CourseList { get; set; }

        //[Display(Name = "Course Name")]
        ////[Required(ErrorMessage = "Please enter course name")]
        //public int? CourseID { get; set; }

        //public string CourseName { get; set; }
       

        public int? RoleId { get; set; }

        public string paperids { get; set; }

        public string sendpaperids { get; set; }

        //[Display(Name = "Terms and Conditions")]
        //[Range(typeof(bool), "true", "true", ErrorMessage = "You gotta tick the box!")]
        //public bool TermsAndConditions { get; set; }
        [Display(Name = "Paper")]
        public string PaperPath { get; set; }

        [Display(Name = "Agency")]
        public int? AgencyId { get; set; }

        [Display(Name = "Agency Name")]
        public string AgencyName { get; set; }
        //public int? QPId { get; set; }
        //[Display(Name = "QP Name")]
        //public string QPName { get; set; }


        [Display(Name = "Send By")]
        public string CreatedByName { get; set; }
        public List<PCPSendPaperModel> FilterList { get; set; }

     
        [Display(Name = "Paper Password")]
        public string PaperPassword { get; set; }
        //public string Session { get; set; }
        [Display(Name = "Session Type")]
        public string SessionType { get; set; }

        [Display(Name = "Paper Open Time")]
        [Required(ErrorMessage = "Please enter paper open time")]
        //[Range(typeof(DateTime), "1/1/2022", "1/1/2040")]
        public DateTime PaperOpenTime { get; set; }

        [RegularExpression(@"^(?:[0-9]{1,3}.){3}[0-9]{1,3}$")]
        [Display(Name = "Static IP")]
        [Required(ErrorMessage = "Please enter static ip")]
        public string StaticIPAddress { get; set; }

        [Display(Name = "Decrypt Password")]
        public string DecryptPassword { get; set; }

        public DateTime  ServerDateTime { get; set; }

        //[Display(Name = "Branch Name")]
        //public string BranchName { get; set; }

        //[Display(Name = "SemYear")]
        //public string SemYearId { get; set; }

        public string AcceptedStatus { get; set; }

        //new
        [Display(Name = "Exam")]
        [Required(ErrorMessage = "Please select exam")]
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public List<ExamMasterModel> ExamList { get; set; }


        [Display(Name = "Course")]
        //[Required(ErrorMessage = "Please select course")]
        public int? CourseId { get; set; }
        public List<ExamCourseMappingModel> CourseList { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }


        [Display(Name = "SemYear")]
        //[Required(ErrorMessage = "Please select Semester year")]
        public int? SemYearId { get; set; }


        [Display(Name = "Subject")]
        //[Required(ErrorMessage = "Please select subject")]
        public int? BranchId { get; set; }
        public List<CourseBranchMappingModel> BranchList { get; set; }
        [Display(Name = "Subject")]
        public string BranchName { get; set; }//note branch table is used as subjectname

        [Display(Name = "Syllabus")]
        //[Required(ErrorMessage = "Please select syllabus")]
        public int? SessionId { get; set; }
        public string Session { get; set; }//not session table is used as syllabus name
        public List<ExamCourseMappingModel> SessionList { get; set; }


        [Display(Name = "QP Name")]
        //[Required(ErrorMessage = "Please enter qp name")]
        //[StringLength(50)]
        public int? QPId { get; set; }

        public string QPName { get; set; }
        public List<QPMasterModel> QPList { get; set; }
        [Display(Name = "QP Code")]
        public string QPCode { get; set; }
    }
}
