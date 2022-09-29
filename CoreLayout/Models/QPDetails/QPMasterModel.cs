using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.QPDetails
{
    public class QPMasterModel :BaseEntity
    {


        [Key]
        public int QPId { get; set; }

        [Display(Name = "QP Code")]
        [Required(ErrorMessage = "Please enter qp code")]
        //[RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        //[Remote(action: "VerifyName", controller: "QPMaster")]
        [StringLength(50)]
        public string QPCode { get; set; }

        [Display(Name = "QP Name")]
        [Required(ErrorMessage = "Please enter qp name")]
        //[RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Use onle character")]
        [StringLength(50)]
        public string QPName { get; set; }

        [Display(Name = "QP Hindi Name")]
        //[Required(ErrorMessage = "Please enter qp hindi name")]
        [StringLength(50)]
        public string QPHindiName { get; set; }

        [Display(Name = "QP Type")]
        [Required(ErrorMessage = "Please select qp type")]
        public int QPTypeId { get; set; }

        [Display(Name = "Faculty")]
         [Required(ErrorMessage = "Please select faculty")]
        public int FacultyId { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select course")]
        public int CourseId { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please select subject")]
        public int BranchID { get; set; }

        [Display(Name = "Subject Type")]
        [Required(ErrorMessage = "Please select subject type")]
        //[StringLength(2)]
        public string SubjectType { get; set; }

        [Display(Name = "OMR Code")]
        public string OMRCode { get; set; }

        [Display(Name = "Semester/Year")]
        [Required(ErrorMessage = "Please select Semester year")]
        public int SemYearId { get; set; }

        [Required(ErrorMessage = "Please select is elective")]
        public int IsElective { get; set; }

        [Required(ErrorMessage = "Please enter credits")]
        public string Credits { get; set; }


        [Display(Name = "Internal Marks")]
        [Required(ErrorMessage = "Please enter internal marks")]
        public int? InternalMarks { get; set; }

        [Display(Name = "External Marks")]
        [Required(ErrorMessage = "Please enter external marks")]
        public int? ExternalMarks { get; set; }

        [Required(ErrorMessage = "Please select is single faculty")]
        public int IsSingleFaculty { get; set; }

        [Display(Name = "Main Group")]
        public string MainGroup { get; set; }

        [Display(Name = "Sub Group")]
        public string SubGroup { get; set; }

        [Display(Name = "No Of Paper")]
        public int? NoOfPaper { get; set; }
        public int? IsGrade { get; set; }

        [Display(Name = "Grade")]
        public int? GradeId { get; set; }

        [Display(Name = "Syllabus")]
        [Required(ErrorMessage = "Please select syllabus")]
        public int SyllabusId { get; set; }

        [Display(Name = "Clubing Code")]
        public string ClubingCode { get; set; }
        public int? IsQualifyingPaper { get; set; }

        [NotMapped]
        public List<FacultyModel> FacultyList { get; set; }
        public string FacultyName { get; set; }
        [NotMapped]
        public List<CourseModel> CourseList { get; set; }
        public string CourseName { get; set; }
        [NotMapped]
        public List<BranchModel> BranchList { get; set; }
        public string BranchName { get; set; }//note branch table is used as subjectname
        [NotMapped]
        public List<SessionModel> SessionList { get; set; }
        public string SyllabusName { get; set; }//not session table is used as syllabus name
        [NotMapped]
        public List<QPTypeModel> QPTypeList { get; set; }
        public string QPTypeName { get; set; }
        [NotMapped]
        public List<GradeDefinitionModel> GradeList { get; set; }//used grade definition table
        public string GradeName { get; set; }


        public List<QPMasterModel> QPListForGrid { get; set; }



    }
}
