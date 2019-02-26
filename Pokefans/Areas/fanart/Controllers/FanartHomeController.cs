// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Data;
using Pokefans.Data.Fanwork;
using Pokefans.Caching;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokefans.Areas.fanart.Models;
using Pokefans.Util.Comments;
using Pokefans.Models;
using Pokefans.Security;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Pokefans.Util;

namespace Pokefans.Areas.fanart.Controllers
{
    public class FanartHomeController : Controller
    {
        private Entities db;
        private Cache cache;
        private ApplicationUserManager userManager;
        private Searcher searcher;
        private Analyzer analyzer;
        private NotificationManager notificationManager;

        public FanartHomeController(Entities ents, Cache c, ApplicationUserManager um, Searcher srch, Analyzer ana, NotificationManager mgr)
        {
            db = ents;
            cache = c;
            userManager = um;
            searcher = srch;
            analyzer = ana;
            notificationManager = mgr;
        }

        public ActionResult UserRss(int id)
        {
            User u = db.Users.FirstOrDefault(g => g.Id == id);
            if (u != null)
            {
                List<Fanart> fanarts = db.Fanarts.Include("Category").Where(g => g.UploadUserId == id).OrderByDescending(g => g.UploadTime).Take(50).ToList();
                if (fanarts.Count > 0)
                {
                    ViewBag.User = u;
                    Response.ContentType = "application/rss+xml";
                    return View("~/Areas/fanart/Views/FanartHome/UserRss.cshtml", fanarts);
                }
            }
            return new HttpNotFoundResult();
        }

        public ActionResult CategoryRss(string id)
        {
            FanartCategory c = db.FanartCategories.FirstOrDefault(g => g.Uri == id);
            if (c != null)
            {
                List<Fanart> fanarts = db.Fanarts.Include("User").Where(g => g.CategoryId == c.Id).OrderByDescending(g => g.UploadTime).Take(50).ToList();
                if (fanarts.Count > 0)
                {
                    ViewBag.Category = c;
                    Response.ContentType = "application/rss+xml";
                    return View("~/Areas/fanart/Views/FanartHome/CategoryRss.cshtml");
                }
            }

            return new HttpNotFoundResult();
        }

        public ActionResult Category(string category)
        {
            throw new NotImplementedException();
        }

        public ActionResult Rss()
        {
            List<Fanart> fanarts = db.Fanarts.Include("User").Include("Category").OrderByDescending(g => g.UploadTime).Take(50).ToList();
            if (fanarts.Count > 0)
            {
                Response.ContentType = "application/rss+xml";
                return View("~/Areas/fanart/Views/FanartHome/Rss.cshtml");
            }

            return new HttpNotFoundResult();
        }

        public ActionResult New()
        {
            int id = int.Parse(ConfigurationManager.AppSettings["FanartTeaserArticle"]);
            FanartIndexViewModel fivm = new FanartIndexViewModel();
            fivm.Teaser = db.Contents.Where(g => g.Id == id).FirstOrDefault();

            fivm.Fanarts = db.Fanarts.Where(g => g.Status == FanartStatus.OK).OrderByDescending(g => g.Id).Take(8 * 8).ToList();

            ViewBag.FanartCatUrls = cache.Get<Dictionary<int, string>>("FanartUrls");

            return View("~/Areas/fanart/Views/FanartHome/Index.cshtml", fivm);
        }

        public ActionResult Search(string term, int page = 1)
        {
            FanartSearchPageViewModel fspvm = new FanartSearchPageViewModel();

            fspvm.CurrentPage = page > 0 ? page : 1;

            // if no explicit fields are entered, search these fields
            MultiFieldQueryParser mfqp = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,
                new string[] { "title", "user", "userid", "rating", "category", "tag", "description" },
                analyzer);

            Query searchQuery = mfqp.Parse(term);

            BooleanQuery finalQuery = new BooleanQuery();

            finalQuery.Add(new BooleanClause(searchQuery, Occur.MUST));
            finalQuery.Add(new BooleanClause(new TermQuery(new Term("type", "fanart")), Occur.MUST)); // this ensures we only get fanart results.

            // cap at 5k search results. this is somewhat arbitrarily limited, but should be more than enough to cover all searches.
            TopDocs docs = searcher.Search(finalQuery, 5000);

            fspvm.Pages = (int)Math.Ceiling(docs.TotalHits / (8M * 8));
            fspvm.Results = new List<FanartSearchViewModel>();
            fspvm.TotalResults = docs.TotalHits;

            // because lucene is somewhat a java api, we need to do things a bit differently.
            // let's calculate the start and end index first.
            int start = (fspvm.CurrentPage - 1) * (8 * 8);
            int end = fspvm.CurrentPage * (8 * 8);

            if (end > docs.TotalHits)
                end = docs.TotalHits;

            Dictionary<int, string> categoryUrls = cache.Get<Dictionary<int, string>>("FanartUrls");

            for(int i = start; i < end; i++)
            {
                Document hit = searcher.Doc(docs.ScoreDocs[i].Doc);
                fspvm.Results.Add(new FanartSearchViewModel()
                {
                    DetailUrl = "/" + categoryUrls[int.Parse(hit.Get("categoryId"))] + "/" + hit.Get("Id"),
                    Id = int.Parse(hit.Get("Id")),
                    ThumbnailUrl = hit.Get("smallThumbnailUrl")
                });
            }

            fspvm.SearchTerm = term;

            return View("~/Areas/fanart/Views/FanartHome/Search.cshtml", fspvm);
        }

        public ActionResult Single(int id)
        {
            // TODO: Optimize
            Fanart fanart = db.Fanarts.Include("UploadUser")
                .Include("Tags")
                .Include("Tags.Tag")
                .FirstOrDefault(x => x.Id == id);

            if (fanart == null)
            {
                return new HttpNotFoundResult();
            }

            FanartSingleViewModel fsvm = new FanartSingleViewModel();
            FanartCommentManager fcm = new FanartCommentManager(db, cache, HttpContext, notificationManager);
            CommentsViewModel cvm = new CommentsViewModel();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User currentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
                cvm.CanHideComment = currentUser.IsInRole("fanart-moderator", cache, db);
                cvm.CurrentUser = currentUser;
                fsvm.IsRatingActive = true;
            }
            else
            {
                cvm.CanHideComment = false;
                cvm.CurrentUser = null;
                fsvm.IsRatingActive = false;
            }

            cvm.Comments = fcm.GetCommentsForObjectId(id);
            cvm.CommentedObjectId = id;
            cvm.Context = CommentContext.Fanart;

            cvm.Level = 0; //note that this is the level we start from, so we can arbitrarily limit comment indentation between 0 and 4 levels
            cvm.Manager = fcm;

            fsvm.Comments = cvm;
            fsvm.Fanart = fanart;
            fsvm.Categories = cache.Get<Dictionary<int, string>>("FanartUrls");
            fsvm.CategoriesName = cache.Get<Dictionary<int, string>>("FanartCategories");

            // get three related fanarts, randomly.
            // related == shares a tag.
            // maybe we could do better with a search backend with proper ranking,
            // but I want some randomness in here.
            List<int> tagIds = fanart.Tags.Select(x => x.TagId).ToList();
            // MySQL does not have the NewGuid Function. So we can either create it (clean solution) or do the randomness
            // in code (ghetto solution).
            List<int> fanartIds = db.FanartsTags.Where(x => tagIds.Contains(x.TagId) && x.FanartId != fanart.Id).Select(x => x.FanartId).ToList().OrderBy(x => Guid.NewGuid()).Take(3).ToList();
            fsvm.Related = db.Fanarts.Where(x => fanartIds.Contains(x.Id)).ToList();

            // todo: next and previous URIs ?

            return View("~/Areas/fanart/Views/FanartHome/Single.cshtml", fsvm);
        }

        public ActionResult Random()
        {
            int fanartid = db.Database.SqlQuery<int>(@"SELECT t.Id FROM Fanarts t 
                                                      JOIN (
                                                         SELECT(FLOOR(max(Id) * rand())) as maxid FROM Fanarts
                                                      ) as tt on t.id >= tt.maxid 
                                                     LIMIT 1").First();

            Fanart f = db.Fanarts.Find(fanartid);
            Dictionary<int, string> fcats = cache.Get<Dictionary<int,string>>("FanartUrls");

            return Redirect(Url.Map(fcats[f.CategoryId]+"/"+f.Id, "fanart"));
        }

        public ActionResult ListApi(int start = 0)
        {
            var fanarts = db.Fanarts.Where(g => g.Status == FanartStatus.OK).OrderByDescending(g => g.Id).Skip(start).Take(8 * 8); // 8 images per column, 8 rows per default, without any filter
            return Json(fanarts);
        }

        private void SetCookie(string key, string value)
        {
            if (this.HttpContext.Response.Cookies.AllKeys.Contains(value))
            {
                this.HttpContext.Response.Cookies[key].Value = value;
            }
            else
            {
                this.HttpContext.Response.Cookies.Add(new HttpCookie(key, value));
            }
        }
    }
}