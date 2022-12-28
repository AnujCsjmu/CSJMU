using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Models.Common
{
    public class SequenceGenerateModel :BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string SequenceFor { get; set; }
        public string Prefix { get; set; }
        public int SeqLength { get; set; }
        public string Sample { get; set; }
        public int CurrentCount { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
