using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Links
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage="Please select a Application Name.")]
        public int AID { get; set; }
       // public virtual Application Application { get; set; }

        [Required]
        public string Name { get; set; }

        //allow null
        [Required(ErrorMessage = "Please select a Parent Name or null if it is a Parent.")]
        public int Parent { get; set; }

        [Required]
        public string URL { get; set; }

        public string IMG_Path { get; set; }
        
        public Boolean IsParent { get; set; }
        public string Frame { get; set; }

        [Display(Name="Sequence Id")]
        public int SQID { get; set; }

        public Boolean IsActive { get; set; }
    
    }

      

}