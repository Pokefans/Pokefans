using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Pokefans.Areas.mitarbeit.Models;
using Pokefans.Areas.mitarbeit.Models.Dashboard;
using Pokefans.Data;

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
            var dashboard = new DashboardViewModel();

            if(User.IsInRole("global-moderator"))
                dashboard.PMReport = PrivateMessagReports();


            return View("~/Areas/mitarbeit/Views/Dashboard/Index.cshtml", dashboard);
        }

        public PrivateMessageReportDashboardViewModel PrivateMessagReports() 
        {
            var report = new PrivateMessageReportDashboardViewModel();

            report.Reports = db.PrivateMessageReports.Include(q => q.PrivateMessage).Include(q => q.Reporter).Include(q => q.From).Where(x => x.Resolved == false).OrderByDescending(x => x.Timestamp).Take(5).ToList();

            DateTime current = DateTime.Now.AddDays(-30);

            var reportsOfLastMonth = db.PrivateMessageReports.Where(x => x.Timestamp > current).ToList();

            report.ReportsPerDay = new Dictionary<DateTime, int>();
            while(current < DateTime.Now) 
            {
                report.ReportsPerDay.Add(current, reportsOfLastMonth.Where(x => x.Timestamp.DayOfYear == current.DayOfYear).Count());
                current = current.AddDays(1);
            }

            report.Open = db.PrivateMessageReports.Count(x => x.Resolved == false);

            return report;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resolve(int id, string notes) 
        {
            return HttpNotFound();
        }
    }
} 