using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class BranchModel :BaseEntity
    {
        [Key]
        public int BranchID { get; set; }

        [Remote(action: "VerifyName", controller: "Branch")]
        [Display(Name = "Branch Code")]
        [Required(ErrorMessage = "Please enter branch code")]
        public string BranchCode { get; set; }

        [Display(Name = "Branch Name")]
        [Required(ErrorMessage = "Please enter branch name")]
        public string BranchName { get; set; }

        [Display(Name = "Hindi Name")]
        public string HindiName { get; set; }

        [Display(Name = "Cerificate Name")]
        public string CerificateName { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Subject Type")]
        [Required(ErrorMessage = "Please select subject type")]
        //[StringLength(2)]
        public string SubjectType { get; set; }


        [Display(Name = "Duration")]
        [Required(ErrorMessage = "Please enter duration")]
        public int Duration { get; set; }

    }
}
