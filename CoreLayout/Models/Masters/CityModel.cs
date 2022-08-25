using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class CityModel : BaseEntity
    {
        [Key]
        public int CityId { get; set; }

        [Display(Name = "State Name")]
        [Required(ErrorMessage = "Please enter state name")]
        public int StateId { get; set; }

        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "Please enter country name")]
        public int CountryId { get; set; }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Please enter city name")]
        public string CityName { get; set; }

        [NotMapped]
        [Display(Name = "State Name")]
        public string StateName { get; set; }


        [NotMapped]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please select status")]
        public int Status { get; set; }
    }
}
