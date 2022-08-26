using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class InstituteTypeModel :BaseEntity
    {
        [Key]
        public int InstituteTypeID { get; set; }
        public string InstituteTypeName { get; set; }

        public string Description { get; set; }

        public string IPAddress { get; set; }

        public int UserId { get; set; }
    }
}
