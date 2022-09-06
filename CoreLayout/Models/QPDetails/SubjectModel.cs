using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.QPDetails
{
    public class SubjectModel :BaseEntity
    {
        public int SubjectID { get; set; }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }
    }
}
