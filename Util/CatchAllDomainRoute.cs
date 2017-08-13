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
using System.Diagnostics;

namespace Pokefans.Util
{
    public class CatchAllDomainRoute : Route
    {
        private Regex domainRegex;

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
        public CatchAllDomainRoute(string domain, string url, RouteValueDictionary defaults)
            : this(domain, url, defaults, new MvcRouteHandler(), new RouteValueDictionary())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRoute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        public CatchAllDomainRoute(string domain, string url, object defaults)
            : this(domain, url, new RouteValueDictionary(defaults))
        {
        }

        public CatchAllDomainRoute(string domain, string url, object defaults, RouteValueDictionary dataTokens)
            : this(domain, url, new RouteValueDictionary(defaults), new MvcRouteHandler(), dataTokens)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRoute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="handler">The handler.</param>
        public CatchAllDomainRoute(string domain, string url, object defaults, IRouteHandler handler)
            : this(domain, url, new RouteValueDictionary(defaults), handler, new RouteValueDictionary())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainRoute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="handler">The handler.</param>
        public CatchAllDomainRoute(string domain, string url, RouteValueDictionary defaults, IRouteHandler handler, RouteValueDictionary dataTokens)
            : base(url, defaults, null, dataTokens, handler)
        {
            this.Domain = domain;

            StackTrace trace = new StackTrace();

            for (int i = 0; i < trace.FrameCount; i++)
            {
                Type t = trace.GetFrame(i).GetMethod().DeclaringType;
                if (t.BaseType == typeof(AreaRegistration))
                {
                    object instance = Activator.CreateInstance(t);
                    // maybe we want to reuse the datatokens array, so we have to check first
                    // if the area already has been added.
                    if(!DataTokens.ContainsKey("area"))
                        this.DataTokens.Add("area", ((AreaRegistration)instance).AreaName);
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            domainRegex = CreateRegex(Domain);

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


            Match dMatch = domainRegex.Match(requestDomain);

            if (dMatch.Success)
            {
                RouteData data = new RouteData(this, RouteHandler);

                if (Defaults != null)
                {
                    foreach (var item in Defaults)
                    {
                        data.Values[item.Key] = item.Value;
                    }
                }

                // match subdomains
                for (int i = 1; i < dMatch.Groups.Count; i++)
                {
                    Group g = dMatch.Groups[i];
                    if (g.Success)
                    { 
                        string key = domainRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(g.Value))
                            {
                                data.Values[key] = g.Value;
                            }
                        }
                    }
                }

                if (this.DataTokens != null && this.DataTokens.Count > 0)
                {
                    foreach (var token in this.DataTokens)
                    {
                        data.DataTokens.Add(token.Key, token.Value);
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

            foreach (var rv in values)
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
            source = source.Replace("{", @"(?<").Replace("}", @">([a-zA-Z0-9_\-]*))");

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
}
