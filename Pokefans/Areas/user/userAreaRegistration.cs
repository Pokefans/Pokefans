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
                    "passwort-vergessen",
                    new { action = "ResetPasswordConfirmation", controller = "Account", id = UrlParameter.Optional }));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "einstellungen/passwort", 
                    new { action = "ChangePassword", controller = "Manage", id = UrlParameter.Optional }, dataTokens));

			context.Routes.Add(new DomainRoute(
					"user." + ConfigurationManager.AppSettings["Domain"],
					"einstellungen/passwort-setzen",
                new { action = "SetPassword", controller = "Manage", id = UrlParameter.Optional }, dataTokens));

			context.Routes.Add(new DomainRoute(
					"user." + ConfigurationManager.AppSettings["Domain"],
					"einstellungen/link-account",
                new { action = "LinkLogin", controller = "Manage", id = UrlParameter.Optional }, dataTokens));

			context.Routes.Add(new DomainRoute(
					"user." + ConfigurationManager.AppSettings["Domain"],
					"einstellungen/unlink-account",
                new { action = "RemoveLogin", controller = "Manage", id = UrlParameter.Optional }, dataTokens));

			context.Routes.Add(new DomainRoute(
					"user." + ConfigurationManager.AppSettings["Domain"],
					"einstellungen/accounts-verwalten",
                new { action = "ManageLogins", controller = "Manage", id = UrlParameter.Optional }, dataTokens));

            /*context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "einstellungen/nutzungsbedingungen",
                new { action = "DsgvoCompliance", controller = "Account" }
            ));*/


            context.Routes.Add(new DomainRoute(
                "user." + ConfigurationManager.AppSettings["Domain"],
                "einstellungen/{action}/{id}",
                new { action = "Index", controller = "Account", id = UrlParameter.Optional }));

			// todo: 2-Factor-Authentication



            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "profil/{url}",
                    new { action = "ViewProfile", controller = "Profile"}
                ));

            // Private messages

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "nachrichten/",
                    new { action = "Index", controller = "PrivateMessage"}
            ));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "nachrichten/labels",
                    new { action = "ManageLabels", controller = "PrivateMessage"}
            ));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "nachrichten/regeln",
                    new { action = "ManageRules", controller = "PrivateMessage"}            
            ));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/labels/delete",
                    new { action = "DeleteLabel", Controller = "PrivateMessage" }
            ));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/labels/edit",
                    new { action = "EditLabel", Controller = "PrivateMessage" }
            ));

            context.Routes.Add(new DomainRoute(
                    "user." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/validateuser",
                new { action = "ValidateUser", Controller = "PrivateMessage" }
            ));

            context.Routes.Add(new DomainRoute(
                "user." + ConfigurationManager.AppSettings["Domain"],
                "nachrichten/neu",
                new { action = "Compose", Controller = "PrivateMessage" }
            ));

            context.Routes.Add(new DomainRoute(
                "user." + ConfigurationManager.AppSettings["Domain"],
                "nachrichten/postausgang",
                new { action = "Outbox", Controller = "PrivateMessage" }
            ));

            context.Routes.Add(new DomainRoute(
                "user." + ConfigurationManager.AppSettings["Domain"],
                "nachrichten/gesendet",
                new { action = "Sent", Controller = "PrivateMessage" }
            ));

            context.Routes.Add(new DomainRoute(
                "user." + ConfigurationManager.AppSettings["Domain"],
                "nachrichten/{id}",
                new { action = "View", Controller = "PrivateMessage" }
            ));

            context.Routes.Add("userDefault", route);
        }
    }
}
