//Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;

namespace Pokefans
{
    /// <summary>
    /// Authorize attribute. Altough it needs to stay here, this would technically belong to the lib/Security project.
    /// It is placed here so the compiler picks this up instead of the System.Web.Mvc.AuthorizeAttribute, which is vital;
    /// because this resolves the difference between HTTP401 and HTTP403. The original AuthorizeAttribute is horribly broken
    /// in this case.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}

