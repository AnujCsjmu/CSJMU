using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Exam
{
    public class AssignProfileModel
    {
        [Display(Name = "Paper Code")]
        public string PaperCode { get; set; }

        [Display(Name = "Paper Name")]
        public string PaperName  { get; set; }

        [Display(Name = "Enrollment No")]
        public string EnrollmentNo { get; set; }

        [Display(Name = "Roll No")]
        public string RollNo { get; set; }

        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        [Display(Name = "Seat")]
        public string Seat { get; set; }
    }
}
