using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class PraticeChildModel
    {

        [Key]
        public int Id { get; set; }
        public int PraticeId { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
