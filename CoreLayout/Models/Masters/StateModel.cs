using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class StateModel : BaseEntity 
    {
        [Key]
        public int StateId { get; set; }

        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "Please enter country name")]
        public int CountryId { get; set; }

        [Display(Name = "State Name")]
        [Required(ErrorMessage = "Please enter state name")]
        [StringLength(50)]
        public string StateName { get; set; }
        public string IPAddress { get; set; }
        public int IsRecordDeleted { get; set; }

        [NotMapped]
        public List<CountryModel> CountryList { get; set; }

        public string CountryName { get; set; }

    }
}
