using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class TehsilModel : BaseEntity
    {
        [Key]
        public int TehsilId { get; set; }

        [Display(Name = "District Name")]
        [Required(ErrorMessage = "Please enter district name")]
        public int DistrictId { get; set; }

        [Display(Name = "Tehsil Name")]
        [Required(ErrorMessage = "Please enter tehsil name")]
        [StringLength(50)]
        public string TehsilName { get; set; }
        public string IPAddress { get; set; }
        public int IsRecordDeleted { get; set; }

        public List<DistrictModel> DistrictList { get; set; }

        public string DistrictName { get; set; }
    }
}
