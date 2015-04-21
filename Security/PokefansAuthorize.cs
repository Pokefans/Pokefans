using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Pokefans.Data;
using Pokefans.SystemCache;
using System.Data.Linq;
using System.Data;

namespace Pokefans.Security
{
    class PokefansAuthorizeAttribute : AuthorizeAttribute
    {
        private string permission = String.Empty;
        Entities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PokefansAuthorizeAttribute"/> class.
        /// </summary>
        public PokefansAuthorizeAttribute()
            : this(String.Empty, new Entities())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PokefansAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public PokefansAuthorizeAttribute(string permission)
            : this(permission, new Entities())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PokefansAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <param name="e">The database context to use.</param>
        public PokefansAuthorizeAttribute(string permission, Entities e)
        {
            this.permission = permission;
            this.context = e;
        }

        /// <summary>
        /// When overridden, provides an entry point for custom authorization checks.
        /// </summary>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>
        /// true if the user is authorized; otherwise, false.
        /// </returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (WebSecurity.CurrentCuser != null)
            {
                if (String.IsNullOrEmpty(permission))
                {
                    return true;
                }
                return WebSecurity.CurrentCuser.HasPermission(permission);
            }
            return false;
        }
    }
}
