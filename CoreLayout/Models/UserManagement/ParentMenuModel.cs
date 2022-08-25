using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.UserManagement
{
    public class ParentMenuModel : BaseEntity
    {
        [Key]
        public int ParentMenuId { get; set; }

        [Display(Name = "Parent Menu")]
        [Required(ErrorMessage = "Please enter parent menu")]
        public string ParentMenuName { get; set; }

        public int SortBy { get; set; }
        public int IsRecordDeleted { get; set; }

        public string IPAddress { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        //public DateTime ModTs { get; set; }

        //public DateTime CreateTs { get; set; }
    }
}
