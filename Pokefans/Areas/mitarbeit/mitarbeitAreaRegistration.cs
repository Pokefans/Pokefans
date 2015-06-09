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
                "{controller}/{action}/{id}",
                new { action = "Index", controller = "Dashboard", id = UrlParameter.Optional })
            );
        }
    }
}