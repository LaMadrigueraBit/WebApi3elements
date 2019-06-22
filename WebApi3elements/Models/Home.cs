using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApi3elements.Models
{
    public class Home
    {
        [Key]
        public string homeId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string userId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}