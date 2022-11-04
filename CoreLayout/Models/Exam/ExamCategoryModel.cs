using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Exam
{
    public class ExamCategoryModel :BaseEntity
    {
        [Key]
        public int ExamCategoryId { get; set; }
        public string ExamCategoryName { get; set; }
        public string ExamCategoryCode { get; set; }
    }
}
