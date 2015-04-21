using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokefans.Data;
using Pokefans.SystemCache;

namespace Pokefans.Security
{
    public static class UserExtensions
    {
        /// <summary>
        /// Generates the URL for this user based on his username.
        /// </summary>
        /// <param name="u">The User.</param>
        /// <returns>url-friendly username</returns>
        public static string GenerateUrl(this User u)
        {
            return u.Name.ToLower().Replace(' ', '+');
        }

        /// <summary>
        /// Determines whether the User has the given permission.
        /// </summary>
        /// <param name="u">The u.</param>
        /// <param name="permissionname">The permissionname.</param>
        /// <returns></returns>
        public static bool HasPermission(this User u, string permissionname)
        {
            return u.HasPermission(permissionname, new Memcached(ConfigurationManager.AppSettings["MemcachedHost"], int.Parse(ConfigurationManager.AppSettings["MemcachedPort"])), new Entities());
        }

        /// <summary>
        /// Determines whether the User has the given permission.
        /// </summary>
        /// <param name="u">The u.</param>
        /// <param name="permissionname">The permissionname.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static bool HasPermission(this User u, string permissionname, Cache cache, Entities context)
        {
            List<Permission> permissions;
            if (!cache.Contains("permissions"))
            {
                permissions = context.Permissions.ToList();
                cache.Add<List<Permission>>("permissions", permissions);
            }
            else
            {
                permissions = cache.Get<List<Permission>>("permissions");
            }

            int id = permissions.Where(x => x.name == permissionname).First().id;

            return context.UserPermissions.Where(x => x.user_id == WebSecurity.CurrentCuser.id && x.permission_id == id).Count() > 0;
        }
    }
}
