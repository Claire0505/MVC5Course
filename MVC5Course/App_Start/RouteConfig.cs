﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5Course
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //17 練習用屬性路由 (Attribute Routing) 來定義自訂網址結構
            routes.MapMvcAttributeRoutes();

            // 示範 ASP.NET MVC 的 Model Binding 過程
            routes.MapRoute(
                    name: "DefaultName",
                    url: "Mbinding/{name}",
                    defaults: new { controller = "MB", action = "Mbinding"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}", 
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[]
                {
                    "MVC5Course.Controllers"
                }
            );
        }
    }
}
