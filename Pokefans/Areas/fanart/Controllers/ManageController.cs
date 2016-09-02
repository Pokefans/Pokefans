// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Data;
using Pokefans.Data.Fanwork;
using Pokefans.Caching;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using Pokefans.Security;
using System.Web.Routing;
using Amib.Threading;
using System.IO;
using System.Configuration;
using System.Drawing.Drawing2D;
using Pokefans.Areas.fanart.Models;
using Microsoft.AspNet.Identity;
using Pokefans.Util.Parser;
using Lucene.Net.Analysis;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Pokefans.Util.Search;

namespace Pokefans.Areas.fanart.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        Entities db;
        Cache cache;
        SmartThreadPool threadpool;
        IndexWriter writer;
        Searcher searcher;
        Analyzer analyzer;

        Dictionary<ImageFormat, string> formatExtensions = new Dictionary<ImageFormat, string>{
            { ImageFormat.Gif , ".gif" },
            { ImageFormat.Png, ".png" },
            { ImageFormat.Jpeg, ".jpeg" },
            { ImageFormat.Tiff, ".tiff" }
        };

        public ManageController(Entities ents, Cache c, SmartThreadPool tp, IndexWriter wrt, Searcher srch, Analyzer ana)
        {
            db = ents;
            threadpool = tp;
            cache = c;
            writer = wrt;
            searcher = srch;
            analyzer = ana;
        }
        // GET: fanart/Manage
        public ActionResult Index()
        {
            int currentUserId = User.Identity.GetUserId<int>();
            List<Fanart> fanarts = db.Fanarts.Include("Tags").Include("Tags.Tag").Where(g => g.UploadUserId == currentUserId).OrderByDescending(g => g.UploadTime).ToList();
            ViewBag.Categories = cache.Get<Dictionary<int, string>>("FanartCategories");
            ViewBag.FanartCatUrls = cache.Get<Dictionary<int, string>>("FanartUrls");

            return View("~/Areas/fanart/Views/Manage/Index.cshtml", fanarts);
        }

        public ActionResult GetTags()
        {
            string[] toptags;

            if (!cache.Contains("FanartTop1000Tags"))
            {
                // this is a rather complex query and I'd like to avoid round trips. still, even tough we cache this for 24 hours, 
                // it could still produce a measurable delay.
                // also this breaks if someone was to rename the tables involved, but eh...
                toptags = db.Database.SqlQuery<FanartTag>(
                    "SELECT a.* FROM FanartTags a" +
                    "RIGHT JOIN ( " +
                    "    SELECT TagId, COUNT(DISTINCT TagId) as count FROM FanartsTags" +
                    "    ORDER BY count DESC" +
                    "    LIMIT 1000" +
                    ") b ON b.TagId = a.Id").Select(g => g.Name).ToArray();

                cache.Add("FanartTop1000Tags", toptags, new DateTimeOffset(DateTime.Now, TimeSpan.FromDays(1)));
            }
            else
            {
                toptags = cache.Get<string[]>("FanartTop1000Tags");
            }

            return Json(toptags);
        }

        public ActionResult GetTags(string q)
        {
            //TODO: Let sphinx to this
            var tags = db.FanartTags.Where(g => g.Name.StartsWith(q)).ToArray();

            return Json(tags);
        }

        // POST: verwaltung/einreichung/<id>/loschen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Fanart art = db.Fanarts.FirstOrDefault(g => g.Id == id);

            int cuid = User.Identity.GetUserId<int>();

            if ((art.UploadUserId != cuid || db.FanartBanlist.Any(g => g.UserId == cuid && g.CanDelete == false)) && User.IsInRole("superadmin") == false)
            {
                Response.StatusCode = 403;
                return View("~/Areas/fanart/Views/Errors/Edit403.cshtml");
            }

            // delete images first
            System.IO.File.Delete(art.ImageDiskPath);
            if (art.Size.X > 130 || art.Size.Y > 130)
            {
                System.IO.File.Delete(art.SmallThumbnailDiskPath);
                System.IO.File.Delete(art.LargeThumbnailDiskPath);
            }

            // ToDo: delete Comments
            db.Fanarts.Remove(art);
            db.SaveChanges();

            return new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "FanartHome" }));
        }

        // GET: verwaltung/einreichung/<id>
        public ActionResult Edit(int id)
        {
            Fanart art = db.Fanarts.Include("Category").Include("Tags.Tag").FirstOrDefault(g => g.Id == id);

            int cuid = User.Identity.GetUserId<int>();

            if ((art.UploadUserId != cuid || db.FanartBanlist.Any(g => g.UserId == cuid && g.CanEdit == false)) && User.IsInRole("superadmin") == false)
            {
                Response.StatusCode = 403;
                return View("~/Areas/fanart/Views/Errors/Edit403.cshtml");
            }

            FanartEditViewModel model = new FanartEditViewModel()
            {
                Title = art.Title,
                Description = art.DescriptionCode,
                Order = art.Order,
                Taglist = string.Join(",", art.Tags.Select(g => g.Tag.Name))
            };

            ViewBag.Success = false;
            ViewBag.Id = art.Id;
            ViewBag.Url = art.Url;
            ViewBag.CategoryUrl = art.Category.Uri;
            ViewBag.IsProtected = art.Protect;
            ViewBag.Max = double.Parse(ConfigurationManager.AppSettings["FanartDiskspace"]);
            int currentUserId = User.Identity.GetUserId<int>();
            ViewBag.Usage = db.Fanarts.Any(g => g.UploadUserId == currentUserId) ? db.Fanarts.Where(g => g.UploadUserId == currentUserId).Sum(g => g.FileSize) : 0;

            return View("~/Areas/fanart/Views/Manage/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FanartEditViewModel toUpdate)
        {
            Fanart art = db.Fanarts.Include("Category").Include("UploadUser").FirstOrDefault(g => g.Id == id);

            int cuid = User.Identity.GetUserId<int>();

            if ((art.UploadUserId != cuid || db.FanartBanlist.Any(g => g.UserId == cuid && g.CanEdit == false)) && User.IsInRole("superadmin") == false)
            {
                Response.StatusCode = 403;
                return View("~/Areas/fanart/Views/Errors/Edit403.cshtml");
            }

            art.Title = toUpdate.Title;
            art.Order = (short)toUpdate.Order;

            // cap at +- 1000
            art.Order = art.Order > 1000 ? (short)1000 : art.Order;
            art.Order = art.Order < -1000 ? (short)-1000 : art.Order;

            ParserConfiguration pc = ParserConfiguration.Default;
            pc.EnableInsideCodes = false;
            pc.NewlineToHtml = true;
            pc.EscapeHtml = true;

            var p = new Util.Parser.Parser(pc);

            art.Description = p.Parse(toUpdate.Description);
            art.DescriptionCode = toUpdate.Description;

            db.FanartsTags.RemoveRange(art.Tags);

            if (!string.IsNullOrWhiteSpace(toUpdate.Taglist))
            {
                string[] tags = toUpdate.Taglist.Split(',');


                List<FanartTags> fanartTags = new List<FanartTags>();

                foreach (string tag in tags)
                {
                    FanartTag ftag;

                    if (!db.FanartTags.Any(g => g.Name == tag))
                    {
                        ftag = new FanartTag()
                        {
                            Name = tag
                        };
                        db.FanartTags.Add(ftag);
                        db.SaveChanges();
                    }
                    else
                    {
                        ftag = db.FanartTags.First(g => g.Name == tag);
                    }

                    fanartTags.Add(new FanartTags() { FanartId = art.Id, TagId = ftag.Id });
                }


                db.FanartsTags.AddRange(fanartTags);
            }
            db.SetModified(art);
            db.SaveChanges();

            // re-add the document to the search index
            BooleanQuery query = new BooleanQuery();
            query.Add(new BooleanClause(new TermQuery(new Term("type", "fanart")), Occur.MUST));
            query.Add(new BooleanClause(new TermQuery(new Term("Id", art.Id.ToString())), Occur.MUST));
            writer.DeleteDocuments(query);
            writer.Flush(false, false, true);
            writer.AddDocument(DocumentGenerator.Fanart(art));
            writer.Flush(false, true, false);

            FanartEditViewModel model = new FanartEditViewModel()
            {
                Title = art.Title,
                Description = art.DescriptionCode,
                Order = art.Order,
                Taglist = string.Join(",", art.Tags.Select(g => g.Tag.Name))
            };

            ViewBag.Success = true;
            ViewBag.Id = art.Id;
            ViewBag.Url = art.Url;
            ViewBag.CategoryUrl = art.Category.Uri;
            ViewBag.IsProtected = art.Protect;
            ViewBag.Max = double.Parse(ConfigurationManager.AppSettings["FanartDiskspace"]);
            int currentUserId = User.Identity.GetUserId<int>();
            ViewBag.Usage = db.Fanarts.Any(g => g.UploadUserId == currentUserId) ? db.Fanarts.Where(g => g.UploadUserId == currentUserId).Sum(g => g.FileSize) : 0;

            return View("~/Areas/fanart/Views/Manage/Edit.cshtml", model);
        }


        // GET: verwaltung/upload
        public ActionResult Upload()
        {
            ViewBag.Categories = db.FanartCategories.OrderBy(g => g.Order);
            ViewBag.Error = "";
            ViewBag.Max = double.Parse(ConfigurationManager.AppSettings["FanartDiskspace"]);
            int currentUserId = User.Identity.GetUserId<int>();
            ViewBag.Usage = db.Fanarts.Any(g => g.UploadUserId == currentUserId) ? db.Fanarts.Where(g => g.UploadUserId == currentUserId).Sum(g => g.FileSize) : 0;
            return View("~/Areas/fanart/Views/Manage/Upload.cshtml");
        }

        // POST: verwaltung/upload
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, int categoryId, int licenseId)
        {
            int currentUserId = User.Identity.GetUserId<int>();

            FanartCategory cat = db.FanartCategories.FirstOrDefault(g => g.Id == categoryId);
            try
            {
                if (db.FanartBanlist.Any(g => g.UserId == currentUserId && g.CanUpload == false))
                {
                    throw new ArgumentException("Dein Benutzeraccount wurde für die Benutzung der Fanart-Galerie gesperrt. Bitte wende dich an einen Fanart-Moderator.");
                }

                if (cat == null)
                {
                    throw new ArgumentException("Die gewählte Kategorie ist ungültig.");
                }

                if (file.InputStream.Length > cat.MaxFileSize)
                {
                    throw new ArgumentException(string.Format("Deine Datei ist zu groß für diese Kategorie. Maximal erlaubt sind {} KiB.", cat.MaxFileSize / 1024));
                }

                Image img = Image.FromStream(file.InputStream);

                if (!img.RawFormat.Equals(ImageFormat.Jpeg) &&
                   img.RawFormat.Equals(ImageFormat.Png) &&
                   img.RawFormat.Equals(ImageFormat.Gif) &&
                   img.RawFormat.Equals(ImageFormat.Tiff))
                {
                    throw new ArgumentException("Deine Datei hat ein falsches Format. Erlaubt sind JPG, PNG oder GIF.");
                }

                if (img.Height > cat.MaximumDimension || img.Height > cat.MaximumDimension)
                {
                    throw new ArgumentException("Dein Bild ist zu hoch oder zu breit für diese Kategorie.");
                }

                Fanart art = new Fanart()
                {
                    CategoryId = categoryId,
                    FileSize = (int)file.InputStream.Length,
                    Order = 0,
                    Title = "Einreichung " + file.FileName,
                    Size = new FanartDimension() { X = (short)img.Width, Y = (short)img.Height },
                    Status = FanartStatus.OK,
                    UploadIp = SecurityUtils.GetIPAddressAsString(HttpContext),
                    UploadUserId = currentUserId,
                    UploadTime = DateTime.Now,
                    Protect = false,
                    Description = string.Empty,
                    DescriptionCode = string.Empty
                };

                db.Fanarts.Add(art);
                db.SaveChanges();
                // TODO: figure out a better way to do this whithout a Database roundtrip. Not using Auto Increment is a bad idea tough (race conditions)
                art.Url = "u" + currentUserId.ToString() + "/f" + art.Id + formatExtensions[img.RawFormat];
                db.SetModified(art);
                db.SaveChanges();

                art = db.Fanarts.Include("UploadUser").Include("Tags").Include("Tags.Tag").First(x => x.Id == art.Id);
                writer.AddDocument(DocumentGenerator.Fanart(art));

                // Image saving and Thumbnail generation is done in a seperate thread, so we can redirect the user faster.
                // Usually, the file is saved way before the next request is done, so this only gives some delay under heavy load.
                threadpool.QueueWorkItem((Image i, Fanart a, int cuid) =>
                {
                    string path = Path.Combine(ConfigurationManager.AppSettings["FanartFilePath"], "u" + cuid.ToString());

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string filename = "f" + a.Id.ToString();

                    // we save the image so metadata and invalid data (from exploits for example) is hopefully purged.
                    i.Save(Path.Combine(path, filename + formatExtensions[i.RawFormat]));

                    // if neccessary, generate thumbnails
                    if (a.Size.X > 130 || a.Size.Y > 130)
                    {
                        resizeAndSave(i, Path.Combine(path, filename + "_t2.png"), 260);
                        resizeAndSave(i, Path.Combine(path, filename + "_t4.png"), 130);
                    }

                    // just to be safe, don't forget: this is multi-threaded
                    i.Dispose();

                }, img, art, currentUserId);

                if (Request.IsAjaxRequest())
                {
                    return Json(art.Id);
                }
                else
                {
                    return new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Manage", Action = "Edit", Id = art.Id }));
                }
            }
            catch (ArgumentException e)
            {
                Response.StatusCode = 400;
                if (Request.IsAjaxRequest())
                {
                    return Json(new { error = e.Message });
                }
                else
                {
                    ViewBag.Error = e.Message;
                    ViewBag.Categories = db.FanartCategories.OrderBy(g => g.Order);
                    ViewBag.Max = double.Parse(ConfigurationManager.AppSettings["FanartDiskspace"]);
                    ViewBag.Usage = db.Fanarts.Any(g => g.UploadUserId == currentUserId) ? db.Fanarts.Where(g => g.UploadUserId == currentUserId).Sum(g => g.FileSize) : 0;
                    return View("~/Areas/fanart/Views/FanartHome/Upload.cshtml");
                }
            }
        }
        private void resizeAndSave(Image img, string fileName, int longestEdge)
        {
            Size s = img.Size;

            if (s.Width > longestEdge)
            {
                decimal round = s.Width / longestEdge;
                s.Width = longestEdge;
                s.Height = (int)(s.Height / round);
            }
            if (s.Height > longestEdge)
            {
                decimal round = s.Height / longestEdge;
                s.Height = longestEdge;
                s.Width = (int)(s.Width / round);
            }

            using (Bitmap map = new Bitmap(s.Width, s.Height))
            {
                using (Graphics g = Graphics.FromImage(map))
                {
                    // enhance image quality a bit
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.DrawImage(img, new Rectangle(new Point(0, 0), s));

                    map.Save(fileName, ImageFormat.Png);
                }
            }
        }
    }
}