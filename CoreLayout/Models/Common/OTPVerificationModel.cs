using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Common
{
    public class OTPVerificationModel : BaseEntity
    {
        [Key]
        public int UniqueId { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string RequestedOn { get; set; }
        public string ValidUpto { get; set; }
        public string OTP { get; set; }
        public bool IsVerified { get; set; }
        public string GeoLocation { get; set; }
    }
}
