using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class InstituteModel :BaseEntity
    {
        [Key]
        public int InstituteID { get; set; }

        [Display(Name = "Institute Code")]
        [Required(ErrorMessage = "Please enter institute code")]
        [StringLength(5)]
        public string InstituteCode { get; set; }

        [Display(Name = "Institute Name")]
        [Required(ErrorMessage = "Please enter institute name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(100)]
        public string InstituteName { get; set; }

        public int? UniversityID { get; set; }

        [Display(Name = "Institute Type")]
        [Required(ErrorMessage = "Please enter institute type")]
        public int InstituteTypeId { get; set; }

        [Display(Name = "Institute Category")]
        [Required(ErrorMessage = "Please enter institute category")]
        public int InstituteCategoryId { get; set; }

        public int IsMinority { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Please enter address")]
        [StringLength(500)]
        public string Address { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Please enter state")]
        public int StateId { get; set; }

        [Display(Name = "District")]
        [Required(ErrorMessage = "Please enter district")]
        public int DistrictID { get; set; }

        [Display(Name = "Tehsil")]
        [Required(ErrorMessage = "Please enter tehsil")]
        public int TehsilId { get; set; }

        [Display(Name = "Pincode")]
        [Required(ErrorMessage = "Please enter pincode")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pincode.")]
        [StringLength(6)]
        public string Pincode { get; set; }

        [Display(Name = "EmailId")]
        [Required(ErrorMessage = "Please enter emailId")]
        [StringLength(100)]
        [EmailAddress]
        public string EmailId { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please enter mobile number")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNumber { get; set; }

        [Display(Name = "Phone")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Phone { get; set; }

        [Display(Name = "Fax")]
        [RegularExpression(@"^([0-9])$", ErrorMessage = "Invalid Fax")]
        [StringLength(20)]
        public string Fax { get; set; }

        [Display(Name = "IsActive")]
        [Required(ErrorMessage = "Please enter IsActive")]
        public int IsActive { get; set; }

        [Display(Name = "IsAffiliated")]
        [Required(ErrorMessage = "Please enter IsAffiliated")]
        public int IsAffiliated { get; set; }


        [Display(Name = "Group")]
        [StringLength(5)]
        public string GroupId { get; set; }

        [Display(Name = "Web URL")]
        [StringLength(100)]
        public string WebURL { get; set; }

        [Display(Name = "Bank")]
        [StringLength(100)]
        public string BankId { get; set; }

        [Display(Name = "Account No")]
        [StringLength(20)]
        public string AccountNo { get; set; }

        [Display(Name = "IFSC Code")]
        [StringLength(10)]
        public string IFSCCode { get; set; }

        [Display(Name = "Establishment Year")]
        [MaxLength(4)]
        [MinLength(4)]
        [StringLength(4)]
        public string EstablishmentYear { get; set; }


        [Display(Name = "Latitude")]
        [StringLength(20)]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        [StringLength(20)]
        public string Longitude { get; set; }

        [Display(Name = "Chairman Adhaar")]
        [StringLength(12)]
        public string ChairmanAdhaar { get; set; }


        [Display(Name = "Chairman Email")]
        [StringLength(50)]
        [EmailAddress]
        public string ChairmanEmail { get; set; }

        [Display(Name = "Chairman MobileNumber")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string ChairmanMobileNumber { get; set; }

        [Display(Name = "Chairman Name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(50)]
        public string ChairmanName { get; set; }

        [Display(Name = "Registrar Adhaar")]
        [StringLength(12)]
        public string RegistrarAdhaar { get; set; }

        [Display(Name = "Registrar Email")]
        [StringLength(50)]
        [EmailAddress]
        public string RegistrarEmail { get; set; }

        [Display(Name = "Registrar MobileNumber")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string RegistrarMobileNumber { get; set; }

        [Display(Name = "Registrar Name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(50)]
        public string RegistrarName { get; set; }

        [Display(Name = "Director Adhaar")]
        [StringLength(12)]
        [MaxLength(12)]
        [MinLength(12)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Adhar")]
        public string DirectorAdhaar { get; set; }

        [Display(Name = "Director Email")]
        [StringLength(50)]
        [EmailAddress]
        public string DirectorEmail { get; set; }

        [Display(Name = "Director MobileNumber")]
        [StringLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string DirectorMobileNumber { get; set; }

        [Display(Name = "Director Name")]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(50)]
        public string DirectorName { get; set; }

        [Display(Name = "Static IP")]
        [StringLength(20)]
        public string StaticIP { get; set; }

        [Display(Name = "Vision")]
        [StringLength(100)]
        public string Vision { get; set; }

        [Display(Name = "Mission")]
        [StringLength(100)]
        public string Mission { get; set; }

        public List<InstituteTypeModel> InstituteTypeList { get; set; }
        public string InstituteTypeName { get; set; }

        public List<InstituteCategoryModel> InstituteCategoryList { get; set; }
        public string InstituteCategoryName { get; set; }

        public List<CountryModel> CountryList { get; set; }
        public string CountryName { get; set; }
        public List<StateModel> StateList { get; set; }
        public string StateName { get; set; }
        public List<DistrictModel> DistrictList { get; set; }
        public string DistrictName { get; set; }
        public List<TehsilModel> TehsilList { get; set; }
        public string TehsilName { get; set; }
        public string IPAddress { get; set; }

        public int? CourseId { get; set; }

        public string CourseName { get; set; }

        public int? BranchId { get; set; }

        public string BranchName { get; set; }

        public string InstituteCodeWithName { get; set; }
    }


}
