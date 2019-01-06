using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;
using Pokefans.Data;
using Pokefans.Util;

namespace Pokefans.Controllers
{
    public class UserApiController : Controller
    {

        Entities db;

        public UserApiController(Entities ents)
        {
            db = ents;
        }

        [Authorize]
        [AllowCors]
        public ActionResult Notifications(int start = 0)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var notifications = db.UserNotifications.Where(x => x.UserId == uid).OrderByDescending(x => x.Id).Skip(start).Take(25);

            // set notifications read directly in the database
            db.Database.ExecuteSqlCommand("UPDATE UserNotifications SET IsUnread=0 WHERE UserId = @p0;", new MySqlParameter("p0", uid));

            return View("~/Views/Api/Notifications.cshtml", notifications);
        }
    }
}
