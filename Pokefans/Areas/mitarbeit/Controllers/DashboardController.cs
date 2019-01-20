using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Pokefans.Areas.mitarbeit.Models;
using Pokefans.Areas.mitarbeit.Models.Dashboard;
using Pokefans.Data;
using Pokefans.Util;
using ChartJSCore.Models;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles="mitarbeiter")]
    public class DashboardController : Controller
    {
        Entities db;

        public DashboardController(Entities entities)
        {
            db = entities;
        }

        // GET: mitarbeit/Dashboard
        public ActionResult Index()
        {
            return View("~/Areas/mitarbeit/Views/Dashboard/Index.cshtml");
        }

        [AllowCors]
        [Authorize(Roles = "wifi-moderator")]
        public ActionResult WifiReportTable()
        {
            var reportsRaw = db.OfferReports
                .Include(x => x.User)
                .Include(x => x.Offer)
                .Include(x => x.Offer.User)
                .Where(x => x.Resolved == false)
                .OrderByDescending(x => x.ReportedOn)
                .Take(5)
                .ToList();

            var reports = new List<object>();

            // we cannot serialize what comes out of the database directly (because of reasons),
            // and we don't want to push all fields anyways. so let's convert it.
            foreach (var x in reportsRaw)
                reports.Add(new
                {
                    title = x.Offer.Title,
                    reportUrl = Url.Action("Reports", "Wifi") + "#" + x.Id.ToString(),
                    user = new
                    {
                        url = x.Offer.User.Url,
                        displayCSS = x.Offer.User.Color,
                        name = x.Offer.User.UserName
                    },
                    reporter = new
                    {
                        url = x.User.Url,
                        displayCSS = x.User.Color,
                        name = x.User.UserName
                    }
                });

            return Json(reports, JsonRequestBehavior.AllowGet);
        }

        [AllowCors]
        [Authorize(Roles = "wifi-moderator")]
        public ActionResult WifiReportChart()
        {
            Chart chart = new Chart();
            chart.Type = "line";

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            DateTime current = DateTime.Now.AddDays(-30);

            var reportsOfLastMonth = db.OfferReports.Where(x => x.ReportedOn > current).ToList();

            var ReportsPerDay = new Dictionary<DateTime, int>();
            while (current < DateTime.Now)
            {
                ReportsPerDay.Add(current, reportsOfLastMonth.Where(x => x.ReportedOn.DayOfYear == current.DayOfYear).Count());
                current = current.AddDays(1);
            }

            data.Labels = ReportsPerDay.Select(x => x.Key.ToString("dd.MM.yyyy")).ToList();
            data.Datasets = new List<Dataset>();

            data.Datasets.Add(new LineDataset()
            {
                Label = "Meldungen",
                Data = ReportsPerDay.Select(x => (double)x.Value).ToList(),
                BackgroundColor = "#dd4b39"
            });
            chart.Options.Legend = new Legend();
            chart.Options.Legend.Display = false;
            chart.Options.Responsive = true;
            chart.Options.MaintainAspectRatio = true;
            chart.Options.Scales = new Scales();
            chart.Options.Scales.YAxes = new List<Scale>();
            chart.Options.Scales.YAxes.Add(new CartesianScale()
            {
                Ticks = new CartesianLinearTick()
                {
                    SuggestedMin = 0,
                    SuggestedMax = 20,
                    StepSize = 2
                }
            });
            chart.Data = data;

            return Content(chart.SerializeBody(), "text/json");
        }

        [AllowCors]
        [Authorize(Roles ="global-moderator")]
        public ActionResult PMReportTable()
        {

            var reportsRaw = db.PrivateMessageReports
                .Include(q => q.PrivateMessage)
                .Include(q => q.Reporter)
                .Include(q => q.From)
                .Where(x => x.Resolved == false)
                .OrderByDescending(x => x.Timestamp)
                .Take(5)
                .ToList();

            var reports = new List<object>();

            // we cannot serialize what comes out of the database directly (because of reasons),
            // and we don't want to push all fields anyways. so let's convert it.
            foreach (var x in reportsRaw)
                reports.Add(new
                {
                    subject = x.PrivateMessage.Subject,
                    reportUrl = Url.Action("ViewPMReport", "PrivateMessageModeration", new { id = x.Id }),
                    user = new
                    {
                        url = x.From.Url,
                        displayCSS = x.From.Color,
                        name = x.From.UserName
                    },
                    reporter = new
                    {
                        url = x.Reporter.Url,
                        displayCSS = x.Reporter.Color,
                        name = x.Reporter.UserName
                    }
                });

            return Json(reports, JsonRequestBehavior.AllowGet);
        }

        [AllowCors]
        [Authorize(Roles="global-moderator")]
        public ActionResult PMReportChart()
        {
            Chart chart = new Chart();
            chart.Type = "line";

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            DateTime current = DateTime.Now.AddDays(-30);

            var reportsOfLastMonth = db.PrivateMessageReports.Where(x => x.Timestamp > current).ToList();

            var ReportsPerDay = new Dictionary<DateTime, int>();
            while (current < DateTime.Now)
            {
                ReportsPerDay.Add(current, reportsOfLastMonth.Where(x => x.Timestamp.DayOfYear == current.DayOfYear).Count());
                current = current.AddDays(1);
            }

            data.Labels = ReportsPerDay.Select(x => x.Key.ToString("dd.MM.yyyy")).ToList();
            data.Datasets = new List<Dataset>();

            data.Datasets.Add(new LineDataset()
            {
                Label = "Meldungen",
                Data = ReportsPerDay.Select(x => (double)x.Value).ToList(),
                BackgroundColor = "#dd4b39"
            });
            chart.Options.Legend = new Legend();
            chart.Options.Legend.Display = false;
            chart.Options.Responsive = true;
            chart.Options.MaintainAspectRatio = true;
            chart.Options.Scales = new Scales();
            chart.Options.Scales.YAxes = new List<Scale>();
            chart.Options.Scales.YAxes.Add(new CartesianScale()
            {
                Ticks = new CartesianLinearTick()
                {
                    SuggestedMin = 0,
                    SuggestedMax = 20,
                    StepSize = 2
                }
            });
            chart.Data = data;

            return Content(chart.SerializeBody(), "text/json");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resolve(int id, string notes) 
        {
            return HttpNotFound();
        }
    }
} 