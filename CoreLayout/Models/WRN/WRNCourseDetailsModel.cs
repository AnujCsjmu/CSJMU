using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.WRN
{
    public class WRNCourseDetailsModel : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "District Name")]
        [Required(ErrorMessage = "Please enter district no")]
        public int DistrictId { get; set; }

        [Display(Name = "District Name")]
        public string DistrictName { get; set; }
        public List<DistrictModel> DistrictList { get; set; }

        [Display(Name = "Institute Name")]
        [Required(ErrorMessage = "Please enter institute name")]
        public int InstituteId { get; set; }

        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }
        public string InstituteCodeWithName { get; set; }
        public List<InstituteModel> InstituteList { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Please enter course name")]
        public int CourseId { get; set; }

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        public string CourseCodeWithName { get; set; }
        public int? CourseCount { get; set; }
        public List<CourseModel> CourseList { get; set; }

        [Display(Name = "Registration No")]
        public string RegistrationNo { get; set; }

        public List<WRNCourseDetailsModel> WRNCourseDataList { get; set; }
    }
}
