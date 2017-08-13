// Copyright 2015-2016 the pokefans authors. See copying.md for legal info.
using System.Configuration;
using System.Web.Mvc;
using Pokefans.Util;
using System.Web.Routing;

namespace Pokefans.Areas.user
{
    public class userAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "user";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteValueDictionary dataTokens = new RouteValueDictionary();
            dataTokens["Namespaces"] = new string[] { "Pokefans.Areas.user.Controllers" }; 
            dataTokens["UseNamespaceFallback"] = false;
            var route = new DomainRoute(
                            "user." + ConfigurationManager.AppSettings["Domain"],
                            "{controller}/{action}/{id}",
                            new { action = "Index", controller = "Home", id = UrlParameter.Optional }, 
                            dataTokens
                        );


            context.Routes.Add(new DomainRoute(
                "user." + ConfigurationManager.AppSettings["Domain"],
                "",
                new { action = "Index", controller = "Profile" }
            ));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "anmeldung",
                    new { action = "Login", controller = "Account", id = UrlParameter.Optional }));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "registrieren",
                    new { action = "Register", controller = "Account", id = UrlParameter.Optional }));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "logout",
                    new { action = "LogOff", controller = "Account", id = UrlParameter.Optional }));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "einstellungen/{action}/{id}",
                    new { action = "Index", controller = "Account", id = UrlParameter.Optional }));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "einstellungen/passwort/{action}/{id}", 
                    new { action = "Index", controller = "Manage", id = UrlParameter.Optional }));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "profil/{id}",
                    new { action = "ViewProfile", controller = "Profile"}
                ));

            context.Routes.Add("userDefault", route);
        }
    }
}
