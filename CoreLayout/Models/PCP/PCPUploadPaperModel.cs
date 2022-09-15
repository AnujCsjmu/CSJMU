using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.PCP
{
    public class PCPUploadPaperModel:BaseEntity
    {
        [Key]
        public int PaperId { get; set; }

        [Display(Name = "Paper Code")]
        [Required(ErrorMessage = "Please enter paper code")]
        [StringLength(50)]
        public string PaperCode { get; set; }

        [Display(Name = "Paper Name")]
        [Required(ErrorMessage = "Please enter paper name")]
        [StringLength(100)]
        public string PaperName { get; set; }

        [Display(Name = "Paper Hindi Name")]
        public string PaperHindiName { get; set; }

        [Display(Name = "Paper Password")]
        [Required(ErrorMessage = "Please enter paper password")]
        [StringLength(50)]
        public string PaperPassword { get; set; }

        [Display(Name = "Paper")]
        public string PaperPath { set; get; }

        [Required(ErrorMessage = "Please choose paper")]
        [Display(Name = "Upload Paper")]
        public IFormFile UploadPaper { get; set; }


        [Required(ErrorMessage = "Please enter session")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }
        public string Session { get; set; }
        public List<SessionModel> SessionList { get; set; }

        public List<PCPAssignedQPModel> QPList { get; set; }
        public string SessionType { get; set; }

        public string UserName { get; set; }

        [Display(Name = "QP Name")]
        [Required(ErrorMessage = "Please enter qp name")]
        public int QPId { get; set; }
        public string QPName { get; set; }
        public string QPCode { get; set; }

    }
}
