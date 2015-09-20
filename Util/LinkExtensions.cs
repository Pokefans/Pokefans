// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Pokefans.Util
{
    public static class LinkExtensions
    {
        #region ActionLink

        /// <summary>
        /// Makes a Resource Link. This is only the url; you have to wrap it in &lt;img&gt; or whatever tag you desire.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceLink(this HtmlHelper helper, string path)
        {
            return new MvcHtmlString(String.Format("//{0}/{1}", ConfigurationManager.AppSettings["Domain"], path));
        }

        /// <summary>
        /// Makes a Resource Link. This is only the url; you have to wrap it in &lt;img&gt; or whatever tag you desire.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="path">The path.</param>
        /// <param name="domain">The domain.</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceLink(this HtmlHelper helper, string path, string domain)
        {
            return new MvcHtmlString(String.Format("//{2}.{0}/{1}", ConfigurationManager.AppSettings["Domain"], path, domain));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, routeValues, new RouteValueDictionary(), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, routeValues, htmlAttributes, requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool requireAbsoluteUrl)
        {
            if (requireAbsoluteUrl)
            {
                HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
                RouteData routeData = RouteTable.Routes.GetRouteData(currentContext);

                routeData.Values["controller"] = controllerName;
                routeData.Values["action"] = actionName;

                DomainRoute domainRoute = routeData.Route as DomainRoute;
                if (domainRoute != null)
                {
                    DomainData domainData = domainRoute.GetDomainData(currentContext, routeData.Values);
                    return htmlHelper.ActionLink(linkText, actionName, controllerName, domainData.Protocol, domainData.HostName, domainData.Fragment, routeData.Values, null);
                }
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string subdomain)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, (HttpContext.Current.Request.IsSecureConnection ? "https" : "http"), subdomain + "." + ConfigurationManager.AppSettings["Domain"], "", routeValues: null, htmlAttributes: null);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string subdomain, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, (HttpContext.Current.Request.IsSecureConnection ? "https" : "http"), subdomain + "." + ConfigurationManager.AppSettings["Domain"], "", routeValues: routeValues, htmlAttributes: htmlAttributes);
        }

        // @Url.Action("Index", "User", new { Area = "mitarbeit" }, HttpContext.Current.Request.IsSecureConnection ? "https" : "http", )
        public static string Action(this UrlHelper helper, string actionName, string controllerName, object RouteValues, string subdomain, object dummy)
        {
            return helper.Action(actionName, controllerName, new RouteValueDictionary(RouteValues), (HttpContext.Current.Request.IsSecureConnection ? "https" : "http"), subdomain + "." + ConfigurationManager.AppSettings["Domain"]);
        }

        public static string Action(this UrlHelper helper, string actionName, string controllerName, object routeValues, bool requireAbsoluteUrl)
        {
            if (requireAbsoluteUrl)
            {
                HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
                RouteData routeData = RouteTable.Routes.GetRouteData(currentContext);

                routeData.Values["controller"] = controllerName;
                routeData.Values["action"] = actionName;

                DomainRoute domainRoute = routeData.Route as DomainRoute;
                if (domainRoute != null)
                {
                    DomainData domainData = domainRoute.GetDomainData(currentContext, routeData.Values);
                    return helper.Action(actionName, controllerName, routeData.Values, domainData.Protocol, domainData.HostName);
                }
            }
            return helper.Action(actionName, controllerName, routeValues);
        }

        #endregion

        #region url

        /// <summary>
        /// Map the specified helper, url and domain with an explicit setting of the subdomain.
        /// set domain NULL if no subdomain is to be used.
        /// </summary>
        /// <param name="helper">Helper.</param>
        /// <param name="url">URL.</param>
        /// <param name="domain">Domain.</param>
        public static string Map(this UrlHelper helper, string url, string domain)
        {
            if (domain == null)
            {
                return String.Format("{0}://{1}/{2}",
                    HttpContext.Current.Request.IsSecureConnection ? "https" : "http",
                    ConfigurationManager.AppSettings["Domain"],
                    url
                );
            }
            return String.Format("{0}://{1}.{2}/{3}",
                HttpContext.Current.Request.IsSecureConnection ? "https" : "http",
                domain,
                ConfigurationManager.AppSettings["Domain"],
                url
            );
        }

        /// <summary>
        /// Map the specified helper and url. This uses the current subdomain as base.
        /// </summary>
        /// <param name="helper">Helper.</param>
        /// <param name="url">URL.</param>
        public static string Map(this UrlHelper helper, string url)
        {
            return helper.Map(url, HttpContext.Current.Request.Url.Host.Replace("." + ConfigurationManager.AppSettings["Domain"], ""));
        }

        #endregion
    }

}
