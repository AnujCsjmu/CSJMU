﻿using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayout.Models.Exam
{
    public class StudentAcademicsModel : BaseEntity
    {
        [Key]
        public int AcademicId { get; set; }

        [Display(Name = "Student")]
        [Required(ErrorMessage = "Please select student")]
        public int StudentID { get; set; }
        public string Name { get; set; }
        public List<StudentModel> StudentList { get; set; }

        [Display(Name = "Institute")]
        [Required(ErrorMessage = "Please select institute")]
        public int InstituteID { get; set; }
        public string InstituteName { get; set; }
        public List<InstituteModel> InstituteList { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select course")]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<CourseModel> CourseList { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please select subject")]
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public List<BranchModel> SubjectList { get; set; }

        [Display(Name = "SemYear")]
        [Required(ErrorMessage = "Please select sem year")]
        public int SemYearId { get; set; }
        public List<BranchModel> SemYearList { get; set; }

        [Display(Name = "Syllabus Session")]
        [Required(ErrorMessage = "Please select syllabus session")]
        public int SyllabusSessionId { get; set; }
        public string SyllabusSessionName { get; set; }
        public List<SessionModel> SyllabusSessionList { get; set; }

        [Display(Name = "Exam Center")]
        [Required(ErrorMessage = "Please select exam center")]
        public int ExamCenterId { get; set; }
        public string ExamCenterName { get; set; }
        public List<InstituteModel> ExamCenterList { get; set; }

        [Display(Name = "Exam Category")]
        public string ExamCategory { get; set; }

        [Display(Name = "Academic Session")]
        [Required(ErrorMessage = "Please select academic session")]
        public int AcademicSessionId { get; set; }
        public string AcademicSessionName { get; set; }
        public List<SessionModel> AcademicSessionList { get; set; }

        [Display(Name = "Exam")]
        [Required(ErrorMessage = "Please enter exam")]
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public List<ExamMasterModel> ExamList { get; set; }

        [Display(Name = "Previous Session")]
        [Required(ErrorMessage = "Please select previous session")]
        public int PreviousSessionId { get; set; }
        public string PreviousSessionName { get; set; }
        public string PreviousResultStatus { get; set; }
        public List<SessionModel> PreviousSessionIdList { get; set; }

        [Display(Name = "Current Exam Month")]
        [Required(ErrorMessage = "Please select current exam month")]
        public int CurrentExamMonth { get; set; }

        [Display(Name = "Old Exam Month")]
        [Required(ErrorMessage = "Please select old exam month")]
        public int OldExamMonth { get; set; }

        public string Batch { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
       
    }
}
