using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.WRN
{
    public class WRNPaymentModel : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string RegistrationNo { get; set; }
        public int PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string ChallanNo { get; set; }
        public string TransactionNo { get; set; }
        public string BankName { get; set; }

        public List<WRNPaymentModel> DataList { get; set; }
    }
}
