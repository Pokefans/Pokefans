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
                "{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional })
            );
        }
    }
}