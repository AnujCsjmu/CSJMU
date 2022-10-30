using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class ReligionModel : BaseEntity 
    {
        [Key]
        public int ReligionID { get; set; }

        [Display(Name = "Religion Name")]
        [Required(ErrorMessage = "Please enter religion name")]
        public string ReligionName { get; set; }
    }
}
