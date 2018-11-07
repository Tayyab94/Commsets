using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace finalProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home1", action = "Index", id = UrlParameter.Optional },

                // this thing is done when we have created the new Area in our project
                //Also change in the Area RouteClass file
                namespaces :new[] { "finalProject.Controllers" } //Namespace of the default controller
            );
        }
    }
}
