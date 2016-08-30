// Copyright 2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pokefans.Util
{
    public class AllowCorsAttribute : ActionFilterAttribute
    {
        public string[] Domains { get; set; }
        private bool all = false;

        public AllowCorsAttribute(params string[] domains)
        {
            Domains = domains;
        }
        public AllowCorsAttribute()
        {
            all = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (all || Domains.Any(x => x + "." + ConfigurationManager.AppSettings["Domain"] == filterContext.RequestContext.HttpContext.Request.UrlReferrer.Host))
            {
                if (filterContext.RequestContext.HttpContext.Request.IsSecureConnection)
                {
                    filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "https://" + filterContext.RequestContext.HttpContext.Request.UrlReferrer.Host + " env=HTTPS");
                }
                else
                {
                    filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "http://" + filterContext.RequestContext.HttpContext.Request.UrlReferrer.Host);
                }
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
