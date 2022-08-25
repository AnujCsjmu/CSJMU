using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.UserManagement
{
    public class MenuModel :BaseEntity
    {
        [Key]
        public int MenuID { get; set; }

        [Display(Name = "Level1 Menu")]
        [Required(ErrorMessage = "Please enter level1 menu")]
        public string Level1 { get; set; }

        [Display(Name = "Level2 Menu")]
        [Required(ErrorMessage = "Please enter level2 menu")]
        public string Level2 { get; set; }

        [Display(Name = "Level3 Menu")]
        [Required(ErrorMessage = "Please enter level3 menu")]
        public string Level3 { get; set; }

        [Display(Name = "Role")]
        //[Required(ErrorMessage = "Please select role")]
        public string Role { get; set; }

        public int RoleId { get; set; }

        [Display(Name = "Controller Name")]
        [Required(ErrorMessage = "Please enter controller name")]
        public string Controller { get; set; }

        [Display(Name = "Action Name")]
        [Required(ErrorMessage = "Please enter menu url")]
        public string Action { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please enter status")]
        public string Status { get; set; }

        [Display(Name = "Remarks")]
        [Required(ErrorMessage = "Please enter remarks")]
        public string Remarks { get; set; }

        public int UserRoleId { get; set; }

    }
}
