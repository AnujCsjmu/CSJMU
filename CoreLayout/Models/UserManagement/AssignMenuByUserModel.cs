using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.UserManagement
{
    public class AssignMenuByUserModel : BaseEntity
    {
        [Key]
        public int MenuPermissionId { get; set; }

        [Required(ErrorMessage = "Please enter menu name")]
        [Display(Name = "Menu Name")]
        public int MenuId { get; set; }

        [Required(ErrorMessage = "Please enter user name")]
        [Display(Name = "User Name")]
        public int UserId { get; set; }
        public int Active { get; set; }
        public int IsRecordDeleted { get; set; }
        public string IPAddress { get; set; }


        public string UserName { get; set; }

        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }

        public List<MenuModel> MenuList { get; set; }
        public List<RegistrationModel> UserList { get; set; }
    }
}
