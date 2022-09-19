using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
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
        public string PaperName { get; set; }

        [Display(Name = "Agency Name")]
        [Required(ErrorMessage = "Please enter agency name")]
        public int UserId { get; set; }
        public string UserName { get; set; }

        [Display(Name = "PaperSetter Name")]
        [Required(ErrorMessage = "Please enter paper setter name")]
        public int PaperSetterId { get; set; }
        public string PaperSetterName { get; set; }

        public List<PCPUploadPaperModel> PaperList { get; set; }

        public List<RegistrationModel> AgencyList { get; set; }

        public List<RegistrationModel> PaperSetterList { get; set; }

        //public int? RoleId { get; set; }
        public string CreatedUserName { get; set; }

        public List<CourseModel> CourseList { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Please enter course name")]
        public int CourseID { get; set; }

        public string CourseName { get; set; }

        public int? RoleId { get; set; }
    }
}
