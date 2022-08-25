using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class RoleModel : BaseEntity
    {
        [Key]
        public int RoleID { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Please enter role name")]
        [StringLength(50)]
        public string RoleName { get; set; }

        [Display(Name = "Role Description")]
        [Required(ErrorMessage = "Please enter role description")]
        [StringLength(50)]
        public string Description { get; set; }
        public bool IsRecordDeleted { get; set; }
        public string IPAddress { get; set; }
        //public int UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public DateTime CreateTs { get; set; }

        public int UserId { get; set; }

    }
}
