using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
         {


            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           

            #region default
           
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //no need to add route under Admin Controller just this 
           routes.MapRoute(
                "default_Admin",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
              );
            
            #endregion


            
            
            
            #region UserRoute
                        
            routes.MapRoute(
                 "UserapiController",
                 "RIP/{controller}/{action}/{id}",
                 new { action = "Index", id = UrlParameter.Optional },
                 new[] { "CMS.Controllers.RIP.UserAPIController" }
                 );

            #endregion

            
        }
    }
}
