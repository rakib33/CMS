using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CMS.Models;

using Microsoft.AspNet.Identity.EntityFramework;

namespace CMS.App_Code
{

    //public class AppHandler : IdentityUser
    //{
      
      
    //}
    public class DBHandler:DbContext
    {
        public  DBHandler()
            : base("CMSConnection")
        { }


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

              
    }
}