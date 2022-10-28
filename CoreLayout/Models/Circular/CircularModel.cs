using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Circular
{
    public class CircularModel : BaseEntity
    {
        [Key]
        public int CircularId { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please enter title")]
        public string Title { get; set; }

        [Display(Name = "Display In WebSite")]
        [Required(ErrorMessage = "Please enter display in website")]
        public int DisplayInWebSite { get; set; }


        [Display(Name = "Display In Collage")]
        [Required(ErrorMessage = "Please select display in collage")]
        public int DisplayInCollage { get; set; }

        [Display(Name = "Original File Name")]
        public string OriginalFileName { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }
        public string Related { get; set; }

        [Display(Name = "Date of Uploading")]
        [Required(ErrorMessage = "Please enter upload date")]
        public DateTime UploadDate { get; set; }

        public string Flag { get; set; }

        public int ReturnCircularId { get; set; }

        //district
        [Display(Name = "District Name")]
        [Required(ErrorMessage = "Please select district")]
        //[Remote(action: "GetInstitute", controller: "Circular")]
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public List<DistrictModel> DistrictList { get; set; }

        //Course
        [Display(Name = "Course Name")]
        //[Required(ErrorMessage = "Please select course")]
        public int? CourseID { get; set; }
        public string CourseName { get; set; }
        public List<CourseModel> CourseList { get; set; }

        //institute
        [Display(Name = "Institute Name")]
        [Required(ErrorMessage = "Please select institute")]
        public int InstituteID { get; set; }
        //public string InstituteCode { get; set; }
        public string InstituteName { get; set; }
        public List<int> InstituteList { get; set; }

       // public List<InstituteModel> InstituteListsss { get; set; }

        //file upload
        [Display(Name = "Upload Circular")]
        [Required(ErrorMessage = "Please choose cicular")]
        public IFormFile FUCircular { get; set; }
        [Display(Name = "Upload Circular")]
        public string CircularPath { get; set; }
    }
}
