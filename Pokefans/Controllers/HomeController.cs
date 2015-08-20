using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokefans.Util;

namespace Pokefans.Controllers
{
    public class HomeController : Controller
    {
        IBreadcrumbs breadcrumbs;

        public HomeController()
        {
        }

        public HomeController(IBreadcrumbs crumbs)
        {
            this.breadcrumbs = crumbs;
        }

        public ActionResult Index()
        {
            breadcrumbs.Add("Startseite");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}