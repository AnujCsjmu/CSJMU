using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreLayout.Models.Exam
{
    public class StudentModel : BaseEntity
    {
        [Key]
        public int StudentID { get; set; }

        [Display(Name = "Roll No")]
        [StringLength(20)]
        [Required(ErrorMessage = "Please enter roll no")]
        public string RollNo { get; set; }

        [Display(Name = "Enrolment No")]
        [StringLength(20)]
        [Required(ErrorMessage = "Please enter enrolment no")]
        public string EnrolmentNo { get; set; }

        [Display(Name = "Entrance Application No")]
        [StringLength(20)]
        public string EntranceApplicationNo { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Hindi Name")]
        [Required(ErrorMessage = "Please enter hindi name")]
        [StringLength(100)]
        public string HindiName { get; set; }

        [Display(Name = "Father Name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [Required(ErrorMessage = "Please enter father name")]
        public string FatherName { get; set; }

        [Display(Name = "Mother Name")]
        [StringLength(50)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        public string MotherName { get; set; }

        [Display(Name = "DOB")]
        public string DOB { get; set; }

        //[Display(Name = "Gender")]
        //public string Gender { get; set; }

        [Display(Name = "Mobile")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [StringLength(10)]
        public string Mobile { get; set; }

        [Display(Name = "Parent MobileNo")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [StringLength(10)]
        public string ParentMobileNo { get; set; }

        [Display(Name = "Email Id")]
        [EmailAddress]
        [StringLength(50)]
        public string EmailId { get; set; }

        [Display(Name = "Correspondence  Address")]
        [StringLength(200)]
        public string CAddress { get; set; }

        [Display(Name = "Correspondence  State")]
        public int? CStateId { get; set; }
        public string CStateName { get; set; }
        public List<StateModel> CStateList { get; set; }

        [Display(Name = "Correspondence  District")]
        public int? CDistrictId { get; set; }
        public string CDistrictName { get; set; }
        public List<DistrictModel> CDistrictList { get; set; }

        [Display(Name = "Correspondence  Tehsil")]
        public int? CTehsilId { get; set; }
        public string CTehsilName { get; set; }
        public List<TehsilModel> CTehsilList { get; set; }

        [Display(Name = "Correspondence  PinCode")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pin Code.")]
        public string CPinCode { get; set; }

        [Display(Name = "Permanent  Address")]
        [StringLength(200)]
        public string PAddress { get; set; }

        [Display(Name = "Permanent  State")]
        public int? PStateId { get; set; }
        public string PStateName { get; set; }
        public List<StateModel> PStateList { get; set; }

        [Display(Name = "Permanent  District")]
        public int? PDistrictId { get; set; }
        public string PDistrictName { get; set; }
        public List<DistrictModel> PDistrictList { get; set; }

        [Display(Name = "Permanent  Tehsil")]
        public int? PTehsilId { get; set; }
        public string PTehsilName { get; set; }
        public List<TehsilModel> PTehsilList { get; set; }

        [Display(Name = "Permanent  PinCode")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pin Code.")]
        public string PPinCode { get; set; }

        //[Display(Name = "IdProof Type")]
        //public string IdProofType { get; set; }

        [Display(Name = "IdProof No")]
        [StringLength(12)]
        public string IdProofNo { get; set; }

        [Display(Name = "Category")]
       // public int? CategoryId { get; set; }
        public string Category { get; set; }
        public List<CategoryModel> CategoryList { get; set; }

        [Display(Name = "Is EWS")]
        public bool IsEWS { get; set; }

        [Display(Name = "Is Disability")]
        public bool IsDisability { get; set; }

        //[Display(Name = "Disability Type")]
        //[StringLength(50)]
        //public string DisabilityType { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Blood Group")]
        [StringLength(3)]
        public string BloodGroup { get; set; }

        [Display(Name = "Is Vaccinated")]
        public bool IsVaccinated { get; set; }

        [Display(Name = "Religion")]
        public int? ReligionID { get; set; }
        public string ReligionName { get; set; }
        public List<ReligionModel> ReligionList { get; set; }

        [Display(Name = "Is Minority")]
        public bool IsMinority { get; set; }

        [Display(Name = "Photograph")]
        public IFormFile FUPhotograph { get; set; }
        [Display(Name = "Photograph")]
        public string PhotographPath { get; set; }

        [Display(Name = "Signature")]
        public IFormFile FUSignature { get; set; }
        [Display(Name = "Signature")]
        public string SignaturePath { get; set; }


        //[Display(Name = "Seat Type")]
        //public string SeatType { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Literal")]
        public bool IsLiteral { get; set; }

        [Display(Name = "Reservation Category")]
        public string ReservationCategory { get; set; }

        [Display(Name = "Admission Session")]
        public int AdmissionSessionId { get; set; }
        public string AdmissionSessionName { get; set; }
        public List<SessionModel> AdmissionSessionList { get; set; }


        [Display(Name = "Data Source")]
        public string DataSource { get; set; }

        //reservation category
        public SelectList ReservationCategoryList = new SelectList(new[]
           {
                new SelectListItem { Selected = true ,Text = "--- Select Reservation Category ---", Value = "0" },
                new SelectListItem { Selected = false, Text = "Freedom Fighter", Value = "Freedom Fighter" },
                new SelectListItem { Selected = false, Text = "Others", Value = "Others" },
            }, "Value", "Text");
        //gender
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public SelectList GenderList = new SelectList(new[]
          {
                new SelectListItem { Selected = true ,Text = "--- Select Gender ---", Value = "0" },
                new SelectListItem { Selected = false, Text = "Male", Value = "Male" },
                new SelectListItem { Selected = false, Text = "Female", Value = "Male" },
                new SelectListItem { Selected = false, Text = "Others", Value = "Male" },
            }, "Value", "Text");

        //Id Proof
        [Display(Name = "IdProof Type")]
        public string IdProofType { get; set; }
        public SelectList IdProofTypeList = new SelectList(new[]
          {
                new SelectListItem { Selected = true ,Text = "--- Select IdProof ---", Value = "0" },
                new SelectListItem { Selected = false, Text = "AadharCard", Value = "AadharCard" },
                new SelectListItem { Selected = false, Text = "PanCard", Value = "PanCard" },
                new SelectListItem { Selected = false, Text = "IdentityCard", Value = "IdentityCard" },
                new SelectListItem { Selected = false, Text = "Passport", Value = "Passport" },
            }, "Value", "Text");

        //Id Proof
        [Display(Name = "Seat Type")]
        public string SeatType { get; set; }
        public SelectList SeatTypeList = new SelectList(new[]
          {
                new SelectListItem { Selected = true ,Text = "--- Select SeatType ---", Value = "0" },
                new SelectListItem { Selected = false, Text = "Direct", Value = "Direct" },
                new SelectListItem { Selected = false, Text = "Reservation", Value = "Reservation" },
                new SelectListItem { Selected = false, Text = "JEE", Value = "JEE" },
                new SelectListItem { Selected = false, Text = "Counselling", Value = "Counselling" },
            }, "Value", "Text");

        //Disability type
        [Display(Name = "Disability Type")]
        public string DisabilityType { get; set; }
        public SelectList DisabilityTypeList = new SelectList(new[]
          {
                new SelectListItem { Selected = true ,Text = "--- Select DisabilityType ---", Value = "0" },
                new SelectListItem { Selected = false, Text = "Blindness", Value = "Blindness" },
                new SelectListItem { Selected = false, Text = "Low-vision", Value = "Low-vision" },
                new SelectListItem { Selected = false, Text = "Leprosy Cured Persons", Value = "Leprosy Cured Persons" },
                new SelectListItem { Selected = false, Text = "Hearing Impairment", Value = "Hearing Impairment" },
                new SelectListItem { Selected = false, Text = "Locomotor Disability", Value = "Locomotor Disability" },
                new SelectListItem { Selected = false, Text = "Dwarfism", Value = "Dwarfism" },
                new SelectListItem { Selected = false, Text = "Intellectual Disability", Value = "Intellectual Disability" },
                new SelectListItem { Selected = false, Text = "Mental Illness", Value = "Mental Illness" },
            }, "Value", "Text");
       
    }
}
