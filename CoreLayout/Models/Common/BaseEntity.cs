using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Common
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public string EncryptedId { get; set; }

        public int ButtonMasterId { get; set; }
        public string IPAddress { get; set; }
        public int IsRecordDeleted { get; set; }

    }
}
