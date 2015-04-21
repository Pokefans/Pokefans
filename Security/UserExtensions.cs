using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokefans.Data;

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
    }
}
