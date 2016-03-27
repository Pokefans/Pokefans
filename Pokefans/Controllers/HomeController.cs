using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokefans.Data;
using Pokefans.Models;
using Pokefans.Util;
using System.Configuration;

namespace Pokefans.Controllers
{
    public class HomeController : Controller
    {
        IBreadcrumbs breadcrumbs;
        private readonly Entities _entities;

        public HomeController()
        {
        }

        public HomeController(IBreadcrumbs crumbs, Entities entities)
        {
            this.breadcrumbs = crumbs;
            _entities = entities;
        }

        public ActionResult Index()
        {
            breadcrumbs.Add("Startseite");

            MainPageViewModel mpvm = new MainPageViewModel();
            int teaserid = int.Parse(ConfigurationManager.AppSettings["StartTeaserId"]);
            int recommendationsid = int.Parse(ConfigurationManager.AppSettings["StartRecommendationsId"]);
            int triviaid = int.Parse(ConfigurationManager.AppSettings["StartTriviaId"]);

            mpvm.StartTeaser = _entities.Contents.FirstOrDefault(g => g.Id == teaserid);
            mpvm.Recommendations = _entities.Contents.FirstOrDefault(g => g.Id == recommendationsid);
            mpvm.Trivia = _entities.Contents.FirstOrDefault(g => g.Id == triviaid);

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