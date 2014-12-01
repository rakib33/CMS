using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using CMS.App_Code;

namespace CMS.Models
{
    
 public class RoleViewModel
    {
        public string Id { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        public string Description { get; set; }

        public RoleDetailsView rdv { get; set; }
        public virtual IList<RoleDetailsView> role_details_view { get; set; }
    }

 
 public class RoleDetailsView
 {
     public int Id { get; set; }

     [NotMapped]
     public string ControllerName { get; set; }
     
     [NotMapped]
     public string ActionName { get; set; }
     public bool IsAccess { get; set; }

     public string RoleId { get; set; }
     public virtual RoleViewModel roles { get; set; }
 }

 public class EditUserViewModel
 {
     public string Id { get; set; }

     [Required]
     [Display(Name = "User Name")]
     public string UserName { get; set; }

     [Required(AllowEmptyStrings = false)]
     [Display(Name = "Email")]
     [EmailAddress]
     public string Email { get; set; }

     // Add the Address Info:
     //public string Address { get; set; }
     //public string City { get; set; }
     //public string State { get; set; }

     ////// Use a sensible display name for views:
     //[Display(Name = "Postal Code")]
     //public string PostalCode { get; set; }
      
      public IEnumerable<SelectListItem> RolesList { get; set; }

 }

 public class row_details
 {
     public int Id { get; set; }
     public string RoleId { get; set; }
     public string ControllerName { get; set; }
     public string ActionName { get; set; }
     public bool IsAccess { get; set; }
 }

 public class mcms_users : IdentityUser
 {

 }

 public class mcms_userroles 
 {
     [Key,Column(Order=1)]
     public string UserId { get; set; }

     [Key,Column(Order=2)]
     public string RoleId { get; set; }
 }

 public class mcms_roles:IdentityRole
 { 
 }
}