// Copyright 2015-2016 the pokefans-core authors. See copying.md for legal info.
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
            context.Routes.Add("ContentNew",
                new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "inhalte/neu",
                    new { action = "New", Controller = "Content" })
            );

            context.Routes.Add("ContentIndex",
                new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "inhalte",
                    new { action = "Index", Controller = "Content" })
            );

           context.Routes.Add("ContentDetail",
                new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "inhalte/{contentId}/{action}/{detailId}",
                    new { action = "Edit", Controller = "Content", detailId = UrlParameter.Optional })
            );

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

            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "user/{id}/sperren",
                    new { action = "Bans", Controller = "User" }
                ));

            // Fanart
            context.Routes.Add(new DomainRoute(
                    "mitarbeit."+ConfigurationManager.AppSettings["Domain"],
                    "fanart/kategorien",
                    new { action = "Categories", Controller = "Fanart" }
                ));

            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "fanart/bearbeiten",
                    new { action = "Select", Controller = "Fanart" }
                ));

            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "fanart/bearbeiten/{id}",
                    new { action = "Edit", Controller = "Fanart" }
                ));

            // wifi
            context.Routes.Add(new DomainRoute(
                    "mitarbeit."+ConfigurationManager.AppSettings["Domain"],
                    "tauschboerse",
                    new { action = "Index", Controller = "Wifi" }
                ));

            context.Routes.Add(new DomainRoute(
                    "mitarbeit."+ConfigurationManager.AppSettings["Domain"],
                    "tauschboerse/banliste",
                    new { action = "Banlist", Controller = "Wifi"}
            ));

            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "tauschboerse/meldungen",
                    new { action = "Reports", Controller = "Wifi" }
            ));

            // wifi api
            context.Routes.Add(new DomainRoute(
                    "mitarbeit."+ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/statistics",
                    new { action = "Statistics", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/generations",
                    new { action = "Generations", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/activity",
                    new { action = "Activity", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit."+ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/pokemon-top10",
                    new { action = "PokemonTop10", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/pokemon-top10-completed",
                    new { action = "PokemonTop10Completed", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/pokemon-top10-open",
                    new { action = "PokemonTop10Open", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/item-top10",
                    new { action = "PokemonTop10", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/item-top10-completed",
                    new { action = "PokemonTop10Completed", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/item-top10-open",
                    new { action = "PokemonTop10Open", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/report/{id}",
                    new { action = "Report", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/cheat",
                    new { action = "Cheat", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/resolve",
                    new { action = "Resolve", Controller = "Wifi" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/v1/wifi/delete",
                    new { action = "Delete", Controller = "Wifi" }
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

            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/fanart/categories/add",
                    new { action = "AddCategory", Controller = "Fanart"}
                ));

            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/fanart/categories/edit",
                    new { action = "EditCategory", Controller = "Fanart" }
                ));

            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/fanart/categories/delete",
                    new { action = "DeleteCategory", Controller = "Fanart" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/user/bans/fanart",
                    new { action = "ModifyFanartBan", Controller = "User" }
                ));
            context.Routes.Add(new DomainRoute(
                    "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                    "api/pokemon/names",
                    new { action = "Names", Controller = "PokemonApi"}
                ));

            // Dashboard API
            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "api/v1/dashboard/pm-report-chart",
                new { action = "PMReportChart", Controller = "Dashboard" }
                ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "api/v1/dashboard/pm-report-table",
                new { action = "PmReportTable", Controller = "Dashboard" }
                ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "api/v1/dashboard/wifi-report-chart",
                new { action = "WifiReportChart", Controller = "Dashboard" }
                ));

            context.Routes.Add(new DomainRoute(
                "mitarbeit." + ConfigurationManager.AppSettings["Domain"],
                "api/v1/dashboard/wifi-report-table",
                new { action = "WifiReportTable", Controller = "Dashboard" }
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