using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.QPDetails
{
    public class GradeDefinitionModel :BaseEntity
    {
        [Key]
        public int GradeId { get; set; }

        [Display(Name = "Grade Name")]
        [Required(ErrorMessage = "Please enter grade name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [Remote(action: "VerifyName", controller: "QPMaster")]
        [StringLength(50)]
        public string GradeName { get; set; }

        [Display(Name = "Start Percentage")]
        public int StartPercentage { get; set; }

        [Display(Name = "End Percentage")]
        public int EndPercentage { get; set; }
    }
}
