using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi3elements.Models
{
    public class Device
    {
        [Key]
        public string deviceId { get; set; }
        
        public string name { get; set; }

        public Boolean on { get; set; }
        
        [ForeignKey("Home")]
        public string homeId { get; set; }
        public virtual Home Home { get; set; }
    }
}