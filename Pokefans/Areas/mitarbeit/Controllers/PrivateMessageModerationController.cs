using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Pokefans.Data.UserData;

namespace Pokefans.Controllers
{
    [Authorize(Roles = "global-moderator")]
    public partial class ModerationController : Controller
    {
        public ActionResult PmReportOverview(int? start, string user, string topic)
        {
            var reports = db.PrivateMessageReports.Include(q => q.PrivateMessage)
                            .Include(q => q.Reporter)
                            .Include(q => q.From);

            reports = reports.OrderByDescending(x => x.Timestamp);

            bool is_admin = User.IsInRole("superadmin");

            List<PrivateMessageReport> reps = new List<PrivateMessageReport>();

            // Resolved reports older than 30 days are only visible by Admins.
            // Reports older than 90 days are deleted, as long as they are resolved.
            foreach(var report in reports)
            {
                if (report.Resolved && report.Timestamp < DateTime.Now.AddDays(-30) && !is_admin)
                    continue;

                if (report.Resolved && report.Timestamp < DateTime.Now.AddDays(-90)) 
                {
                    db.PrivateMessageReports.Remove(report);
                }

                reps.Add(report);
            }
            db.SaveChanges();


            return View("~/Areas/mitarbeit/Views/Moderation/PmReport/Overview.cshtml", reps);
        }

        public ActionResult ViewPMReport(int id) 
        {
            var report = db.PrivateMessageReports.Include(q => q.Reporter).Include(g => g.From).Include(g => g.PrivateMessage).FirstOrDefault(x => x.Id == id);

            if (report == null)
                return HttpNotFound();

            if (report.Resolved && report.Timestamp < DateTime.Now.AddDays(-30) && !User.IsInRole("superadmin"))
                return View("~/Areas/mitarbeit/Views/Moderation/PmReport/Hidden.cshtml");

            if (report.Resolved && report.Timestamp < DateTime.Now.AddDays(-90)) 
            {
                db.PrivateMessageReports.Remove(report);
                db.SaveChanges();
                return HttpNotFound();
            }

            return View("~/Areas/mitarbeit/Views/Moderation/PmReport/View.cshtml", report);
        }
    }
}
