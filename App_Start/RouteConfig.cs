﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Task_Management_Platform
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Team",
              url: "Teams/AdaugaUser/{id}/{id1}",
              defaults: new { controller = "Teams", action = "AdaugaUser", id = UrlParameter.Optional,id1 = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Team",
              url: "Teams/AdaugaUser/{id}/{id1}",
              defaults: new { controller = "Teams", action = "AdaugaUser", id = UrlParameter.Optional, id1 = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
