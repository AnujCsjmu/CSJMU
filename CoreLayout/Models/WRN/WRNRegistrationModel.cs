using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.WRN
{
    public class WRNRegistrationModel : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Mode Of Admission")]
        [Required(ErrorMessage = "Please select mode of admission")]
        public string ModeOfAdmission { get; set; }

        [Display(Name = "Application No.")]
        //[Required(ErrorMessage = "Please enter application no")]
        [StringLength(20)]
        public string ApplicationNo { get; set; }

        [Display(Name = "Registration No.")]
        [Required(ErrorMessage = "Please enter registration no")]
        [StringLength(15)]
        public string RegistrationNo { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        //[Required(ErrorMessage = "Please enter first name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        //[Required(ErrorMessage = "Please enter last name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        public string LastName { get; set; }

        [Display(Name = "Hindi Name")]
        public string HindiName { get; set; }

        [Display(Name = "Father's Name")]
        [Required(ErrorMessage = "Please enter father name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        public string FatherName { get; set; }

        [Display(Name = "Mother's Name")]
        [Required(ErrorMessage = "Please enter mother name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        public string MotherName { get; set; }

        [Display(Name = "Mobile No")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [Required(ErrorMessage = "Please enter mobile no")]
        //[Remote(action: "VerifyMobile", controller: "PCPRegistration")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Display(Name = "Mobile Verification Code")]
        [Required(ErrorMessage = "Please enter mobile verification code")]
        [StringLength(6)]
        public string MobileVerificationCode { get; set; }

        [Display(Name = "Email Id")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter email id")]
        [StringLength(50)]
        //[Remote(action: "VerifyEmail", controller: "PCPRegistration")]
        public string EmailId { get; set; }

        [Display(Name = "Email Verification Code")]
        [Required(ErrorMessage = "Please enter verification code")]
        [StringLength(6)]
        public string EmailVerificationCode { get; set; }

        [Display(Name = "Aadhar")]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid valid aadhar.")]
        [Required(ErrorMessage = "Please enter aadhar")]
        [StringLength(12)]
        //[Remote(action: "VerifyAadhar", controller: "PCPRegistration")]
        public string AadharNumber { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please select date of birth")]
        public string DOB { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select category")]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public List<CategoryModel> CategoryList { get; set; }

        [Display(Name = "Nationality")]
        [Required(ErrorMessage = "Please select nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Religion")]
        [Required(ErrorMessage = "Please select religion")]
        public int ReligionId { get; set; }

        [Display(Name = "Religion")]
        public string ReligionName { get; set; }
        public List<ReligionModel> ReligionList { get; set; }

        [Display(Name = "Physical Disabled")]
        [Required(ErrorMessage = "Please select physical disabled")]
        public string PhysicalDisabled { get; set; }

        [Display(Name = "Permanent Address")]
        [Required(ErrorMessage = "Please enter permanent address")]
        [StringLength(200)]
        public string PermanentAddress { get; set; }

        [Display(Name = "Permanent State")]
        [Required(ErrorMessage = "Please select permanent state")]
        public int PermanentStateId { get; set; }

        [Display(Name = "Permanent State")]
        public string PermanentStateName { get; set; }
        public List<StateModel> StateList { get; set; }

        [Display(Name = "Permanent District")]
        [Required(ErrorMessage = "Please select permanent district")]
        public int PermanentDistrictId { get; set; }

        [Display(Name = "Permanent District")]
        public string PermanentDistrictName { get; set; }
        public List<DistrictModel> DistrictList { get; set; }

        [Display(Name = "Permanent Pincode")]
        [Required(ErrorMessage = "Please enter permanent pincode")]
        [StringLength(6,MinimumLength =6)]
        public string PermanentPincode { get; set; }

        [Display(Name = "Communication Address")]
        [Required(ErrorMessage = "Please enter communication address")]
        [StringLength(200)]
        public string CommunicationAddress { get; set; }

        [Display(Name = "Communication State")]
        [Required(ErrorMessage = "Please select communication state")]
        public int CommunicationStateId { get; set; }

        [Display(Name = "Communication State")]
        public string CommunicationStateName { get; set; }

        [Display(Name = "Communication District")]
        [Required(ErrorMessage = "Please select communication district")]
        public int CommunicationDistrictId { get; set; }

        [Display(Name = "Communication District")]
        public string CommunicationDistrictName { get; set; }

        [Display(Name = "Communication Pincode")]
        [Required(ErrorMessage = "Please enter communication pincode")]
        [StringLength(6, MinimumLength = 6)]
        public string CommunicationPincode { get; set; }

        [Display(Name = "I accept the above terms and conditions.")]
        [CheckBoxRequired(ErrorMessage = "Please accept the terms and condition.")]
        public bool TermsConditions { get; set; }
        public bool IsActive { get; set; }

        [Display(Name = "I hereby declare that all the information provided by me is correct. The University will not be responsible for any incorrect information provided by me.")]
        [CheckBoxRequired(ErrorMessage = "Please accept the terms and condition.")]
        public bool FinalSubmit { get; set; }

        [Display(Name = "Academic Session")]
        [Required(ErrorMessage = "Please enter academic session")]
        [StringLength(10)]
        public string AcademicSession { get; set; }

        //upload photo and signature
        [Display(Name = "Photo")]
        [Required(ErrorMessage = "Please upload photo")]
        public string PhotoPath { get; set; }

        [Display(Name = "Photo")]
        [Required(ErrorMessage = "Please upload photo")]
        public IFormFile Photo { get; set; }

        [Display(Name = "Signature")]
        [Required(ErrorMessage = "Please upload Signature")]
        public string SignaturePath { get; set; }

        [Display(Name = "Signature")]
        [Required(ErrorMessage = "Please upload Signature")]
        public IFormFile Signature { get; set; }
        public DateTime? PhotoDate { get; set; }
        public DateTime? SignatureDate { get; set; }
        public int? PrintStatus { get; set; }

        public List<WRNRegistrationModel> WRNUploadDataList { get; set; }
    }
    public class CheckBoxRequired : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }

            return false;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val-checkboxrequired", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        }
    }
}

