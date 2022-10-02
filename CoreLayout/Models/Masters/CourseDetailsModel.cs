using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class CourseDetailsModel:BaseEntity
    {
        [Key]
        public int CourseDetailId { get; set; }

        //[Display(Name = "Pramotion Type")]
        //[Required(ErrorMessage = "Please select pramotion type")]
        ////[RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        ////[StringLength(100)]
        //public string PramotionType { get; set; }

        [Display(Name = "Course Duration Type")]
        [Required(ErrorMessage = "Please select course exam type")]
        //[RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
       // [StringLength(100)]
        public string CourseDurationType { get; set; }

        [Display(Name = "Session")]
        [Required(ErrorMessage = "Please select session")]
        public int SessionId { get; set; }

        public string Session { get; set; }

        [Display(Name = "Number Of Year")]
        [Required(ErrorMessage = "Please enter number of year")]
        public int NumberOfYear { get; set; }

        [Display(Name = "Number Of Semester")]
        [Required(ErrorMessage = "Please enter number Of Semester")]
        public int NumberOfSemester { get; set; }


        [Display(Name = "Process Group Type")]
        [Required(ErrorMessage = "Please enter process group type")]
        public string ProcessGroupType { get; set; }

        public int IsGrade { get; set; }

        public int IsCreadit { get; set; }

        public List<SessionModel> SessionList { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Please enter course name")]
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public List<CourseModel> CourseList { get; set; }
    }
}
