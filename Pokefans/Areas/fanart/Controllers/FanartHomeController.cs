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

namespace Pokefans.Areas.fanart.Controllers
{
    public enum FanartIndex { Popular, New };

    public class FanartHomeController : Controller
    {
        private Entities db;
        private Cache cache;

        public FanartHomeController(Entities ents, Cache c)
        {
            db = ents;
            cache = c;
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

            return View("~/Areas/fanart/Views/FanartHome/Index.cshtml", fivm);
        }

        public ActionResult Popular()
        {
            int id = int.Parse(ConfigurationManager.AppSettings["FanartTeaserArticle"]);
            FanartIndexViewModel fivm = new FanartIndexViewModel();

            fivm.Teaser = db.Contents.Where(g => g.Id == id).FirstOrDefault();
            fivm.Fanarts = db.Fanarts.Where(g => g.Status == FanartStatus.OK).OrderByDescending(g => g.Id).Take(8 * 8).ToList();

            return View("~/Areas/fanart/Views/FanartHome/Index.cshtml", fivm);
        }

        public ActionResult ListApi(string index, int start = 0)
        {
            FanartIndex fi = string.IsNullOrEmpty(index) ? FanartIndex.New : (FanartIndex)Enum.Parse(typeof(FanartIndex), index, true);

            switch (fi)
            {
                case FanartIndex.Popular:
                    return Json(new List<Fanart>());
                case FanartIndex.New: // fall through
                default:
                    var fanarts = db.Fanarts.Where(g => g.Status == FanartStatus.OK).OrderByDescending(g => g.Id).Skip(start).Take(8 * 8); // 8 images per column, 8 rows per default, without any filter
                    return Json(fanarts);
            }
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