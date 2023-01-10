using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.WRN
{
    public class WRNQualificationModel :BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Registration No.")]
        [Required(ErrorMessage = "Please enter registration no")]
        [StringLength(15)]
        public string RegistrationNo { get; set; }

        [Display(Name = "Education/Qualification")]
        [Required(ErrorMessage = "Please select education/qualification")]
        public int QualificationId { get; set; }

        public string Qualification { get; set; }
        //[Display(Name = "Examination Details")]
        //[StringLength(100)]
        public string QualificationDetails { get; set; }

        [Display(Name = "Board/Institute")]
        [Required(ErrorMessage = "Please select board/institute")]
        //[StringLength(100)]
        public int BoardUniversityId { get; set; }

        public string University { get; set; }

        //[Display(Name = "Board University")]
        //[Required(ErrorMessage = "Please select board university")]
        public string BoardUniversityDetails { get; set; }

        [Display(Name = "Passing Year")]
        //[Required(ErrorMessage = "Please select passing year")]
        public int? PassingYear { get; set; }

        [Display(Name = "Result Status")]
        [Required(ErrorMessage = "Please select result status")]
        public string ResultStatus { get; set; }

        [Display(Name = "Marks Criteria")]
        //[Required(ErrorMessage = "Please select marks criteria")]
        public string MarksCriteria { get; set; }
        public List<int> PassingYearList { get; set; }

        [Display(Name = "Passing Month")]
        //[Required(ErrorMessage = "Please select passing month")]
        public int? PassingMonth { get; set; }

        public List<int> PassingMonthList { get; set; }

        [Display(Name = "Percentage/Grade")]
        //[Required(ErrorMessage = "Please select percentag")]
        //[RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Marks can't have more than 2 decimal places")]
        public string PercentagOfMarksObtained { get; set; }

        [Display(Name = "Division Class Grade")]
        //[Required(ErrorMessage = "Please select grade")]
        public string DivisionClassGrade { get; set; }

        [Display(Name = "Subjects")]
        //[Required(ErrorMessage = "Please select subjects")]
        public string Subjects { get; set; }

        [Display(Name = "Result Criteria")]
        [Required(ErrorMessage = "Please select result criteria")]
        public string ResultCriteria { get; set; }

        [Display(Name = "Other University")]
        //[Required(ErrorMessage = "Please enter other university")]
        public string OtherUniversity { get; set; }

        [Display(Name = "Qualification Type")]
        [Required(ErrorMessage = "Please select qualification type")]
        public string QualificationType { get; set; }

        [Display(Name = "Upload Sacn Copy of Marksheet")]
        public IFormFile MarksheetAttachment { get; set; }
        public string MarksheetAttachmentPath { get; set; }
        public List<WRNQualificationModel> DataList { get; set; }

        public List<EducationalQualificationModel> EducationalQualificationList { get; set; }
        //public List<BoardUniversityModel> BoardUniversityList { get; set; }
        public List<BoardUniversityModel> BoardUniversityTypeList { get; set; }
    }
}
