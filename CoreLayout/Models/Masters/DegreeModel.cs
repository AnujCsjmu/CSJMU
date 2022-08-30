using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class DegreeModel : BaseEntity
    {
        [Key]
        public int DegreeId { get; set; }

        [Display(Name = "Degree Name")]
        [Required(ErrorMessage = "Please enter degree name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(100)]
        
        public string DegreeName { get; set; }

        [Display(Name = "Degree Code")]
        [Remote(action: "VerifyName", controller: "Degree")]
        [Required(ErrorMessage = "Please enter degree code")]
        [StringLength(50)]
        public string DegreeCode { get; set; }

        [Display(Name = "Degree Templete File")]
        public string DegreeTempleteFile { get; set; }

        [Display(Name = "Degree Suffix")]
        public string DegreeSuffix { get; set; }

        [Display(Name = "Degree Prefix")]
        public string DegreePrefix { get; set; }

        public string IPAddress { get; set; }
        public int IsRecordDeleted { get; set; }
    }
}
