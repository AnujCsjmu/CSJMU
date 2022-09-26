using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.QPDetails
{
    public class QPTypeModel : BaseEntity
    {
        [Key]
        public int QPTypeId { get; set; }

        [Remote(action: "VerifyName", controller: "QPType")]
        [Display(Name = "QP Type Name")]
        [Required(ErrorMessage = "Please enter qp type")]
        //[RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(50)]
        public string QPTypeName { get; set; }
        public int UserId { get; set; }
        
    }
}
