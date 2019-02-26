using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles="moderator")]
    public class UserListsController : Controller
    {
        Entities db = new Entities();

        // GET: mitarbeit/UserLists
        public ActionResult BbsList()
        {
            return View();
        }

        public ActionResult Posthunter()
        {
            return View();
        }

        public ActionResult Trashhunter()
        {
            return View();
        }

        public ActionResult Multiaccounts()
        {
            return View();
        }

        public ActionResult Bans()
        {
            var model = db.Users.Where(g => g.IsLockedOut && (g.LockedOutDate == null || g.LockedOutDate > DateTimeOffset.Now));
            return View(model);
        }
    }
}