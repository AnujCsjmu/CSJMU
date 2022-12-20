using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.UserManagement
{
    public class ButtonPermissionModel :BaseEntity
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = " Menu Name")]
        [Required(ErrorMessage = "Please select Menu")]
        //public string ButtonId { get; set; }
        public int MenuId { get; set; }

        [Display(Name = "URL")]
        //[Required(ErrorMessage = "Please enter url")]
        [StringLength(50)]
        public string URL { get; set; }

        [Display(Name = "Controller")]
        //[Required(ErrorMessage = "Please select controller")]
        public string Controller { get; set; }

        [Display(Name = "Index")]
        [Required(ErrorMessage = "Please enter index")]
        [StringLength(50)]
        public string Action { get; set; }

        [Display(Name = " Buttons")]
        [Required(ErrorMessage = "Please select buttons")]
        //public string ButtonId { get; set; }
        public int ButtonId { get; set; }

        [Display(Name = " User")]
        [Required(ErrorMessage = "Please select user")]
        public int UserId { get; set; }

        [Display(Name = " Role")]
        [Required(ErrorMessage = "Please select role")]
        public int RoleId { get; set; }

        [Display(Name = " IsRecordActive")]
        [Required(ErrorMessage = "Please select status")]
        public int IsRecordActive { get; set; }
        public int IsRecordDeleted { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string ButtonName { get; set; }
       // public List<int> ButtonList { get; set; }
        public string IPAddress { get; set; }

        public string MenuName { get; set; }

        public List<ButtonModel> ButtonModelList { get; set; }
        public List<MenuModel> MenuList { get; set; }

        public List<ButtonPermissionModel> MenuModelList { get; set; }
        public List<int> ButtonList { get; set; }

        //public List<ButtonPermissionModel> ButtonListEdit { get; set; }
        public List<RoleModel> RoleList { get; set; }
        public List<RegistrationModel> UserList { get; set; }
    }
}
