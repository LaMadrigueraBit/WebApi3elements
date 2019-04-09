using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi3elements.Models
{
    public class Measure
    {
        [Key]
        public int measureId { get; set; }
        public DateTime date { get; set; }
        public float consumption { get; set; }
        public string type { get; set; }
    }
}