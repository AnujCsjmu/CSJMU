using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayout.Models.PCP
{
    public class PCPRegistrationModel :BaseEntity
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter user name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Display(Name = "Login ID")]
        [Required(ErrorMessage = "Please enter login id")]
        [StringLength(50)]
        public string LoginID { get; set; }

        [Display(Name = "Mobile No")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [Required(ErrorMessage = "Please enter mobile")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Display(Name = "Email ID")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter email")]
        [StringLength(50)]
        public string EmailID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Salt { get; set; }
        public string SaltedHash { get; set; }

        [Display(Name = "IsUser Active")]
        public int IsUserActive { get; set; }

        public int IsRoleActive { get; set; }

        public int IsMainRole { get; set; }
        public string RefID { get; set; }
        public string RefType { get; set; }
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Role Name")]
        public int RoleId { get; set; }

        [Display(Name = "Institute Name")]
        [Required(ErrorMessage = "Please enter institute name")]
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public string IsPasswordChange { get; set; }
        public int ReturnUserId { get; set; }
        public int UserRoleId { get; set; }

     
        [NotMapped]
        public List<RoleModel> RoleList { get; set; }

        [NotMapped]
        public List<QPMasterModel> QPCodeList { get; set; }

        [Display(Name = "QP Code")]
        [Required(ErrorMessage = "Please enter qp code")]
        public int QPId { get; set; }

        public string QPCode { get; set; }

        public string QPName { get; set; }

        [NotMapped]
        public List<CourseModel> CourseList { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Please enter course name")]
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        [NotMapped]
        public List<BranchModel> BranchList { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Please enter subject name")]
        public int BranchID { get; set; }
        public string BranchName { get; set; }

        public List<string> UserList { get; set; }

        public int? IsApproved { get; set; }

        public int? RefUserId { get; set; }
    }
}
