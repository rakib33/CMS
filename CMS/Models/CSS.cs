using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class CSS
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Insert a css tag Name from arrivoDSstyle.css for tr & td tag!")]
        public string ClassName { get; set; }

        [Required]
        public string StyleSheetName { get; set; }

    }
}