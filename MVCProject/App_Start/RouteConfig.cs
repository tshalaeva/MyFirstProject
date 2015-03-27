using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ArticleListing", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ArticleListing",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ArticleListing", action = "OpenDetails", id = UrlParameter.Optional });
        }
    }
}