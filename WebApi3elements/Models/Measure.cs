﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApi3elements.Models
{
    public class Measure
    {
        [Key]
        public int measureId { get; set; }

        public DateTime date { get; set; }
        
        public float consumption { get; set; }
        
        public string type { get; set; }

        [ForeignKey("Device")]
        public string deviceId { get; set; }
        public virtual Device Device { get; set; }

        [ForeignKey("ApplicationUser")]
        public string userId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}