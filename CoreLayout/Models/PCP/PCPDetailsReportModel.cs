using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.PCP
{
    public class PCPDetailsReportModel
    {
        [Display(Name = "Exam Name")]
        public int ExamId { get; set; }
        [Display(Name = "Exam Name")]
        public string  ExamName { get; set; }

        [Display(Name = "Course Name")]
        public int CourseId { get; set; }
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Display(Name = "Subject Name")]
        public int SubjectId { get; set; }
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Display(Name = "Setter Registration Count")]
        public int RegisterCount { get; set; }

        [Display(Name = "Setter Approved Count")]
        public int AppovedCount { get; set; }

        [Display(Name = "QP Assigned Count")]
        public int QPAssignedCount { get; set; }

        [Display(Name = "Upload Paper Count")]
        public int UploadPaperCount { get; set; }

        [Display(Name = "Send to Agency Count")]
        public int SendToPaperAgencyCount { get; set; }

        [Display(Name = "Agency Accepted Count")]
        public int AcceptPaperAgencyCount { get; set; }
    }
}
