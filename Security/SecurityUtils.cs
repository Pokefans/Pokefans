using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Pokefans.Security
{
    public enum UserStatus { Activated = 1, NotActivated = 0, Banned = -1 };
    class SecurityUtils
    {
        /// <summary>
        /// Gets the client ip address of the current request. This also aims to detect cheap proxies by looking at the HTTP_X_FORWAREDED_FOR header. 
        /// If this header however contains a private address, the proxy address is used as a fallback.
        /// </summary>
        /// <returns>IP Adress as string</returns>
        public static string GetIPAddressAsString()
        {
            string ffip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(ffip))
            {
                string ip_t = ip.Split(',')[0];
                // Filter out private adresses. Better use the proxy before we get some natted stuff, which can happen using Microsoft Threat Management Gateway, for example.
                if(!Regex.IsMatch(ip_t, @"^(192\.168\.|10\.|172\.1[6789]\.|172\.2[0-9]\.|172\.3[0-6]\.|169\.254\.|f[cd]..:)"))
                {
                    ip = ip_t;
                }
            }

            return ip;
        }
    }
}
