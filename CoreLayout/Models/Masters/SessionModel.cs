using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class SessionModel :BaseEntity
    {
        [Key]
        public int SessionId { get; set; }
        public string Session { get; set; }
        public string Description { get; set; }
    }
}
