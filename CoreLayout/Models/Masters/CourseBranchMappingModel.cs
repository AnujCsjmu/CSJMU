using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class CourseBranchMappingModel :BaseEntity
    {
        [Key]
        public int CBId { get; set; }

        
        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select course")]
        public int CourseId { get; set; }

        [Display(Name = "Branch Name")]
        [Required(ErrorMessage = "Please select branch")]
        public int  BranchId { get; set; }

        public string CourseName { get; set; }

        public string BranchName { get; set; }

        public List<CourseModel> CourseList { get; set; }

        public List<BranchModel> BranchList { get; set; }

        public int UserId { get; set; }
        
    }
}
