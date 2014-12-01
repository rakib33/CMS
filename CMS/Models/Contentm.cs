using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CMS.Models
{
    public class Contentm
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int LinksID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }


    }
}