using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayout.Models.UserManagement
{
    public class RegistrationModel :BaseEntity
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter user name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Display(Name = "Login ID")]
        [Required(ErrorMessage = "Please enter login id")]
        //[Remote(action: "VerifyLoginId", controller: "User")] remove due to forget password is not working
        [StringLength(50)]
        public string LoginID { get; set; }

        [Display(Name = "Mobile No")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [Required(ErrorMessage = "Please enter mobile")]
        //[Remote(action: "VerifyMobile", controller: "User")]remove due to forget password is not working
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Display(Name = "Email ID")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter email")]
        //[Remote(action: "VerifyEmail", controller: "User")]remove due to forget password is not working
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
        public string IPAddress { get; set; }     
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Role Name")]
        public int RoleId { get; set; }

        [Display(Name = "Institute Name")]
        //[Required(ErrorMessage = "Please enter institute name")]
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public string InstituteCode { get; set; }
        public string IsPasswordChange { get; set; }
        public int ReturnUserId { get; set; }
        public int UserRoleId { get; set; }

        [NotMapped]
        public List<RegistrationModel> InstituteList { get; set; }
        [NotMapped]
        public List<RoleModel> RoleList { get; set; }

        //public List<RoleToRoleMappingModel> RoleToRoleMappingList { get; set; }
        public int? PaperSetterId { get; set; }
        public string PaperSetterName { get; set; }


        //chnage password
        public string OldPassword { get; set; }
        public string OldSalt { get; set; }
        public string OldSaltedHash { get; set; }

        //sms/email record insert
        public string EmailReminder { get; set; }
        public string MobileReminder { get; set; }
        public string Remarks { get; set; }
    }
}