using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Pokefans.Util;

namespace Pokefans.Areas.forum
{
    public class forumAreaRegistration : AreaRegistration
    {
		public override string AreaName
		{
			get
			{
				return "forum";
			}
		}

        public override void RegisterArea(AreaRegistrationContext context) {

            RouteValueDictionary dataTokens = new RouteValueDictionary();
            dataTokens["Namespaces"] = new string[] { "Pokefans.Areas.forum.Controllers" };
            dataTokens["UseNamespaceFallback"] = false;
            var route = new DomainRoute(
                            "forum." + ConfigurationManager.AppSettings["Domain"],
                            "{controller}/{action}/{id}",
                            new { action = "Index", controller = "BoardIndex", id = UrlParameter.Optional },
                            dataTokens
                        );

            context.Routes.Add("forumIndex", new DomainRoute(
                        "forum." + ConfigurationManager.AppSettings["Domain"],
                        "",
                        new { action = "Index", controller = "BoardIndex" }
            ));

            context.Routes.Add("forumNewThread", new DomainRoute(
                "forum." + ConfigurationManager.AppSettings["Domain"],
                "{url}/neu.html",
                new {action = "NewThread", controller="Forum" }
            ));

            // Maybe we need to do a forum route which is more specific to
            // what exists in the Database and what does not.
            context.Routes.Add("forumViewForum", new DomainRoute(
                "forum." + ConfigurationManager.AppSettings["Domain"],
                "{url}",
                new { action = "ViewForum", controller = "Forum", url = UrlParameter.Optional }
            ));

            context.Routes.Add("forumdefault", route);
        }

    }
}
