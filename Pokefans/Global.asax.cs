using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Pokefans.App_Start;
using Microsoft.Practices.Unity;
using Pokefans.Util;
using Pokefans.Caching;
using System.Configuration;
using System.Web.Http;
using Pokefans.Security;
using Pokefans.Data;

namespace Pokefans
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private bool cacheInitialized = false;

        protected void Application_Start()
        {
            // this purposefully ignores the subdomain. It is placed here so it is the first Route that will be registered
            RouteTable.Routes.MapRoute("TosAcceptRoute", "einstellungen/nutzungsbedingungen", new { action = "DsgvoCompliance", Controller = "Account" }, new string[] { "Pokefans.Areas.user.Controllers" });
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LuceneConfig.Configure(UnityConfig.GetConfiguredContainer());
        }
        
        protected void Application_End()
        {
            LuceneConfig.Unload(UnityConfig.GetConfiguredContainer());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            UnityConfig.GetConfiguredContainer().RegisterInstance<IBreadcrumbs>(new Breadcrumbs(), new PerRequestLifetimeManager());
            UnityConfig.GetConfiguredContainer().RegisterInstance<HttpContextBase>(new HttpContextWrapper(HttpContext.Current), new PerRequestLifetimeManager());
            
            if (ConfigurationManager.AppSettings["CachingBackend"].ToLower() == "native")
            {
                UnityConfig.GetConfiguredContainer().RegisterType<Cache, NativeCache>(new PerRequestLifetimeManager());
                cacheInitialized = false;
            }
            else
            {
                UnityConfig.GetConfiguredContainer().RegisterType<Cache, Memcached>(new PerRequestLifetimeManager());
            }

            if (!cacheInitialized)
            {
                UnityConfig.GetConfiguredContainer().Resolve<CacheFill>();
            }

        }

        /// <summary>
        ///  This event is consumed because at one point in the pipeline we need to check
        ///  DSGVO Compliance status. We redirect if we do not have compliance, converting
        ///  any POST to GET requests discarding the reuqest body. Once consent is reached,
        ///  Users can re-upload their stuff.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if(Request.IsAuthenticated)
            if (User.Identity.IsAuthenticated)
            {
                Cache cache = UnityConfig.GetConfiguredContainer().Resolve<Cache>();
                Entities db = UnityConfig.GetConfiguredContainer().Resolve<Entities>();

                DateTime? DsgvoUpdate = null;
                if (!cache.TryGet("DsgvoUpdate", out DsgvoUpdate))
                {
                    if (!db.DsgvoComplianceInfos.Any())
                        return;
                    DsgvoUpdate = db.DsgvoComplianceInfos.Where(x => x.EffectiveTime <= DateTime.Now).Max(g => g.EffectiveTime);
                    cache.Add<DateTime?>("DsgvoUpdate", DsgvoUpdate);
                }
                if (db.Users.Any(g => g.UserName == User.Identity.Name && (g.LastTermsOfServiceAgreement == null || g.LastTermsOfServiceAgreement < DsgvoUpdate)))
                {
                    
                    string path = HttpContext.Current.Request.Path;
                    path = path.TrimStart('/');
                    if (!path.StartsWith("einstellungen/nutzungsbedingungen", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // redirect to dsgvo consent site
                        string target = "/einstellungen/nutzungsbedingungen?redirect=" + HttpContext.Current.Request.Url.AbsoluteUri;
                        Response.Clear();

                        Response.Status = "303 See Other";
                        Response.AddHeader("Location", ConfigurationManager.AppSettings["PreferedProtocol"] + "://user."+ConfigurationManager.AppSettings["Domain"]+target);

                        Response.End();
                    }
                }
            }
        }
    }
}
