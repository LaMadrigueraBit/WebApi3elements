using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi3elements.DTOs
{
    public class MeasureDto
    {
        public DateTime date { get; set; }
        public float consumption { get; set; }
        public string type { get; set; }
        public string deviceName { get; set; }

    }
}