using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.UserManagement
{
    public class AssignMenuByRoleModel :BaseEntity
    {
        [Key]
        public int MenuPermissionId { get; set; }

        [Required(ErrorMessage = "Please enter menu name")]
        [Display(Name = "Menu Name")]
        //[Remote(action: "VerifyAssignMenuByRole", controller: "AssignMenuByRole")]
        public int MenuId { get; set; }

        [Required(ErrorMessage = "Please enter role name")]
        [Display(Name = "Role Name")]
        //[Remote(action: "VerifyAssignMenuByRole", controller: "AssignMenuByRole")]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        public string MenuName { get; set; }

        public int Active { get; set; }
        public int IsRecordDeleted { get; set; }
        public string IPAddress { get; set; }

        //public int EntryBy { get; set; }
        // public int UpdateBy { get; set; }

        public List<MenuModel> MenuList { get; set; }
        public List<RoleModel> RoleList { get; set; }
    }
}
