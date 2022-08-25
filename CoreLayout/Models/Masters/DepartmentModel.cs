using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class DepartmentModel:BaseEntity
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
       // public string Status { get; set; }
    }
}
