using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Exam
{
    public class ExamCourseMappingModel:BaseEntity
    {
        [Key]
        public int ECId { get; set; }

        [Display(Name = "Exam Name")]
        [Required(ErrorMessage = "Please enter exam name")]
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public List<ExamMasterModel> ExamList { get; set; }


        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please enter course")]
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public List<CourseModel> CourseList { get; set; }



        [Display(Name = "Session")]
        [Required(ErrorMessage = "Please enter session")]
        public int SessionId { get; set; }
        public string Session { get; set; }


        [Display(Name = "Course Type")]
        [Required(ErrorMessage = "Please enter course type")]
        public int CourseTypeId { get; set; }
        public string CourseTypeName { get; set; }

        [Display(Name = "Semester Year")]
        [Required(ErrorMessage = "Please enter sem year")]
        public int SemYearId { get; set; }
        public string SemYearName { get; set; }

        [Display(Name = "Paper Setter Permission")]
        [Required(ErrorMessage = "Please enter setter permission")]
        public string PaperSetterPermission { get; set; }
    }
}
