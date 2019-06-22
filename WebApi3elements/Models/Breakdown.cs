using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi3elements.Models
{
    public class Breakdown
    {
        [Key]
        public int breakdownId { get; set; }
        public DateTime date { get; set; }
        public Boolean solved { get; set; }
        [ForeignKey("Device")]
        public string deviceId { get; set; }
        public virtual Device Device { get; set; }
    }
}