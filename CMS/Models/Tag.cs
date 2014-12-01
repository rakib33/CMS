using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Tag
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string ViewName { get; set; }
    }
}