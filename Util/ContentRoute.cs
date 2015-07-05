// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pokefans.Data;

namespace Pokefans.Util
{
    public class ContentRoute : Route
    {
        private readonly Entities _entities;

        public ContentRoute(string controllerName, string actionName)
            : base(string.Empty, new RouteValueDictionary(new { controller = controllerName, action = actionName }), new MvcRouteHandler())
        {
            _entities = DependencyResolver.Current.GetService<Entities>();
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

            var url = _entities.ContentUrls.FirstOrDefault(u => u.Url == requestPath);

            if (url == null)
            {
                return null;
            }

            var routeData = new RouteData(this, RouteHandler);

            if (Defaults != null)
            {
                foreach (var item in Defaults)
                {
                    routeData.Values[item.Key] = item.Value;
                }
            }

            routeData.Values["contentId"] = url.ContentId;

            return routeData;
        }
    }
}
