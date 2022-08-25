using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class DistrictModel :BaseEntity
    {
        [Key]
        public int DistrictId { get; set; }

        [Display(Name = "State Name")]
        [Required(ErrorMessage = "Please enter state name")]
        public int StateId { get; set; }

        [Display(Name = "District Name")]
        [Required(ErrorMessage = "Please enter district name")]
        [StringLength(50)]
        public string DistrictName { get; set; }
        public string IPAddress { get; set; }
        public int IsRecordDeleted { get; set; }

        public List<StateModel> StateList { get; set; }

        public string StateName { get; set; }
    }
}
