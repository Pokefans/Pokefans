using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;
using Pokefans.Data;

namespace Pokefans.Controllers
{
    public class NotificationsController : Controller
    {
        Entities db;

        public NotificationsController(Entities ents)
        {
            db = ents;
        }

        [Authorize]
        public ActionResult ViewNotifications(int start = 0)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            ViewBag.HasMore = db.UserNotifications.Where(x => x.UserId == uid).Count() > (start + 25);

            var notifications = db.UserNotifications.Where(x => x.UserId == uid).OrderByDescending(x => x.Id).Skip(start).Take(25);

            // set notifications read directly in the database
            db.Database.ExecuteSqlCommand("UPDATE UserNotifications SET IsUnread=0 WHERE UserId = @p0;", new MySqlParameter("p0", uid));

            return View("~/Areas/user/Views/Notifications/ViewNotifications.cshtml", notifications);
        }
    }
}
