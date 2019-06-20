using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi3elements.DTOs
{
    public class DeviceDto
    {
        public string deviceId { get; set; }
        public string name { get; set; }
        public Boolean on { get; set; }
        public string homeId { get; set; }
    }
}