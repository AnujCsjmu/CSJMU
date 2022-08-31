using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class CourseTypeModel :BaseEntity
    {
        [Key]
        public int CourseTypeId { get; set; }

        public string CourseTypeName { get; set; }

        public string CourseTypeHindi { get; set; }

        public string Description { get; set; }
    }
}
