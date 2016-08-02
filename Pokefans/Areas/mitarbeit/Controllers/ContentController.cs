// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.IO;
using DiffMatchPatch;
using Microsoft.AspNet.Identity;
using PagedList;
using Pokefans.Areas.mitarbeit.Models;
using Pokefans.Data;
using Pokefans.Util;
using System.Configuration;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles = "mitarbeiter")]
    public class ContentController : Controller
    {
        /// <summary>
        /// Entities instance for database access
        /// </summary>
        private readonly Entities _entities;

        private readonly ContentManager _contentManager;

        public ContentController(Entities entities, ContentManager contentManager)
        {
            _entities = entities;
            _contentManager = contentManager;
        }

        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (this.HttpContext.Request.Params["error"] != null)
            {
                switch (this.HttpContext.Request.Params["error"])
                {
                    case "not-found":
                        ViewBag.Error = "Der gewählte Artikel konnte nicht gefunden werden.";
                        break;
                    case "permissions":
                        ViewBag.Error = "Du hast keine ausreichenden Berechtigungen, um diesen Artikel zu bearbeiten.";
                        break;
                    default:
                        ViewBag.Error = "Beim Zugriff auf den Artikel ist ein Fehler aufgetreten " +
                                        this.HttpContext.Request.Params["error"];
                        break;
                }
            }

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
                    model.Filter += $" {model.AdditionalFilter}";
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
                    model.Filter,
                    model.Page
                });

                return Redirect(url?.Replace("%3A", ":").Replace("%20", "+"));
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
                return RedirectToRoute("ContentIndex", new { error = "not-found" });
            }

            if (content.EditPermission != null && !User.IsInRole(content.EditPermission.Name))
            {
                return RedirectToRoute("ContentIndex", new { error = "permissions" });
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
            var currentUser = _entities.Users.Find(User.Identity.GetUserId<int>());

            Content content = null;
            if (ModelState.IsValid)
            {
                if (model.ContentId == null)
                {
                    // Create the content if it does not exist.
                    content = _contentManager.CreateContent(currentUser);
                    model.ContentId = content.Id;
                }
                else
                {
                    // Try to find the existing content object
                    var result = _entities.Contents
                        .Include("EditPermission")
                        .Include("Boilerplates")
                        .Include("BoilerplatesUsed")
                        .Include("Versions")
                        .Where(c => c.Id == model.ContentId);

                    if (result.Any())
                    {
                        content = result.First();

                        if (content.EditPermission != null && !User.IsInRole(content.EditPermission.Name))
                        {
                            return RedirectToRoute("ContentIndex", new { error = "permissions" });
                        }
                    }
                    else
                    {
                        return RedirectToRoute("ContentIndex", new { error = "not-found" });
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
                            // Default url is /news/-/<id>
                            model.Url = "-";
                        }
                    }

                    if (!string.IsNullOrEmpty(model.Url))
                    {
                        model.Url = model.Url.Trim('/');
                        if (model.Type == ContentType.News && !Regex.IsMatch(model.Url, @"news/[a-zA-Z0-9_\-/]+/\d+"))
                        {
                            model.Url = $"news/{model.Url}/{content.Id}";
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
                model.UpdateContentMeta(content);
                content.Updated = DateTime.Now;

                try
                {
                    content.Parse();
                }
                catch (InvalidOperationException exception)
                {
                    ViewBag.Error =
                        "Der Inhalt deines Artikels enthält Fehler und konnte daher nicht gespeichert werden. Bitte überprüfe Inside-Codes und BB-Codes auf ihre Richtigkeit. " +
                        exception.Message;
                    model.Saved = false;
                    goto Return;
                }

                try
                {
                    content.CompileLess();
                    System.IO.File.WriteAllText(Path.Combine(ConfigurationManager.AppSettings["ContentStylesheetPath"], content.Id + ".css"), content.StylesheetCss);
                }
                catch (ArgumentException)
                {
                    ViewBag.Error =
                        "Dein artikelspezifischer CSS-Code enthält Fehler und wurde daher nicht in den Artikel übernommen (aber in der Datenbank gespeichert).";
                }
                catch (UnauthorizedAccessException)
                {
                    ViewBag.Error = "Fehler beim Speichern des artikelspezifischen Stylesheets (dein Code ist aber in Ordnung).";
                }

                _contentManager.UpdateContentVersions(content, currentUser);
            }

            model.Saved = ModelState.IsValid;

Return:
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

            var trackerSources = _entities.ContentTrackingSources
                .Where(s => s.ContentId == content.Id)
                .Where(s => !string.IsNullOrEmpty(s.SourceHost))
                .GroupBy(s => s.SourceHost)
                .Select(grp => new ContentStatisticsHostEntry
                {
                    Host = grp.Key,
                    Count = grp.Count(),
                    Urls = grp
                        .GroupBy(g => g.SourceUrl)
                        .Select(g => new ContentStatisticsLinkEntry
                        {
                            Url = g.Key,
                            Count = g.Count()
                        })
                        .OrderByDescending(g => g.Count),
                })
                .OrderByDescending(g => g.Count);

            var sources = _entities.ContentTrackingSources
                .Where(s => s.ContentId == content.Id)
                .Where(s => !string.IsNullOrEmpty(s.SourceHost))
                .GroupBy(s => new { s.SourceHost, s.SourceUrl })
                .OrderBy(s => s.Key.SourceHost);

            var trackerSources2 = sources
                .GroupBy(s => s.Key.SourceHost)
                .Select(s => new ContentStatisticsHostEntry
                {
                    Host = s.Key,
                    Count = s.Sum(x => x.Count()),
                    Urls = sources
                        .Where(x => x.Key.SourceHost == s.Key)
                        .Select(x => new ContentStatisticsLinkEntry
                        {
                            Url = x.Key.SourceUrl,
                            Count = x.Count()
                        })
                });

            var model = new ContentStatisticsViewModel
            {
                Content = content,
                ViewCount = content.ViewCount,
                TrackerSources = trackerSources2
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
                        Text = $"{c.Title} ({c.Id})",
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
                        if (content.DefaultUrl != null && content.DefaultUrl.Type != UrlType.System)
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

        /// <summary>
        /// Display a content preview
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public ActionResult Preview(int? contentId)
        {
            var content = _entities.Contents
               .Include(c => c.Urls)
               .FirstOrDefault(c => c.Id == contentId);

            if (content == null)
            {
                return RedirectToRoute("ContentIndex");
            }


            var model = new ContentPreviewViewModel
            {
                Title = content.Title,
                ContentId = content.Id,
                Contents = content.ParsedContent,
                Stylesheet = content.StylesheetCss,
                Teaser = content.Teaser
            };

            return View("~/Areas/mitarbeit/Views/Content/Preview.cshtml", model);
        }

        /// <summary>
        /// Display a content preview
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Preview(ContentEditViewModel editModel)
        {
            Content content = null;
            if (editModel.ContentId != null)
            {
                content = _entities.Contents
                    .Include(c => c.Urls)
                    .FirstOrDefault(c => c.Id == editModel.ContentId);
            }

            if (content == null)
            {
                content = new Content();
            }

            editModel.UpdateContentMeta(content);
            content.CompileLess();
            content.Parse();

            var model = new ContentPreviewViewModel
            {
                Title = content.Title,
                ContentId = editModel.ContentId ?? 0,
                Contents = content.ParsedContent,
                Stylesheet = content.StylesheetCss,
                Teaser = content.Teaser
            };


            return View("~/Areas/mitarbeit/Views/Content/Preview.cshtml", model);
        }
    }
}