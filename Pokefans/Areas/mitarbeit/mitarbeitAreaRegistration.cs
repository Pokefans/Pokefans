// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.Configuration;
using System.Web.Mvc;
using Pokefans.Util;

namespace Pokefans.Areas.mitarbeit
{
    public class mitarbeitAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "mitarbeit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/liste/bbs",
                new { action = "BbsList", Controller = "UserLists", id = UrlParameter.Optional })
            );

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/liste/posthunter",
                new { action = "Posthunter", Controller = "UserLists", id = UrlParameter.Optional })
            );

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/liste/posthunter-mullschlucker",
                new { action = "Trashhunter", Controller = "UserLists", id = UrlParameter.Optional })
            );

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/liste/multiaccounts",
                new { action = "Multiaccounts", Controller = "UserLists", id = UrlParameter.Optional })
            );

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/liste/sperren",
                new { action = "Bans", Controller = "UserLists", id = UrlParameter.Optional })
            );

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/",
                new { action = "Index", Controller = "User" }
            ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/{id}",
                new { action = "Notes", Controller = "User" })
            );

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/{id}/neue-notiz",
                new { action = "AddNote", Controller = "User" })
            );

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/{id}/werbung",
                new { action = "AddAdvertising", Controller = "User" }
                ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/{id}/accounts",
                new { action = "Multiaccount", Controller = "User" }
                ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/{id}/rechte",
                new { action = "Roles", Controller = "User" }
                ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "api/rolemanager/{id}/set-role",
                new { action = "SetRole", Controller = "User" }
                ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "user/{id}/stammdaten",
                new { action = "UserData", Controller = "User" }
                ));

            // AJAX API below
            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "api/multiaccount/{id}/set-exception",
                new { action = "SetMultiaccountException", Controller = "User" }
                ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "api/multiaccount/{id}/set-no-multiaccount",
                new { action = "SetNoMultiaccount", Controller = "User" }
                ));

            // Default Route
            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "{controller}/{action}/{id}",
                new { action = "Index", controller = "Dashboard", id = UrlParameter.Optional }
                ));
        }
    }
}