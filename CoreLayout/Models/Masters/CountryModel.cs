using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class CountryModel : BaseEntity
    {
        [Key]
        public int CountryId { get; set; }

        [Display(Name ="Country Name")]
        [Required(ErrorMessage = "Please enter country name")]
        [StringLength(50)]
        public string CountryName { get; set; }

        [Display(Name = "Country Description")]
        [Required(ErrorMessage = "Please enter country description")]
        [StringLength(50)]
        public string Description { get; set; }
        public string IPAddress { get; set; }
        public int IsRecordDeleted { get; set; }
    }
}
