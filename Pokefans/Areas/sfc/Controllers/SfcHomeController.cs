// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Pokefans.Data;
using Pokefans.Util;
using Pokefans.SystemCache;
using Pokefans.Areas.sfc.Models;

namespace Pokefans.Areas.sfc.Controllers
{
    public class SfcHomeController : Controller
    {
        private Cache cache;
        private Entities db;

        public SfcHomeController(Cache c, Entities ents)
        {
            cache = c;
            db = ents;
        }

        public ActionResult Index()
        {
            List<Content> news;
            if (!cache.Contains("sfcNews"))
            {
                news = (from s in db.Contents.Include("Author")
                                    where s.Type == ContentType.News
                                        && s.Category.AreaName == "sfc"
                                        && s.Status == ContentStatus.Published
                                    select s).OrderBy(s => s.Published).Take(4).ToList();
                cache.Add<List<Content>>("sfcNews", news, TimeSpan.FromDays(1));
            }
            else
            {
                news = cache.Get<List<Content>>("sfcNews");
            }

            return View("~/Areas/sfc/Views/Home/Index.cshtml", news);
        }

        public ActionResult Archive(long start)
        {
            NewsArchiveViewModel navm = new NewsArchiveViewModel();

            navm.News = (from s in db.Contents.Include("Author")
                                  where s.Type == ContentType.News
                                      && s.Category.AreaName == "sfc"
                                      && s.Status == ContentStatus.Published
                                      && s.Published > new DateTime(start)
                                  select s).OrderBy(g => g.Published).Take(15).ToList();
            
            navm.HasPrev = (db.Contents.Where(s => s.Published < new DateTime(start)).Count() < 5);

            if (navm.News.Count() > 1)
            {
                navm.HasNext = db.Contents.Any(s => s.Published > navm.News.Last().Published);
                if (navm.HasNext)
                {
                    navm.Next = navm.News.Last().Published.Ticks;
                }
            }

            if (navm.HasPrev)
            {
                navm.Prev = db.Contents.Where(s => s.Published < new DateTime(start))
                    .OrderByDescending(s => s.Published)
                    .Skip(15)
                    .Take(1)
                    .First()
                    .Published.Ticks;
            }

            return View("~/Areas/sfc/Views/Home/Archive.cshtml");
        }

        public ActionResult ArCrypt()
        {
            return View("~/Areas/sfc/Views/Home/ArCrypt.cshtml");
        }
    }
}