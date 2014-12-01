﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
             name: "GetmethodApi",
             routeTemplate: "api/{controller}/{action}/{id}",
             defaults: new { controller = "UserAPI", action = "GetAllLatestLinks", id = RouteParameter.Optional }
                );
            
                                 

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Below code for return Json type data from all api Controller method

            //config.Routes.MapHttpRoute(
            //    name: "ApiByUserName",
            //    routeTemplate: "api/{controller}/{action}/{name}",
            //    defaults: null
            //    , constraints: new { name = @"^[a-zA-Z0-9]+$" }
            //);

            //var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
