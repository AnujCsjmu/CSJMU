using CoreLayout.Models.Common;
using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.PCP
{
    public class PCPAssignedQPModel :BaseEntity
    {
        [Key]
        public int AssignedQPId { get; set; }

        [Display(Name = "QP Name")]
        [Required(ErrorMessage = "Please enter qp name")]
        //[StringLength(50)]
        public int QPId { get; set; }
        public List<QPMasterModel> QPList { get; set; }
        public string QPName { get; set; }
        public string QPCode { get; set; }

        [Display(Name = "Paper Setter Name")]
        [Required(ErrorMessage = "Please enter paper setter name")]
       // [StringLength(50)]
        public int UserId { get; set; }
        public List<int> UserList { get; set; }
        public string UserName { get; set; }

        public int? PCPRegID { get; set; }
        public List<List<PCPAssignedQPModel>> QPListForGrid { get; set; }

        //display qp details
        public string QPCreatedDate { get; set; }
        public string QPCreatedBy { get; set; }
    }
}
