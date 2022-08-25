using CoreLayout.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace CoreLayout.Models.UserManagement
{
    public class LoginModel : BaseEntity
    {
        public int UserID { get; set; }
       // public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter login id")]
        [StringLength(50)]
        public string LoginID { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       // public string RoleName { get; set; }

        [Display(Name = "Remember me?")]
        public bool Remember { get; set; }
       public int IsRoleActive { get; set; }
    }
}