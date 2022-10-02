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
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.PCP
{
    public class PCPUploadPaperModel:BaseEntity
    {
        [Key]
        public int PaperId { get; set; }

        //[Display(Name = "Paper Code")]
        //[Required(ErrorMessage = "Please enter paper code")]
        //[StringLength(50)]
        //public string PaperCode { get; set; }

        //[Display(Name = "Paper Name")]
        //[Required(ErrorMessage = "Please enter paper name")]
        //[StringLength(100)]
        //public string PaperName { get; set; }

        //[Display(Name = "Paper Hindi Name")]
        //public string PaperHindiName { get; set; }

        [Display(Name = "Paper Password")]
        //[Required(ErrorMessage = "Please enter paper password")]
        [StringLength(50)]
        public string PaperPassword { get; set; }

        public string PaperRandomPassword { get; set; }
        public string DecryptPassword { get; set; }

        [Display(Name = "Question Paper")]
        public string PaperPath { set; get; }

        [Required(ErrorMessage = "Please choose question paper")]
        [Display(Name = "Upload Question Paper")]
        public IFormFile UploadPaper { get; set; }

         [Display(Name = "Upload Question Paper")]
        public IFormFile UploadPaperEdit { get; set; }


        [Remote(action: "VerifyName", controller: "PCPUploadPaper")]
        public List<PCPAssignedQPModel> QPList { get; set; }
        //public string SessionType { get; set; }

        [Display(Name = "Setter Name")]
        public string UserName { get; set; }

        [Display(Name = "QP Name")]
        [Required(ErrorMessage = "Please enter qp name")]
        //[Remote(action: "VerifyName", controller: "PCPUploadPaper")]
        public int QPId { get; set; }
        public string QPName { get; set; }

        [Display(Name = "QP Code")]
        public string QPCode { get; set; }

        public int? UserId { get; set; }

        public int? PCPRegID { get; set; }

        public string paperids { get; set; }
        public int? RoleId { get; set; }

        [Display(Name = "Setter Name")]
        public string PaperSetterName { get; set; }

        public string CreatedUserName { get; set; }


        [Display(Name = "Institute Code")]
        public string InstituteCode { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }



        [Display(Name = "Subject Code")]
        public string BranchCode { get; set; }


        public string DownloadStatus { get; set; }

        public int? ReturnUploadId { get; set; }

        public string FinalSubmit { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select course")]
        public int CourseId { get; set; }
        public List<QPMasterModel> CourseList { get; set; }
        public string CourseName { get; set; }


        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please select subject")]
        public int BranchId { get; set; }
        public List<QPMasterModel> BranchList { get; set; }
        public string BranchName { get; set; }//note branch table is used as subjectname


        [Required(ErrorMessage = "Please enter session")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }
        public string Session { get; set; }
        public List<QPMasterModel> SessionList { get; set; }


        [Display(Name = "Answer Paper")]
        public string AnswerPath { set; get; }

        [Required(ErrorMessage = "Please choose answer paper")]
        [Display(Name = "Upload Answer Paper")]
        public IFormFile AnswerPaper { get; set; }

        [Display(Name = "Upload Answer Paper")]
        public IFormFile AnswerPaperEdit { get; set; }

        [Display(Name = "SemYear")]
        [Required(ErrorMessage = "Please select Semester year")]
        public int SemYearId { get; set; }
        public List<QPMasterModel> SemYearList { get; set; }

        public string RequestQuestionPwdStatus { set; get; }

        public string RequestAnswerPwdStatus { set; get; }

        public int? ExamId { get; set; }
    }
}
