using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class PraticeModel : DepartmentModel
    {
   
        public int Id { get; set; }
        public int PraticeId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Display(Name = "State Name")]
        [Required(ErrorMessage = "Please enter state name")]
        public int StateId { get; set; }

        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "Please enter country name")]
        public int CountryId { get; set; }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Please enter city name")]
        public int CityId { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Please enter role")]
        public int roleid { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Please enter department")]
        public List<int> departmentid { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please enter gender")]
        public string gender { get; set; }

        //[Display(Name = "Upload Image")]
        //[Required(ErrorMessage = "Please upload image")]
        [Display(Name = "File Name")]
        public string UploadFileName { set; get; }

        [Required(ErrorMessage = "Please choose profile image")]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }


        [NotMapped]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        [NotMapped]
        [Display(Name = "State Name")]
        public string StateName { get; set; }

        [NotMapped]
        [Display(Name = "City Name")]
        public string CityName { get; set; }

       
        [Display(Name = "Role Name")]
        public string Role { get; set; }


    }
}
