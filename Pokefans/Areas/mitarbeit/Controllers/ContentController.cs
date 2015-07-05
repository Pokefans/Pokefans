// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System.Text.RegularExpressions;
using DiffMatchPatch;
using Microsoft.AspNet.Identity;
using PagedList;
using Pokefans.Areas.mitarbeit.Models;
using Pokefans.Data;
using Pokefans.Util;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles = "mitarbeiter")]
    public class ContentController : Controller
    {
        /// <summary>
        /// Entities instance for database access
        /// </summary>
        private readonly Entities _entities;

        public ContentController(Entities entities)
        {
            _entities = entities;
        }

        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new ContentListViewModel
            {
                Contents = _entities.Contents
                    .Include("Author")
                    .OrderByDescending(c => c.Created)
                    .ToPagedList(1, ContentListViewModel.PageSize),
                Page = 1
            };
            return View("~/Areas/mitarbeit/Views/Content/Index.cshtml", model);
        }

        /// <summary>
        /// Index page with applied filters 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(ContentListViewModel model)
        {
            if (model.Page != null && model.Page < 1)
            {
                model.Page = null;
            }

            // If neccessary add a new filter to the string
            if (!string.IsNullOrEmpty(model.AdditionalFilter))
            {
                if (string.IsNullOrEmpty(model.Filter))
                {
                    // If there was no filter we can just set it
                    model.Filter = model.AdditionalFilter;
                }
                else if (!model.AdditionalFilter.Contains(':'))
                {
                    // If the new filter is just a search string we can add it without further testing
                    model.Filter += String.Format(" {0}", model.AdditionalFilter);
                }
                else
                {
                    // If the new filter is a real filter we have to check for duplicates
                    var filterItems = model.Filter.Split(' ');
                    var filter = model.AdditionalFilter.Split(':');
                    var result = "";

                    foreach (var filterItem in filterItems)
                    {
                        if (filterItem.StartsWith(filter[0]))
                        {
                            // Found it! We just leave it out
                        }
                        else
                        {
                            result += " " + filterItem;
                        }
                    }

                    // Filter was not present, just add it to the list
                    if (filter.Length < 2 || !string.IsNullOrEmpty(filter[1]))
                    {
                        result += " " + model.AdditionalFilter;
                    }

                    model.Filter = result.Trim();
                }

                model.AdditionalFilter = string.Empty;

                // Redirect to make the filter-variable look nice!
                var url = Url.RouteUrl("ContentIndex", new
                {
                    Filter = model.Filter,
                    Page = model.Page
                });

                return Redirect(url.Replace("%3A", ":").Replace("%20", "+"));
            }

            model.Contents = _entities.Contents
                .Include("Author")
                .Include("Category")
                .Filter(model.Filter ?? "")
                .OrderByDescending(c => c.Created)
                .ToPagedList(model.Page ?? 1, ContentListViewModel.PageSize);

            return View("~/Areas/mitarbeit/Views/Content/Index.cshtml", model);
        }

        /// <summary>
        /// Create new Content
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            var model = new ContentEditViewModel
            {
                Categories = _entities.ContentCategories.OrderBy(c => c.OrderingPosition),
                IsContentAdministrator = User.IsInRole("artikel-administrator"),
                ContentStatusList = Enum.GetValues(typeof(ContentStatus))
                    .OfType<ContentStatus>()
                    .Where(e => e != ContentStatus.Published)
                    .Select(e => new SelectListItem
                    {
                        Text = e.GetDisplayName(),
                        Value = ((int)e).ToString()
                    }),
                ContentPermissionList = _entities.Roles
                    .Where(r => r.Metapermission.Name == "mitarbeiter")
                    .Select(r => new SelectListItem
                    {
                        Text = r.FriendlyName,
                        Value = r.Id.ToString()
                    })
            };
            return View("~/Areas/mitarbeit/Views/Content/Edit.cshtml", model);
        }

        /// <summary>
        /// Create new Content
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult New(ContentEditViewModel model)
        {
            return Edit(model);
        }

        /// <summary>
        /// View all Content Versions
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public ActionResult Versions(int? contentId, int? detailId)
        {
            if (detailId != null)
            {
                return Version(detailId);
            }

            var content = _entities.Contents.FirstOrDefault(c => c.Id == contentId);

            if (content == null)
            {
                return RedirectToRoute("ContentIndex");
            }

            var model = new ContentVersionListViewModel
            {
                Content = content,
                Versions =
                    _entities.ContentVersions.Where(v => v.ContentId == content.Id).Include("User").OrderByDescending(v => v.Updated)
            };

            return View("~/Areas/mitarbeit/Views/Content/Versions.cshtml", model);
        }

        /// <summary>
        /// View a specific Content Version
        /// </summary>
        /// <returns></returns>
        public ActionResult Version(int? contentId)
        {
            var version = _entities.ContentVersions.FirstOrDefault(v => v.Id == contentId);

            if (version == null)
            {
                return RedirectToRoute("ContentIndex");
            }

            var previousVersion = _entities.ContentVersions
                .Where(v => v.Version < version.Version && v.ContentId == version.ContentId)
                .OrderByDescending(v => v.Version)
                .FirstOrDefault();

            var model = new ContentVersionViewModel
            {
                CurrentVersion = version,
                PreviousVersion = previousVersion
            };

            if (previousVersion != null)
            {
                var dmp = new diff_match_patch();
                var diffsContent = dmp.diff_main(previousVersion.UnparsedContent, version.UnparsedContent);
                var diffsStylesheet = dmp.diff_main(previousVersion.StylesheetCode, version.StylesheetCode);

                model.ContentDiff = dmp.diff_prettyHtml(diffsContent);
                model.StylesheetDiff = dmp.diff_prettyHtml(diffsStylesheet);
            }

            return View("~/Areas/mitarbeit/Views/Content/Version.cshtml", model);
        }

        /// <summary>
        /// Create new Content
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int? contentId)
        {
            var content = _entities.Contents.FirstOrDefault(c => c.Id == contentId);

            if (content == null)
            {
                return RedirectToRoute("ContentIndex");
            }

            if (content.EditPermission != null && !User.IsInRole(content.EditPermission.Name))
            {
                return RedirectToRoute("ContentIndex");
            }
            var model = ContentEditViewModel.FromContent(content);
            model.Categories = _entities.ContentCategories.OrderBy(c => c.OrderingPosition);
            model.IsContentAdministrator = User.IsInRole("artikel-administrator");

            // Remove status options the user cannot use
            var statusOptions = Enum.GetValues(typeof(ContentStatus))
                .OfType<ContentStatus>();
            if (!model.IsContentAdministrator)
            {
                statusOptions = content.Status == ContentStatus.Published ?
                    statusOptions.Where(e => e == ContentStatus.Published) : statusOptions.Where(e => e != ContentStatus.Published);
            }

            model.ContentStatusList = statusOptions.Select(e => new SelectListItem
                {
                    Text = e.GetDisplayName(),
                    Value = ((int)e).ToString()
                });
            model.ContentPermissionList = _entities.Roles
                .Where(r => r.Metapermission.Name == "mitarbeiter")
                .Select(r => new SelectListItem
                {
                    Text = r.FriendlyName,
                    Value = r.Id.ToString()
                });


            return View("~/Areas/mitarbeit/Views/Content/Edit.cshtml", model);
        }

        /// <summary>
        /// Create new Content
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ContentEditViewModel model)
        {
            Content content = null;
            if (ModelState.IsValid)
            {
                ContentVersion latestVersion = null;

                if (model.ContentId == null)
                {
                    // Create a new Content Object if there is none
                    var c = new Content
                    {
                        Version = 1,
                        EditPermissionId = null,
                        Published = DateTime.MinValue,
                        PublishedByUserId = null,
                        Created = DateTime.Now,
                        AuthorUserId = User.Identity.GetUserId<int>(),
                        UnparsedContent = "",
                        StylesheetCss = ""
                    };

                    content = _entities.Contents.Add(c);

                    var v = new ContentVersion
                    {
                        ContentId = content.Id,
                        UserId = content.AuthorUserId,
                        UnparsedContent = "",
                        StylesheetCode = "",
                        Title = "",
                        Description = "",
                        Teaser = "",
                        Notes = ""
                    };

                    latestVersion = _entities.ContentVersions.Add(v);
                    _entities.SaveChanges();
                    model.ContentId = content.Id;

                    var url = new ContentUrl
                    {
                        Url = String.Format("inhalt/{0}", content.Id),
                        ContentId = content.Id,
                        Type = UrlType.System,
                        Enabled = true
                    };

                    _entities.ContentUrls.Add(url);
                    content.DefaultUrl = url;
                }
                else
                {
                    // Try to find the existing content object
                    var result = _entities.Contents
                        .Include("EditPermission")
                        .Include("Boilerplates")
                        .Include("BoilerplatesUsed")
                        .Where(c => c.Id == model.ContentId);

                    if (result.Any())
                    {
                        content = result.First();

                        if (content.EditPermission != null && !User.IsInRole(content.EditPermission.Name))
                        {
                            return RedirectToRoute("ContentIndex");
                        }
                    }
                    else
                    {
                        return RedirectToRoute("ContentIndex");
                    }
                }

                if (User.IsInRole("artikel-administrator"))
                {
                    if (model.Status == ContentStatus.Published && content.Status != ContentStatus.Published)
                    {
                        // Publish the content
                        content.Published = DateTime.Now;
                        content.PublishedByUserId = User.Identity.GetUserId<int>();
                    }
                    else if (model.Status != ContentStatus.Published && content.Status == ContentStatus.Published)
                    {
                        // Unpublish the content
                        content.Published = DateTime.MinValue;
                        content.PublishedByUserId = null;
                    }

                    var permission = _entities.Roles.FirstOrDefault(r => r.Id == model.PermissionId);
                    if (permission != null && permission.Metapermission.Name == "mitarbeiter")
                    {
                        content.EditPermissionId = model.PermissionId;
                    }

                    if (string.IsNullOrEmpty(model.Url) && model.Status == ContentStatus.Published)
                    {
                        if (model.Type == ContentType.News)
                        {
                            // Default url is /news/-/1234
                            model.Url = "-";
                        }
                    }

                    if (!string.IsNullOrEmpty(model.Url))
                    {
                        model.Url = model.Url.Trim('/');
                        if (model.Type == ContentType.News && !Regex.IsMatch(model.Url, @"news/[a-zA-Z0-9_\-/]+/\d+"))
                        {
                            model.Url = String.Format("news/{0}/{1}", model.Url, content.Id);
                        }

                        if (content.DefaultUrl == null || content.DefaultUrl.Url != model.Url)
                        {
                            var duplicates = _entities.ContentUrls
                                .Where(u => u.Url == model.Url);

                            if (!duplicates.Any() || duplicates.First().Enabled == false)
                            {
                                _entities.ContentUrls.RemoveRange(duplicates);

                                var url = new ContentUrl
                                {
                                    ContentId = content.Id,
                                    Enabled = true,
                                    Type = UrlType.Default,
                                    Url = model.Url
                                };

                                if (content.DefaultUrl != null && content.DefaultUrl.Type == UrlType.Default)
                                {
                                    content.DefaultUrl.Type = UrlType.Alternative;
                                }

                                content.DefaultUrl = _entities.ContentUrls.Add(url);
                            }
                            else
                            {
                                ModelState.AddModelError("Url", "Die gewählte URL existiert bereits.");
                            }
                        }
                    }
                }
                else
                {
                    // Normal editors cannot change these options
                    model.HomePageOptions = content.HomePageOptions;
                    model.PermissionId = content.EditPermissionId;
                    model.Url = content.DefaultUrl.Url;

                    // Normal editors cannot un-publish content
                    if (model.Status != ContentStatus.Published && content.Status == ContentStatus.Published)
                    {
                        model.Status = content.Status;
                    }
                }

                // Update Database Fields
                model.UpdateContent(content);
                content.Updated = DateTime.Now;

                int userId;
                if (content.Version > 0)
                {
                    latestVersion = latestVersion ?? _entities.ContentVersions.Where(v => v.ContentId == content.Id).OrderByDescending(v => v.Updated).First();
                    userId = latestVersion.UserId;
                }
                else
                {
                    userId = content.AuthorUserId;
                }

                try
                {
                    content.CompileLess();
                }
                catch (ArgumentException)
                {
                    ViewBag.Error =
                        "Dein artikelspezifischer CSS-Code enthält Fehler und wurde daher nicht in den Artikel übernommen (aber in der Datenbank gespeichert).";
                }

                content.Parse();

                // Calculate Changes
                int updateCharsDeleted = 0;
                int updateCharsInserted = 0;
                double updateMagnificance = 0;

                if (latestVersion != null)
                {
                    var dmp = new diff_match_patch();
                    var diffs = dmp.diff_main(latestVersion.UnparsedContent ?? "", model.UnparsedContent ?? "");

                    foreach (var diffItem in diffs.Where(diffItem => diffItem.text.Trim().Length != 0))
                    {
                        switch (diffItem.operation)
                        {
                            case Operation.DELETE:
                                {
                                    updateCharsDeleted += diffItem.text.Length;
                                    break;
                                }
                            case Operation.INSERT:
                                {
                                    updateCharsInserted += diffItem.text.Length;
                                    break;
                                }
                        }
                    }

                    updateMagnificance =
                        Math.Min(updateCharsInserted * 0.9 / Math.Max(1, latestVersion.UnparsedContent.Length) +
                                 updateCharsDeleted * 0.1 / Math.Max(1, latestVersion.UnparsedContent.Length), 1);
                    updateMagnificance = Math.Min(1, Math.Tan(updateMagnificance));
                }

                // If the user was not the one who updated last we have to create a new version
                if (userId != User.Identity.GetUserId<int>())
                {
                    content.Version++;
                    latestVersion = new ContentVersion
                    {
                        ContentId = content.Id,
                        Title = content.Title,
                        Version = content.Version,
                        UnparsedContent = content.UnparsedContent,
                        ParsedContent = content.ParsedContent,
                        Description = content.Description,
                        StylesheetCss = content.StylesheetCss,
                        StylesheetCode = content.StylesheetCode,
                        Teaser = content.Teaser,
                        UserId = User.Identity.GetUserId<int>(),
                        Note = content.Notes,
                        Updated = DateTime.Now,
                        UpdateMagnificance = updateMagnificance,
                        UpdateCharsChanged = updateCharsInserted,
                        UpdateCharsDeleted = updateCharsDeleted
                    };

                    _entities.ContentVersions.Add(latestVersion);
                }
                else
                {
                    if (latestVersion != null)
                    {
                        model.UpdateContentVersion(latestVersion);
                        latestVersion.Version = content.Version;
                        latestVersion.ParsedContent = content.ParsedContent;
                        latestVersion.StylesheetCss = content.StylesheetCss;
                        latestVersion.Updated = DateTime.Now;
                        latestVersion.UpdateMagnificance = updateMagnificance;
                        latestVersion.UpdateCharsChanged = updateCharsInserted;
                        latestVersion.UpdateCharsDeleted = updateCharsDeleted;
                    }
                }

                // Save all Changes
                _entities.SaveChanges();
            }

            model.Saved = ModelState.IsValid;
            model.Categories =
                model.Categories ?? _entities.ContentCategories.OrderBy(c => c.OrderingPosition);
            model.IsContentAdministrator = User.IsInRole("artikel-administrator");

            // Remove status options the user cannot use
            var statusOptions = Enum.GetValues(typeof(ContentStatus))
                .OfType<ContentStatus>();

            if (content != null)
            {
                if (!model.IsContentAdministrator)
                {
                    statusOptions = content.Status == ContentStatus.Published
                        ? statusOptions.Where(e => e == ContentStatus.Published)
                        : statusOptions.Where(e => e != ContentStatus.Published);
                }
            }
            else
            {
                statusOptions = statusOptions.Where(e => e != ContentStatus.Published);
            }

            model.ContentStatusList = statusOptions.Select(e => new SelectListItem
            {
                Text = e.GetDisplayName(),
                Value = ((int)e).ToString()
            });
            model.ContentPermissionList = _entities.Roles
                .Where(r => r.Metapermission.Name == "mitarbeiter")
                .Select(r => new SelectListItem
                {
                    Text = r.FriendlyName,
                    Value = r.Id.ToString()
                });

            return View("~/Areas/mitarbeit/Views/Content/Edit.cshtml", model);
        }

        /// <summary>
        /// View a content's statistics
        /// </summary>
        /// <param name="contentId">Content to display</param>
        /// <returns></returns>
        public ActionResult Statistics(int? contentId)
        {
            var content = _entities.Contents.FirstOrDefault(c => c.Id == contentId);

            if (content == null)
            {
                return RedirectToRoute("ContentIndex");
            }

            var model = new ContentStatisticsViewModel
            {
                Content = content,
                ViewCount = content.ViewCount
            };
            return View("~/Areas/mitarbeit/Views/Content/Statistics.cshtml", model);
        }

        /// <summary>
        /// View content includes
        /// </summary>
        /// <param name="contentId">Content to display</param>
        /// <param name="detailId">Boilerplate id</param>
        /// <returns></returns>
        public ActionResult Includes(int? contentId, int? detailId)
        {
            var content = _entities.Contents.Include("Boilerplates").Include("BoilerplatesUsed").FirstOrDefault(c => c.Id == contentId);

            if (content == null)
            {
                return RedirectToRoute("ContentIndex");
            }

            if (detailId != null)
            {
                // Delete the requested id
                var boilerplate =
                    _entities.ContentBoilerplates.Include("Content")
                        .Include("Boilerplate")
                        .FirstOrDefault(c => c.Id == detailId);

                if (boilerplate == null || boilerplate.ContentId != content.Id)
                {
                    return RedirectToRoute("ContentDetail", new { contentId = content.Id, action = "includes" });
                }

                _entities.ContentBoilerplates.Remove(boilerplate);
                content.Parse();
                _entities.SaveChanges();

                return RedirectToRoute("ContentDetail", new { contentId = content.Id, action = "includes" });
            }

            var model = new ContentIncludesViewModel
            {
                Content = content,
                ContentId = content.Id,
                AvailableBoilerplates = _entities.Contents
                    .Where(c => c.Id != content.Id)
                    .Where(c => c.Type == ContentType.Boilerplate)
                    .Where(c => _entities.ContentBoilerplates.All(b => b.BoilerplateId != c.Id || b.ContentId != content.Id))
                    .AsEnumerable()
                    .Select(c => new SelectListItem
                        {
                            Text = string.Format("{0}: {1}", c.Id, c.Title),
                            Value = c.Id.ToString()
                        })
            };
            return View("~/Areas/mitarbeit/Views/Content/Includes.cshtml", model);
        }

        /// <summary>
        /// Submit boilerplate
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Includes(ContentIncludesViewModel model)
        {
            // grab the content object and load the boilerplate navigation properties
            var content = _entities.Contents
                .Include(c => c.Boilerplates.Select(b => b.Boilerplate))
                .Include(c => c.BoilerplatesUsed.Select(b => b.Boilerplate))
                .FirstOrDefault(c => c.Id == model.ContentId);

            if (content == null)
            {
                return RedirectToRoute("ContentIndex");
            }

            if (ModelState.IsValid)
            {
                var boilerplateDuplicates = _entities.ContentBoilerplates
                    .Where(c => c.Content.Id == model.ContentId)
                    .Where(c => c.BoilerplateId == model.BoilerplateId);

                var boilerplateContent = _entities.Contents
                    .Where(c => c.Id == model.BoilerplateId)
                    .Where(c => c.Type == ContentType.Boilerplate);

                if (!boilerplateContent.Any())
                {
                    ViewBag.Error = "Der Textbaustein existiert nicht.";
                    model.Saved = false;
                }
                else if (boilerplateDuplicates.Any())
                {
                    ViewBag.Error = "Du kannst den Textbaustein nicht mehrfach einbinden.";
                    model.Saved = false;
                }
                else
                {
                    var boilerplate = new ContentBoilerplate
                    {
                        ContentId = content.Id,
                        BoilerplateId = model.BoilerplateId,
                        ContentBoilerplateName = model.BoilerplateName
                    };
                    _entities.ContentBoilerplates.Add(boilerplate);

                    // We have to manually load these navigation properties for whatever reason
                    foreach (var bp in content.Boilerplates)
                    {
                        _entities.Entry(bp).Reference(b => b.Boilerplate).Load();
                    }
                    foreach (var bp in content.BoilerplatesUsed)
                    {
                        _entities.Entry(bp).Reference(b => b.Boilerplate).Load();
                        _entities.Entry(bp).Reference(b => b.Content).Load();
                    }

                    // re-parse the content
                    content.Parse();

                    _entities.SaveChanges();

                    model.Saved = true;
                }
            }

            // Update content object
            content = _entities.Contents
                            .Include(c => c.Boilerplates.Select(b => b.Boilerplate))
                            .Include(c => c.BoilerplatesUsed.Select(b => b.Boilerplate))
                            .FirstOrDefault(c => c.Id == model.ContentId);

            model.Content = content;
            model.AvailableBoilerplates = _entities.Contents
                .Where(c => c.Id != content.Id)
                .Where(c => c.Type == ContentType.Boilerplate)
                .Where(c => _entities.ContentBoilerplates.All(b => b.BoilerplateId != c.Id || b.ContentId != content.Id))
                .AsEnumerable()
                .Select(c => new SelectListItem
                {
                    Text = string.Format("{0}: {1}", c.Id, c.Title),
                    Value = c.Id.ToString()
                });

            return View("~/Areas/mitarbeit/Views/Content/Includes.cshtml", model);
        }

        /// <summary>
        /// List all Urls
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public ActionResult Urls(int? contentId, int? detailId)
        {
            var content = _entities.Contents
                .Include(c => c.Urls)
                .FirstOrDefault(c => c.Id == contentId);

            if (content == null)
            {
                return RedirectToRoute("ContentIndex");
            }

            if (detailId != null)
            {
                if (!User.IsInRole("artikel-administrator"))
                {
                    return RedirectToRoute("ContentDetail", new { contentId = content.Id, action = "urls" });
                }

                var url = _entities.ContentUrls
                    .FirstOrDefault(u => u.Id == detailId);

                if (url == null || url.ContentId != contentId || url.Enabled == false || url.Type == UrlType.System)
                {
                    return RedirectToRoute("ContentDetail", new { contentId = content.Id, action = "urls" });
                }

                url.Enabled = false;
                _entities.SaveChanges();

                return RedirectToRoute("ContentDetail", new { contentId = content.Id, action = "urls" });
            }

            var model = new ContentUrlsViewModel
            {
                Content = content,
                ContentId = content.Id,
                IsContentAdministrator = User.IsInRole("artikel-administrator")
            };

            return View("~/Areas/mitarbeit/Views/Content/Urls.cshtml", model);
        }

        /// <summary>
        /// Add a Url
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Urls(ContentUrlsViewModel model)
        {
            if (!User.IsInRole("artikel-administrator"))
            {
                return RedirectToRoute("ContentIndex");
            }

            var content = _entities.Contents
                .Include(c => c.Urls)
                .FirstOrDefault(c => c.Id == model.ContentId);

            if (content == null)
            {
                return RedirectToRoute("ContentIndex");
            }

            if (ModelState.IsValid)
            {
                // remove leading and trailing slashes
                model.UrlName = model.UrlName.Trim(' ', '/');

                var urls = _entities.ContentUrls.Where(u => u.Url == model.UrlName).Include("Content");

                if (!urls.Any() || urls.First().Enabled == false)
                {
                    // Remove Urls with the same name, if they were disabled before
                    _entities.ContentUrls.RemoveRange(urls);

                    var url = new ContentUrl
                    {
                        Url = model.UrlName,
                        ContentId = content.Id,
                        Type = model.SetDefault ? UrlType.Default : UrlType.Alternative,
                        Enabled = true
                    };

                    if (model.SetDefault)
                    {
                        if (content.DefaultUrl != null)
                        {
                            content.DefaultUrl.Type = UrlType.Alternative;
                        }
                        content.DefaultUrl = url;
                    }

                    _entities.ContentUrls.Add(url);
                    _entities.SaveChanges();

                    model.Saved = true;
                }
                else
                {
                    ViewBag.Error = String.Format("Die URL {0} existiert bereits.", model.UrlName);
                }
            }

            model.Content = content;
            model.IsContentAdministrator = User.IsInRole("artikel-administrator");

            return View("~/Areas/mitarbeit/Views/Content/Urls.cshtml", model);
        }
    }
}