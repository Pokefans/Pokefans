// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokefans.Data;
using Pokefans.Security.Exceptions;
using Pokefans.Caching;

namespace Pokefans.Security
{
    public static class UserExtensions
    {
        /// <summary>
        /// Generates the URL for this user based on his Username.
        /// </summary>
        /// <param name="u">The User.</param>
        /// <returns>url-friendly email</returns>
        public static string GenerateUrl(this User u)
        {
            return u.UserName.ToLower().Replace(' ', '+');
        }

        /// <summary>
        /// Determines whether the User has the given permission.
        /// </summary>
        /// <param name="u">The u.</param>
        /// <param name="permissionname">The permissionname.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static bool IsInRole(this User u, string permissionname, Cache cache, Entities context)
        {
            List<Role> permissions;
            if (!cache.Contains("permissions"))
            {
                permissions = context.Roles.ToList();
                cache.Add<List<Role>>("permissions", permissions);
            }
            else
            {
                permissions = cache.Get<List<Role>>("permissions");
            }

            if (!permissions.Any(x => x.Name == permissionname))
                throw new PermissionNotFoundException();

            int id = permissions.Where(x => x.Name == permissionname).First().Id;

            return context.UserRoles.Any(x => x.UserId == u.Id && x.PermissionId == id);
        }
    }
}
