// Copyright 2015-2016 the pokefans authors. See copying.md for legal info
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokefans.Data;
using Pokefans.Models;
using Pokefans.Util;
using System.Configuration;
using Pokefans.Caching;

namespace Pokefans.Controllers
{
    public class HomeController : Controller
    {
        IBreadcrumbs breadcrumbs;
        private readonly Entities _entities;
        Cache cache;

        public HomeController()
        {
        }

        public HomeController(IBreadcrumbs crumbs, Entities entities, Cache c)
        {
            this.breadcrumbs = crumbs;
            _entities = entities;
            cache = c;
        }

        public ActionResult Index()
        {
            breadcrumbs.Add("Startseite");

            MainPageViewModel mpvm = new MainPageViewModel();
            if (!cache.Contains("StartTeaser"))
            {
                int teaserid = int.Parse(ConfigurationManager.AppSettings["StartTeaserId"]);
                mpvm.StartTeaser = _entities.Contents.FirstOrDefault(g => g.Id == teaserid);
                if (mpvm.StartTeaser != null)
                    cache.Add("StartTeaser", mpvm.StartTeaser, TimeSpan.FromDays(1));
            }
            else
            {
                mpvm.StartTeaser = cache.Get<Content>("StartTeaser");
            }

            if (!cache.Contains("StartRecommendations"))
            {
                int recommendationsid = int.Parse(ConfigurationManager.AppSettings["StartRecommendationsId"]);
                mpvm.Recommendations = _entities.Contents.FirstOrDefault(g => g.Id == recommendationsid);
                if (mpvm.Recommendations != null)
                    cache.Add("StartRecommendations", mpvm.Recommendations, TimeSpan.FromDays(1));
            }
            else
            {
                mpvm.Recommendations = cache.Get<Content>("StartRecommendations");
            }

            if (!cache.Contains("StartTrivia"))
            {
                int triviaid = int.Parse(ConfigurationManager.AppSettings["StartTriviaId"]);
                mpvm.Trivia = _entities.Contents.FirstOrDefault(g => g.Id == triviaid);
                if (mpvm.Trivia != null)
                    cache.Add("StartTrivia", mpvm.Trivia, TimeSpan.FromDays(1));
            }
            else
            {
                mpvm.Trivia = cache.Get<Content>("StartTrivia");
            }

            mpvm.News = _entities.Contents.Where(g => g.Type == ContentType.News).OrderByDescending(g => g.Published).Take(10).ToList();
            mpvm.LatestArticles = _entities.Contents.Where(g => g.Type == ContentType.Article).OrderByDescending(g => g.Published).Take(10).ToList();
            mpvm.Fanarts = _entities.Fanarts.OrderByDescending(g => g.UploadTime).Take(4 * 4).ToList();

            return View(mpvm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ViewContent(int contentId)
        {
            var content = _entities.Contents
                .FirstOrDefault(c => c.Id == contentId);

            if (content == null)
            {
                return HttpNotFound();
            }

            var allNews = _entities.Contents
                .Where(c => c.Type == ContentType.News)
                .Where(c => c.Status == ContentStatus.Published)
                .Where(c => c.Category.AreaName != "sfc") //TODO: Consider adding a Code Handle to the data class for this. Referring to the Name feels kinda hacky
                .OrderByDescending(c => c.Published);

            // TODO: The LINQs here use .ToList() because otherwise MySQL would throw an exception, which I couldn't find a solution for.
            // [MySqlException (0x80004005): There is already an open DataReader associated with this Connection which must be closed first.]
            var model = new ContentViewModel
            {
                Content = content,
                News = allNews.Take(8).ToList(),
                RelatedNews = allNews
                    .Where(c => c.CategoryId == content.CategoryId)
                    .Take(8)
                    .ToList()
            };

            content.TrackView(Request, _entities);

            return View("~/Views/Home/ViewContent.cshtml", model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}