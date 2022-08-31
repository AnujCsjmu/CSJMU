using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class CourseModel :BaseEntity
    {
        [Key]
        public int CourseID { get; set; }

        [Display(Name = "Course Code")]
        [Remote(action: "VerifyName", controller: "Course")]
        [Required(ErrorMessage = "Please enter course code")]
        //[RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(20)]
        public string CourseCode { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Please enter course name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(100)]
        public string CourseName { get; set; }

        [Display(Name = "Hindi Name")]
        //[RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(100)]
        public string HindiName { get; set; }

        [Display(Name = "Min Duration")]
        public int? MinDuration { get; set; }

        [Display(Name = "Max Duration")]
        public int? MaxDuration { get; set; }

        [Display(Name = "Program")]
        [Required(ErrorMessage = "Please enter program")]
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }

        [Display(Name = "Course Type")]
        public int? CourseTypeId { get; set; }
        public string CourseTypeName { get; set; }

        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "Aff Online")]
        [StringLength(1)]
        public string AffOnline { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Nominal Duration")]
        public int? NominalDuration { get; set; }

        [Display(Name = "On Campus")]
        public int? OnCampus { get; set; }

        [Display(Name = "Off Campus")]
        public int? OffCampus { get; set; }

        [Display(Name = "Certificate Name")]
        public string CertificateName { get; set; }

        [Display(Name = "Certificate Hindi Name")]
        public string CertificateHindiName { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public int? IsNEP { get; set; }

        public int IsActive { get; set; }

        public int? IsLateralAllowed { get; set; }

        public List<ProgramModel> ProgramList { get; set; }
        public List<CourseTypeModel> CourseTypeList { get; set; }
    }
}
