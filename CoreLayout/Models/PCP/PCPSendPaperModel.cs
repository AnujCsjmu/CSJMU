using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        [Display(Name = "Agency Name")]
        [Required(ErrorMessage = "Please enter agency name")]
        public int UserId { get; set; }

        [Display(Name = "Setter Name")]
        public string UserName { get; set; }

        [Display(Name = "PaperSetter Name")]
        //[Required(ErrorMessage = "Please enter paper setter name")]
        public int? PaperSetterId { get; set; }
        public string PaperSetterName { get; set; }

        public List<int> PaperList { get; set; }

        public List<string> SelectedPaperList { get; set; }

        public List<RegistrationModel> AgencyList { get; set; }

        public List<RegistrationModel> PaperSetterList { get; set; }

        //public int? RoleId { get; set; }
        public string CreatedUserName { get; set; }

        public List<CourseModel> CourseList { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Please enter course name")]
        public int CourseID { get; set; }

        public string CourseName { get; set; }
        public string CourseCode { get; set; }

        public int? RoleId { get; set; }

        public string paperids { get; set; }

        //[Display(Name = "Terms and Conditions")]
        //[Range(typeof(bool), "true", "true", ErrorMessage = "You gotta tick the box!")]
        //public bool TermsAndConditions { get; set; }
        [Display(Name = "Paper")]
        public string PaperPath { get; set; }

        [Display(Name = "Agency")]
        public int? AgencyId { get; set; }

        [Display(Name = "Agency Name")]
        public string AgencyName { get; set; }
        public int? QPId { get; set; }

        [Display(Name = "Send By")]
        public string CreatedByName { get; set; }
        public List<PCPSendPaperModel> FilterList { get; set; }

        [Display(Name = "QP Name")]
        public string QPName { get; set; }
        [Display(Name = "Paper Password")]
        public string PaperPassword { get; set; }
        public string Session { get; set; }
        [Display(Name = "Session Type")]
        public string SessionType { get; set; }

        [Display(Name = "Paper Open Time")]
        public DateTime PaperOpenTime { get; set; }

        [Display(Name = "Static IP")]
        public string StaticIPAddress { get; set; }

        [Display(Name = "Decrypt Password")]
        public string DecryptPassword { get; set; }

        
    }
}
