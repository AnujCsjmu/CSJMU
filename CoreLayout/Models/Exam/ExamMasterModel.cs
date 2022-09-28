using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Exam
{
    public class ExamMasterModel : BaseEntity
    {
        [Key]
        public int ExamId { get; set; }

        [Remote(action: "VerifyName", controller: "ExamMaster")]
        [Display(Name = "Exam Name")]
        [Required(ErrorMessage = "Please enter exam name")]
        public string ExamName { get; set; }


        [Display(Name = "Session")]
        [Required(ErrorMessage = "Please enter session")]
        public int SessionId { get; set; }
        public string Session{ get; set; }
        public List<SessionModel> SessionList { get; set; }
    }
}
