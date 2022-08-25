using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Masters
{
    public class ButtonModel:BaseEntity
    {
        [Key]
        public int ButtonId { get; set; }

        [Display(Name = "Button Name")]
        [Required(ErrorMessage = "Please enter button name")]
        public string ButtonName { get; set; }
        public int IsRecordDeleted { get; set; }
        public string IPAddress { get; set; }

        public string isChecked { get; set; }

    }
}
