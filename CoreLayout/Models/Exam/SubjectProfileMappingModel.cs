using CoreLayout.Models.Common;
using CoreLayout.Models.Masters;
using CoreLayout.Models.QPDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Exam
{
    public class SubjectProfileMappingModel : BaseEntity
    {
        [Key]
        public int SubjectProfileMapId { get; set; }
        //public int SubjectProfileId { get; set; }

        [Display(Name = "QP Code")]
        [Required(ErrorMessage = "Please enter qp code")]
        public int QPId { get; set; }
        public string QPCode { get; set; }
        public List<QPMasterModel> QPList { get; set; }

        [Display(Name = "QP Type")]
        [Required(ErrorMessage = "Please enter qp type")]
        public int QPType { get; set; }

        [Display(Name = "Subject Code")]
        [Required(ErrorMessage = "Please enter subject code")]
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public List<BranchModel> SubjectList { get; set; }

        [Display(Name = "Subject Category")]
        [Required(ErrorMessage = "Please enter subject category")]
        public int SubjectCategory { get; set; }
    }
}
