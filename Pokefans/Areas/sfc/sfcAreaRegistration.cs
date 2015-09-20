//Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Web.Mvc;
using Pokefans.Util;
using System.Configuration;
using System.Web.Routing;

namespace Pokefans.Areas.sfc
{
    public class sfcAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "sfc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteValueDictionary dataTokens = new RouteValueDictionary();
            dataTokens["Namespaces"] = new string[] { "Pokefans.Areas.sfc.Controllers" };
            dataTokens["UseNamespaceFallback"] = false;

            var route = new DomainRoute(
                            "sfc." + ConfigurationManager.AppSettings["Domain"],
                            "{controller}/{action}/{id}",
                            new { action = "Index", controller = "SfcHome", id = UrlParameter.Optional },
                            dataTokens
                        );

            context.Routes.Add("sfcarcrypt", new DomainRoute(
                    "sfc." + ConfigurationManager.AppSettings["Domain"],
                    "tools/arcrypt",
                    new { action = "ArCrypt", controller = "SfcHome" }/*,
                    dataTokens*/
                ));


            // Default Route
            context.Routes.Add("sfcdefault", route);
        }
    }
}

