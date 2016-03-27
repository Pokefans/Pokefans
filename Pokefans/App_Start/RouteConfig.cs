// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Pokefans.Util;

namespace Pokefans
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("glimpse.axd");

            routes.Add("Home", new DomainRoute(
                    ConfigurationManager.AppSettings["Domain"],
                    url: "",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                ));

            routes.Add("News", new DomainRoute(
                    ConfigurationManager.AppSettings["Domain"],
                    url: "news/{name}/{contentId}",
                    defaults: new { controller = "Home", action = "ViewContent", name = UrlParameter.Optional }
                ));

            // Default route for handling content urls
            routes.Add("Content", new ContentRoute(
                    "Home", "ViewContent"
                ));

            var route = routes.MapRoute(
                            name: "Default",
                            url: "{controller}/{action}/{id}",
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                            namespaces: new[] { "Pokefans.Controllers" }  
                        );
            route.DataTokens["UseNamespaceFallback"] = false;
        }
    }
}
