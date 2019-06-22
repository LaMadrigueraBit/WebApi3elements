using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi3elements.DTOs
{
    public class BreakdownDto
    {
        public DateTime date { get; set; }
        public Boolean solved { get; set; }
        public string deviceName { get; set; }
    }
}