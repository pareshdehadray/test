using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Default1",
               url: "{controller}/{action}/{type}/{value}",
               defaults: new { controller = "Home", action = "Index" }
           );

            routes.MapRoute(
               name: "Default2",
               url: "{controller}/{action}/{region}/{country}/{city}/{office}/{graphType}/{Period}/{StartDate}/{EndDate}",
               defaults: new { controller = "Home", action = "Index" }
           );
        }
    }
}
