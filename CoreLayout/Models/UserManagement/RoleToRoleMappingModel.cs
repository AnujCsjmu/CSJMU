using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.UserManagement
{
    public class RoleToRoleMappingModel : BaseEntity
    {
        [Key]
        public int RoleMappingId { get; set; }

        [Display(Name = "From Role Name")]
        [Required(ErrorMessage = "Please enter from role")]
        public int FromRoleId { get; set; }
        public string FromRoleName { get; set; }

        public List<RoleModel> FromRoleList { get; set; }


        [Display(Name = "To Role Name")]
        [Required(ErrorMessage = "Please enter to role")]
        public int ToRoleId { get; set; }
        public string ToRoleName { get; set; }

        public List<int> ToRoleList { get; set; }
    }
}
