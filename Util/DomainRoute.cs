// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pokefans.Util
{
    public class DomainRoute : Route
    {
        private Regex domainRegex;
        private Regex pathRegex;

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public string Domain { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRoute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        public DomainRoute(string domain, string url, RouteValueDictionary defaults)
            : this(domain, url, defaults, new MvcRouteHandler())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRoute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        public DomainRoute(string domain, string url, object defaults)
            : this(domain, url, new RouteValueDictionary(defaults))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRoute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="handler">The handler.</param>
        public DomainRoute(string domain, string url, object defaults, IRouteHandler handler)
            : this(domain, url, new RouteValueDictionary(defaults), handler)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRoute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="handler">The handler.</param>
        public DomainRoute(string domain, string url, RouteValueDictionary defaults, IRouteHandler handler)
            : base(url, defaults, handler)
        {
            this.Domain = domain;
        }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            domainRegex = CreateRegex(Domain);
            pathRegex = CreateRegex(Url);

            // filter out the port, it is irrelevant to routing
            string requestDomain = httpContext.Request.Headers["host"];
            if (!string.IsNullOrEmpty(requestDomain))
            {
                if (requestDomain.Contains(':'))
                {
                    requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":"));
                }
            }
            else
            {
                requestDomain = httpContext.Request.Url.Host;
            }

            string requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

            Match dMatch = domainRegex.Match(requestDomain);
            Match pMatch = pathRegex.Match(requestPath);

            if(dMatch.Success && pMatch.Success)
            {
                RouteData data = new RouteData(this, RouteHandler);

                if(Defaults != null)
                {
                    foreach(var item in Defaults)
                    {
                        data.Values[item.Key] = item.Value;
                    }
                }

                // match subdomains
                for(int i = 1; i < dMatch.Groups.Count; i++)
                {
                    Group g = dMatch.Groups[i];
                    if(g.Success)
                    { 
                        string key = domainRegex.GroupNameFromNumber(i);

                        if(!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if(!string.IsNullOrEmpty(g.Value))
                            {
                                data.Values[key] = g.Value;
                            }
                        }
                    }
                }

                // and then match the path
                for (int i = 1; i < pMatch.Groups.Count; i++)
                {
                    Group g = pMatch.Groups[i];
                    if (g.Success)
                    {
                        string key = pathRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(g.Value))
                            {
                                data.Values[key] = g.Value;
                            }
                        }
                    }
                }

                return data;
            }
            return null;
        }

        /// <summary>
        /// Gibt Informationen zu der URL zurück, die der Route zugeordnet ist.
        /// </summary>
        /// <param name="requestContext">Ein Objekt, das Informationen zur angeforderten Route kapselt.</param>
        /// <param name="values">Ein Objekt, das die Parameter für eine Route enthält.</param>
        /// <returns>
        /// Ein Objekt, das Informationen zur URL enthält, die der Route zugeordnet ist.
        /// </returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, RemoveDomainTokens(values));
        }

        public DomainData GetDomainData(HttpContextBase context, RouteValueDictionary values)
        {
            string hostname = Domain;

            foreach(var rv in values)
            {
                hostname = hostname.Replace("{" + rv.Key + "}", rv.Value.ToString());
            }

            return new DomainData()
            {
                Protocol = context.Request.IsSecureConnection ? "https" : "http",
                HostName = hostname,
                Fragment = ""
            };
        }

        /// <summary>
        /// Creates the regex.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private Regex CreateRegex(string source)
        {
            source = source.Replace("/", @"\/?").Replace(".", @"\.?").Replace("-", @"\-?");
            source = source.Replace("{", @"(?<").Replace("}", @">([a-zA-Z0-9_]*))");

            return new Regex("^" + source + "$");
        }

        /// <summary>
        /// Removes the domain tokens.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private RouteValueDictionary RemoveDomainTokens(RouteValueDictionary values)
        {
            Regex tokenRegex = new Regex(@"({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?");
            Match tokenMatch = tokenRegex.Match(Domain);
            for (int i = 0; i < tokenMatch.Groups.Count; i++)
            {
                Group group = tokenMatch.Groups[i];
                if (group.Success)
                {
                    string key = group.Value.Replace("{", "").Replace("}", "");
                    if (values.ContainsKey(key))
                        values.Remove(key);
                }
            }

            return values;
        }
    }

    public class DomainData
    {
        /// <summary>
        /// Gets or sets the protocol (i.E. http or https)
        /// </summary>
        /// <value>
        /// The protocol.
        /// </value>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the hostname
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the URL fragment.
        /// </summary>
        /// <value>
        /// The fragment.
        /// </value>
        public string Fragment { get; set; }
    }
}
