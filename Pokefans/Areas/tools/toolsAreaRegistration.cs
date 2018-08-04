// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Pokefans.Util;

namespace Pokefans.Areas.tools
{
    public class toolsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "tools";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            RouteValueDictionary dataTokens = new RouteValueDictionary();
            dataTokens["Namespaces"] = new string[] { "Pokefans.Areas.tools.Controllers" };
            dataTokens["UseNamespaceFallback"] = false;

            context.Routes.Add(new DomainRoute(
                "tools." + ConfigurationManager.AppSettings["Domain"],
                "{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                dataTokens
                )
            );
        }
    }
}