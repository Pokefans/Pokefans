using System.Configuration;
using System.Web.Mvc;
using Pokefans.Util;

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
            //context.MapRoute(
            //    "user_default",
            //    "user/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

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


        }
    }
}