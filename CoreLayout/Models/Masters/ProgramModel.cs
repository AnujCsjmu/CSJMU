using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class ProgramModel :BaseEntity
    {
        [Key]
        public int ProgramId { get; set; }

        [Display(Name = "Program Name")]
        [Required(ErrorMessage = "Please enter program name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(100)]
        [Remote(action: "VerifyName", controller: "Program")]
        public string ProgramName { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter description")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(100)]
        public string Description { get; set; }

        public string IPAddress { get; set; }

        public int UserId { get; set; }
        public int IsRecordDeleted { get; set; }
    }
}
