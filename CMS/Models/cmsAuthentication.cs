using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CMS.Models
{
    public class cmsAuthentication
    {
    }
    //public class CmsUser : IdentityUser
    //{
    //    //public async Task<ClaimsIdentity>
    //    //    GenerateUserIdentityAsync(UserManager<CmsUser> manager)
    //    //{
    //    //    var userIdentity = await manager
    //    //        .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //    //    return userIdentity;
    //    //}

    //    //public DateTime ExpireDate { set; get; }

    //    //public string Address { get; set; }
    //    //public string City { get; set; }
    //    //public string State { get; set; }

    //    //// Use a sensible display name for views:
    //    //[Display(Name = "Postal Code")]
    //    //public string PostalCode { get; set; }

    //    //// Concatenate the address info for display in tables and such:
    //    //public string DisplayAddress
    //    //{
    //    //    get
    //    //    {
    //    //        string dspAddress = string.IsNullOrWhiteSpace(this.Address) ? "" : this.Address;
    //    //        string dspCity = string.IsNullOrWhiteSpace(this.City) ? "" : this.City;
    //    //        string dspState = string.IsNullOrWhiteSpace(this.State) ? "" : this.State;
    //    //        string dspPostalCode = string.IsNullOrWhiteSpace(this.PostalCode) ? "" : this.PostalCode;
    //    //        return string.Format("{0} {1} {2} {3}", dspAddress, dspCity, dspState, dspPostalCode);
    //    //    }
    //    //}
    //}

    //public class ApplicationRole : IdentityRole
    //{
    //     public ApplicationRole() : base() { }

    //     public ApplicationRole(string name) : base(name) { }

    //    public string Description { get; set; }
    //}

   //previous method by akib..
  // public class MCMS_DBContext:IdentityDbContext<IdentityUser>
    public class MCMS_DBContext : IdentityDbContext<IdentityUser>
    {
        public MCMS_DBContext()
            : base("CMSConnection")
            {
            }


        public DbSet<Links> objLinks { get; set; }
        public DbSet<Application> objApplication { get; set; }
        // public DbSet<mcms_userroles> objUserRoles { get; set; }
        public DbSet<Contentm> objContentm { get; set; }
        public DbSet<Contentd> objContentd { get; set; }
        public DbSet<CSS> objCSS { get; set; }

        public DbSet<AviContentd> Avicontetds { get; set; }


        // public DbSet<mcms_soap_request> soapRequest { get; set; }
        public DbSet<mcms_SubcriptionCheck> objCheckSubcription { set; get; }
        // public DbSet<mcms_soap_response> soapResponse { get; set; }
        public DbSet<row_details> rowdetails { get; set; }

        public DbSet<SOAP_Log> SOAP_Log { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        //protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<IdentityUser>().ToTable("mcms_users");
        //    modelBuilder.Entity<IdentityRole>().ToTable("mcms_roles");
        //    modelBuilder.Entity<IdentityUserRole>().ToTable("mcms_userroles");
        //    modelBuilder.Entity<IdentityUserLogin>().ToTable("mcms_userlogins");
        //    modelBuilder.Entity<IdentityUserClaim>().ToTable("mcms_userclaims");



        //}
       
    }
}