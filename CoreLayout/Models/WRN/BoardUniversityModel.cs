using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.WRN
{
    public class BoardUniversityModel : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string University { get; set; }
        public string Type { get; set; }
    }
}
