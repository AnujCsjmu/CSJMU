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
        //public int EntryBy { get; set; }
        //public int UpdateBy { get; set; }

        [Display(Name = "Level1")]
        public string Level1 { get; set; }

        [Display(Name = "Level2")]
        public string Level2 { get; set; }

        [Display(Name = "Level3")]
        public string Level3 { get; set; }
    }
}
