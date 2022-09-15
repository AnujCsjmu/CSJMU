using CoreLayout.Models.Common;
using CoreLayout.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.PCP
{
    public class PCPSendPaperModel :BaseEntity
    {
        [Key]
        public int SendPaperId { get; set; }

        [Display(Name = "Agency Name")]
        [Required(ErrorMessage = "Please enter agency name")]
        
        public int PaperId { get; set; }
        public string PaperName { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter paper name")]
        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<PCPUploadPaperModel> PaperList { get; set; }

        public List<RegistrationModel> AgencyList { get; set; }
    }
}
