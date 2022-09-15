using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayout.Models.PCP
{
    public class PCPRegistrationModel :BaseEntity
    {
        [Key]
        public int PCPRegID { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter user name")]
        [StringLength(50)]
        public string UserName { get; set; }

        //[Display(Name = "Login ID")]
        //[Required(ErrorMessage = "Please enter login id")]
        //[StringLength(50)]
        public string LoginID { get; set; }

        [Display(Name = "Mobile No")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [Required(ErrorMessage = "Please enter mobile")]
        [Remote(action: "VerifyMobile", controller: "PCPRegistration")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Display(Name = "Email ID")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter email")]
        [StringLength(50)]
        [Remote(action: "VerifyEmail", controller: "PCPRegistration")]
        public string EmailID { get; set; }

        //[Display(Name = "Password")]
        //[Required(ErrorMessage = "Please enter password")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "Confirm Password")]
        //[Required(ErrorMessage = "Confirmation Password is required.")]
        //[Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        //[DataType(DataType.Password)]
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

        //[Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Role Name")]
        public int RoleId { get; set; }

        [Display(Name = "Institute Name")]
        [Required(ErrorMessage = "Please enter institute name")]
        //public int InstituteId { get; set; }
        public string InstituteName { get; set; }

        [Display(Name = "Institute Code")]
        [Required(ErrorMessage = "Please enter institute code")]
        public string InstituteCode { get; set; }

        public string IsPasswordChange { get; set; }
        public int ReturnUserId { get; set; }
        public int UserRoleId { get; set; }

     
        [NotMapped]
        public List<RoleModel> RoleList { get; set; }

        [NotMapped]
        public List<InstituteModel> InstituteList { get; set; }

        [NotMapped]
        [Display(Name = "QP Code")]
        public List<QPMasterModel> QPCodeList { get; set; }

        [Display(Name = "QP Code")]
        [Required(ErrorMessage = "Please enter qp code")]
        public int QPId { get; set; }

        [Display(Name = "QP Code")]
        public string QPCode { get; set; }

        public string QPName { get; set; }

        [NotMapped]
        public List<CourseModel> CourseList { get; set; }

        [Display(Name = "Course Name")]
        [Required(ErrorMessage = "Please enter course name")]
        public int CourseID { get; set; }

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [NotMapped]
        public List<CourseBranchMappingModel> BranchList { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "Please enter subject name")]
        public int BranchId { get; set; }


        [Display(Name = "Subject Name")]
        public string BranchName { get; set; }

        [Display(Name = "Subject Code")]
        public string BranchCode { get; set; }
        public List<string> UserList { get; set; }

        public int? IsApproved { get; set; }

        public int? IsApprovedBy { get; set; }

        public int? UserId { get; set; }

        public string IsEmailReminder { get; set; }
        public string IsMobileReminder { get; set; }


        [Display(Name = "Father's Name")]
        [Required(ErrorMessage = "Please enter father name")]
        [StringLength(50)]
        public string FatherName { get; set; }

        [Display(Name = "Full Address")]
        [Required(ErrorMessage = "Please enter full address")]
        [StringLength(200)]
        public string Address { get; set; }

        [Display(Name = "Teaching Experience in Year")]
        [Required(ErrorMessage = "Please enter teaching experience")]
        public int? TeachingExp { get; set; }

        [Display(Name = "Aadhar")]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid valid aadhar.")]
        [Required(ErrorMessage = "Please enter aadhar")]
        [StringLength(12)]
        [Remote(action: "VerifyAadhar", controller: "PCPRegistration")]
        public string Aadhar { get; set; }

        [Display(Name = "PAN")]
        [StringLength(12)]
        [Required(ErrorMessage = "Please enter pan")]
        [Remote(action: "VerifyPan", controller: "PCPRegistration")]
        public string PAN { get; set; }

        [Display(Name = "Bank Name")]
        [Required(ErrorMessage = "Please enter bank name")]
        [StringLength(50)]
        public string BankName { get; set; }

        [Display(Name = "Bank IFSC Code")]
        [Required(ErrorMessage = "Please enter ifsc code")]
        [StringLength(15)]
        public string IFSC { get; set; }

        [Display(Name = "Bank Account No")]
        [Required(ErrorMessage = "Please enter account no")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid account no.")]
        public string AccountNo { get; set; }

        [Display(Name = "Bank Branch address")]
        [Required(ErrorMessage = "Please enter branch address")]
        [StringLength(50)]
        public string BranchAddress { get; set; }


        [Display(Name = "Type of Employeement")]
        [Required(ErrorMessage = "Please enter type of employeement")]
        public string TypeOfEmployeement { get; set; }

        [Display(Name = "Paper Setting Experience")]
        [Required(ErrorMessage = "Please enter paper setting experience")]
        public int? PaperSettingExp { get; set; }

        //[Display(Name = "Collage Details")]
        //[Required(ErrorMessage = "Please enter collage details")]
        //public string CollageDetails { get; set; }

        //[Display(Name = "Subject")]
        //[Required(ErrorMessage = "Please enter subject")]
        //public string Subject { get; set; }

        //[Display(Name = "Course")]
        //[Required(ErrorMessage = "Please enter course")]
        //public string Course { get; set; }

        [Display(Name = "Specilization")]
        [Required(ErrorMessage = "Please enter specilization")]
        public string Specilization { get; set; }

        public string Remarks { get; set; }

        public string UploadFileName { set; get; }

        [Required(ErrorMessage = "Please choose photo")]
        [Display(Name = "Photo")]
        public IFormFile ProfileImage { get; set; }


        //for send reminder view 
        [Display(Name = "Paper")]
        public string PaperPath { set; get; }

        [Display(Name = "Paper Created By")]
        public string PaperCreatedBy { get; set; }

        [Display(Name = "Paper Created Date")]
        public string PaperCreatedDate { get; set; }

        public int? PaperId { get; set; }
        public string Session { get; set; }

        public string SessionType { get; set; }

        [Display(Name = "Paper Pwd")]
        public string PaperPassword { get; set; }


    }
}
