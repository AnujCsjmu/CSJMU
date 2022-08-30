using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class FacultyModel : BaseEntity 
    {
        [Key]
        public int FacultyID { get; set; }

        [Display(Name = "Program Name")]
        [Required(ErrorMessage = "Please enter program name")]
        public int ProgramId { get; set; }

        [Display(Name = "Faculty Name")]
        [Required(ErrorMessage = "Please enter faculty name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(100)]
        [Remote(action: "VerifyName", controller: "Faculty")]
        public string FacultyName { get; set; }
        public string IPAddress { get; set; }
        public int IsRecordDeleted { get; set; }

        [NotMapped]
        public List<ProgramModel> ProgramList { get; set; }

        public string ProgramName { get; set; }

    }
}
