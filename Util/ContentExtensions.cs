// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using dotless.Core;
using dotless.Core.configuration;
using Pokefans.Data;

namespace Pokefans.Util
{
    public static class ContentExtensions
    {
        /// <summary>
        /// Compiles the Content's Stylesheet and stores it.
        /// </summary>
        /// <param name="content"></param>
        public static void CompileLess(this Content content)
        {
            var lessPath = ConfigurationManager.AppSettings["VariableLessPath"];
            var less = "#content { " + content.StylesheetCode + "}";

            if (File.Exists(lessPath))
            {
                less += "@import \"" + lessPath + "\";\n";
            }

            var config = new DotlessConfiguration
            {
                MinifyOutput = true
            };
            var result = Less.Parse(less, config);

            if (String.IsNullOrWhiteSpace(result) && !String.IsNullOrWhiteSpace(content.StylesheetCode))
            {
                // TODO: Show Compilation Errors
                throw new ArgumentException("Error parsing less code.");
            }

            content.StylesheetCss = result;
        }

        /// <summary>
        /// Parse the Content and evaluate BB-Codes/Zing etc.
        /// </summary>
        /// <param name="content"></param>
        public static void Parse(this Content content)
        {
            // Apply BbCodes
            var parser = new Parser.Parser();
            content.ParsedContent = parser.Parse(content.UnparsedContent);

            // Apply includes
            foreach (var boilerplate in content.Boilerplates.Where(boilerplate => boilerplate.ContentId != boilerplate.BoilerplateId))
            {
                content.ParsedContent = content.ParsedContent.Replace(String.Format("%{0}%", boilerplate.ContentBoilerplateName),
                    boilerplate.Boilerplate.ParsedContent);
            }

            // Update contents that include this one
            foreach (var boilerplate in content.BoilerplatesUsed.Where(boilerplate => boilerplate.ContentId != boilerplate.BoilerplateId))
            {
                // TODO: Check for loops
                boilerplate.Content.Parse();
            }
        }

        /// <summary>
        /// Check weither or not two usernames match
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool MatchesUsername(this string username, string name)
        {
            var regex = new Regex(@"[^a-zA-Z0-9\-_üöäÜÖÄß]");

            return regex.Replace(username, "_") == regex.Replace(name, "_");
        }

        /// <summary>
        /// Check weither or not a content matches a single filter
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filterName"></param>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        private static bool MatchesFilter(this Content content, string filterName, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
            {
                return true;
            }

            switch (filterName)
            {
                // Check for special filters
                case "special":
                    {
                        switch (filterValue)
                        {
                            case "routen":
                                return content.MatchesFilter("title", "%Route%in Omega Rubin und Alpha Saphir%");
                        }
                        return true;
                    }
                // Check for content title
                case "title":
                    {
                        try
                        {
                            return Regex.IsMatch(content.Title, filterValue.Replace("%", ".*"));
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                // Check for content type
                case "type":
                    {
                        switch (filterValue)
                        {
                            case "article":
                                return content.Type == ContentType.Article;
                            case "news":
                                return content.Type == ContentType.News;
                            case "special":
                            case "element":
                                return content.Type == ContentType.Special;
                            case "include":
                            case "boilerplate":
                                return content.Type == ContentType.Boilerplate;
                            default:
                                return false;
                        }
                    }
                // Check for content category
                case "cat":
                    {
                        int id;
                        if (int.TryParse(filterValue, out id))
                        {
                            return content.CategoryId == id;
                        }
                        return false;
                    }
                // Check for content author
                case "user":
                case "author":
                    {
                        int id;
                        if (int.TryParse(filterValue, out id))
                        {
                            return content.AuthorUserId == id;
                        }
                        return content.Author.UserName.MatchesUsername(filterValue);
                    }
                // Check for content area
                case "index":
                case "area":
                    {
                        return content.Category != null && content.Category.AreaName == filterValue;
                    }
                // Check for content status
                case "status":
                    {
                        switch (filterValue)
                        {
                            case "public":
                                return content.Status == ContentStatus.Published;
                            case "ready":
                                return content.Status == ContentStatus.Ready;
                            case "progress":
                                return content.Status == ContentStatus.WorkInProcess;
                            case "discarded":
                                return content.Status == ContentStatus.Discarded;
                            default:
                                return false;
                        }
                    }
            }

            return false;
        }

        /// <summary>
        /// Checks weither or not a content matches a filter
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static bool MatchesFilter(this Content content, string filter)
        {
            var tokens = filter.Split(':');

            if (tokens.Length != 2 || string.IsNullOrEmpty(tokens[0]))
                return content.UnparsedContent.Contains(filter);

            var values = tokens[1].Split('|');
            return values.Any(v => content.MatchesFilter(tokens[0], v));
        }

        /// <summary>
        /// Filter a list of Content objects
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IEnumerable<Content> Filter(this IEnumerable<Content> data, string filter)
        {
            var filterItems = filter.Split(' ');

            return filterItems.Aggregate(data, (current, filterItem) => current.Where(c => c.MatchesFilter(filterItem)));
        }

        /// <summary>
        /// Process a content view.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="request"></param>
        /// <param name="context"></param>
        public static void TrackView(this Content content, HttpRequestBase request, Entities context)
        {
            // Local requests or requests to unpublished content won't need to be tracked.
#if !DEBUG
            if (request.IsLocal || content.Status != ContentStatus.Published || request.)
            {
                return;
            }
#endif

            // Ignore internal referrals
            if (request.UrlReferrer != null &&
                request.UrlReferrer.Host.Contains(ConfigurationManager.AppSettings["Domain"]))
            {
                return;
            }

            content.ViewCount++;

            var trackingEntry = new ContentTrackingSource
            {
                ContentId = content.Id,
                IpAdress = request.UserHostAddress,
                RequestTime = DateTime.Now,
                TargetUrl = request.Url?.AbsoluteUri,
            };

            if (request.UrlReferrer != null)
            {
                var searchHosts = new[] { "google.", "bing.", "suche.", "search.", "seach/", "pokefans" };

                trackingEntry.SourceUrl = request.UrlReferrer.AbsoluteUri;
                trackingEntry.SourceHost = request.UrlReferrer.Host;

                if (searchHosts.Any(h => trackingEntry.SourceUrl.Contains(h)))
                {
                    var queryString = HttpUtility.ParseQueryString(request.UrlReferrer.Query);
                    trackingEntry.SearchTerm = queryString["query"] ?? queryString["q"];
                }
            }

            context.ContentTrackingSources.Add(trackingEntry);
            context.SaveChanges();
        }

        /// <summary>
        /// Query the latest version of this content.
        /// </summary>
        /// <param name="content">The content to fetch the versions for</param>
        /// <param name="skip">The amount of versions to skip, i.e. fetching the <paramref name="skip"/>th version</param>
        /// <returns></returns>
        public static ContentVersion GetLatestVersion(this Content content, int skip=0)
        {
            return content.Versions.OrderByDescending(v => v.Updated)
                .Skip(skip)
                .FirstOrDefault();
        }
    }
}
