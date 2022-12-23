using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Common
{
    public class MailRequest :MailSettings
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "To Email")]
        [Required(ErrorMessage = "Please enter course")]
        public string ToEmail { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please enter subject")]
        public string Subject { get; set; }

        [Display(Name = "Body")]
        [Required(ErrorMessage = "Please enter body")]
        public string Body { get; set; }

        [Display(Name = "Attachments")]
        //[Required(ErrorMessage = "Please select attachments")]
        public List<IFormFile> MailAttachments { get; set; }
        public IFormFile Attachments { get; set; }
        public IEnumerable<MailRequest> SendMailList { get; set; }

        public bool IsChecked { get; set; }

        public List<SelectListItem> SelectedEmailIds { get; set; }
    }
}
