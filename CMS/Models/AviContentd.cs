using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    public class AviContentd
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int MID { get; set; }
        
        
        //public string ContentName { get; set; }

        [Required]
        public byte[] AVIContent { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        HttpPostedFileBase ImageUpload { get; set; }

        // [Required]
        public string ContentType { get; set; }

       // public virtual Contentd contend { get; set; }
    }

    public class AviContentView
    {
        public int ID { get; set; }

        public int MID { get; set; }

        //public string ContentName { get; set; }

        public string ContentType { get; set; }
    }

}