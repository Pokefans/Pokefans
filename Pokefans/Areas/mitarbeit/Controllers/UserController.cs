using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokefans.Areas.mitarbeit.Models;
using Pokefans.Data;
using Pokefans.Security;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles="moderator")]
    public class UserController : Controller
    {
        private ApplicationUserManager userManager;

        public UserController(ApplicationUserManager umgr)
        {
            userManager = umgr;
        }

        // GET: mitarbeit/User
        public ActionResult Index()
        {
            return View("~/Areas/mitarbeit/Views/User/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserSerachViewModel usvm)
        {
            int uid;
            if(int.TryParse(usvm.UserToFind, out uid))
            {
                return RedirectToAction("Notes", new { id = uid });
            }
            else
            {
                User u = userManager.FindByNameAsync(usvm.UserToFind).Result;
                if(u != null)
                {
                    return RedirectToAction("Notes", new { id = u.Id });
                }
                ViewBag.Error = "Der angegebene Benutzer konnte leider nicht gefunden werden. Vielleicht hast du dich vertippt?";
            }

            return View("~/Areas/mitarbeit/Views/User/Index.cshtml");
        }

        public ActionResult Notes(int id)
        {
            return View("~/Areas/mitarbeit/Views/User/Notes.cshtml");
        }
    }
}