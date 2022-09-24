using CoreLayout.Models.Common;
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

        [Display(Name = "Paper Code")]
        [Required(ErrorMessage = "Please enter paper code")]
        [StringLength(50)]
        public string PaperCode { get; set; }

        [Display(Name = "Paper Name")]
        [Required(ErrorMessage = "Please enter paper name")]
        [StringLength(100)]
        public string PaperName { get; set; }

        [Display(Name = "Paper Hindi Name")]
        public string PaperHindiName { get; set; }

        [Display(Name = "Paper Password")]
        //[Required(ErrorMessage = "Please enter paper password")]
        [StringLength(50)]
        public string PaperPassword { get; set; }

        public string PaperRandomPassword { get; set; }
        public string DecryptPassword { get; set; }

        [Display(Name = "Paper")]
        public string PaperPath { set; get; }

        [Required(ErrorMessage = "Please choose paper")]
        [Display(Name = "Upload Paper")]
        public IFormFile UploadPaper { get; set; }


        [Required(ErrorMessage = "Please enter session")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }
        public string Session { get; set; }
        public List<SessionModel> SessionList { get; set; }

        [Remote(action: "VerifyName", controller: "PCPUploadPaper")]
        public List<PCPAssignedQPModel> QPList { get; set; }
        public string SessionType { get; set; }

        [Display(Name = "Paper Setter Name")]
        public string UserName { get; set; }

        [Display(Name = "QP Name")]
        [Required(ErrorMessage = "Please enter qp name")]
        //[Remote(action: "VerifyName", controller: "PCPUploadPaper")]
        public int QPId { get; set; }
        public string QPName { get; set; }
        public string QPCode { get; set; }

        public int? UserId { get; set; }

        public int? PCPRegID { get; set; }

        public string paperids { get; set; }
        public int? RoleId { get; set; }

        [Display(Name = "Paper Setter Name")]
        public string PaperSetterName { get; set; }

        public string CreatedUserName { get; set; }


        [Display(Name = "Institute Code")]
        public string InstituteCode { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }


        [Display(Name = "Subject Code")]
        public string BranchCode { get; set; }


        [Display(Name = "Subject Name")]
        public string BranchName { get; set; }

        public string DownloadStatus { get; set; }

        public int? ReturnUploadId { get; set; }

        public string FinalSubmit { get; set; }

    }
}
