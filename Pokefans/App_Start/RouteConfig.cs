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

#if DEBUG
            routes.Add("FileServer", new CatchAllDomainRoute(
                    "files." + ConfigurationManager.AppSettings["Domain"],
                    url: "{*url}",
                    defaults: new { controller = "StaticFile", action = "Files" }
                ));
            routes.Add("StaticServer", new CatchAllDomainRoute(
                    "files." + ConfigurationManager.AppSettings["Domain"],
                    url: "{*url}",
                    defaults: new { controller = "StaticFile", action = "Static" }
                ));
#endif

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

            routes.Add("TauschIndex", new DomainRoute(
                    ConfigurationManager.AppSettings["Domain"],
                    url: "tausch/",
                    defaults: new { controller = "Trading", action = "Index" }
                ));
            routes.Add("TauschNeu", new DomainRoute(
                    ConfigurationManager.AppSettings["Domain"],
                    url: "tausch/neu",
                    defaults: new { controller = "Trading", action = "AddOffer" }
                ));

			routes.Add("TauschMy", new DomainRoute(
					ConfigurationManager.AppSettings["Domain"],
					url: "tausch/meine-angebote",
					defaults: new { controller = "Trading", action = "My" }
				));
			routes.Add("TauschProtocol", new DomainRoute(
					ConfigurationManager.AppSettings["Domain"],
					url: "tausch/protokoll",
					defaults: new { controller = "Trading", action = "Protocol" }
				));

            routes.Add("TauschDetails", new DomainRoute(
                    ConfigurationManager.AppSettings["Domain"],
                    url: "tausch/{id}",
                    defaults: new { controller = "Trading", action = "Details" }
                ));


            // api routes

            routes.Add("CommentAddApi", new DomainRoute(
                    "api."+ConfigurationManager.AppSettings["Domain"],
                    "v1/comments/add",
                    new { controller = "CommentApi", action = "Add" }
                ));
            routes.Add("CommentHideApi", new DomainRoute(
                    "api."+ConfigurationManager.AppSettings["Domain"],
                    "v1/comments/hide/{id}",
                    new { controller = "CommentApi", action = "Hide" }
                ));
            routes.Add("CommentDeleteApi", new DomainRoute(
                    "api."+ConfigurationManager.AppSettings["Domain"],
                    "v1/comments/delete/{id}",
                    new { controller = "CommentApi", action = "Delete" }
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
