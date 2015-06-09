using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles="mitarbeiter")]
    public class DashboardController : Controller
    {
        // GET: mitarbeit/Dashboard
        public ActionResult Index()
        {
            return View("~/Areas/mitarbeit/Views/Dashboard/Index.cshtml");
        }
    }
} 