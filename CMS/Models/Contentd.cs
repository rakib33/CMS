using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Contentd
    {
      
        [Key]
        public int ID { get; set; }
       
        [Required]
        public int MID { get; set; }

        [Required]
        public int RowID { get; set; }

        [Required]
        public string Content { get; set; }


        public int AVIContent { get; set; }

        [Required]
        public int GroupID { get; set; }
        public int CssID { get; set; }

        //This is just a model field that is not table field .Schema
        //[NotMapped]
        //public string ClassName { get; set; }

      //  public virtual ICollection<AviContentd> _Avicontentd { get; set; }
       
       }

    public class Content_m_d
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string ClassName { get; set; }

        public int Group_Id { get; set; }
        
        public string SuraName { get; set; }
          
        //aud_ved_img contain ID of aviContentd table and return all content 
        public int aud_ved_img { get; set; }
    }
}