// Copyright 2017 the pokefans authors. See copyright.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Pokefans.Data.UserData;
using Pokefans.Data;
using Microsoft.AspNet.Identity;
using System.Security.Claims;


namespace Pokefans.Areas.user.Controllers
{
	[Authorize]
    public class ProfileController : Controller
    {
		Entities db;

        public ProfileController(Entities ents) {
            db = ents;
        }

        public ActionResult Index() {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var profile = db.UserProfile.Include("User").Where(x => x.UserId == uid).FirstOrDefault();

            return View("~/Areas/user/Views/Profile/ViewProfile.cshtml", profile);
        }

        public ActionResult ViewProfile(string url) {
            // We need to get a UID first.
            int uid = -1;

            if(!int.TryParse(url, out uid)) {
                var nuid = db.Users.Where(x => x.Url == url).FirstOrDefault();
                if(nuid == null)
                    return View("~/Areas/user/Views/Profile/ViewProfile.cshtml", null);
                uid = nuid.Id;
            }
			// uid is now guaranteed to be different to -1: either it was numeric in the first place,
			// then it's set and it's fine and dandy.
			// or we have - at this point - found a user which corresponds to this name.
            // if the id is invalid, the following query will return null, which
            // in turn will trigger the unknown user message to be displayed.

			var profile = db.UserProfile.Include("User").Where(x => x.UserId == uid).FirstOrDefault();

			return View("~/Areas/user/Views/Profile/ViewProfile.cshtml", profile);
        }
    }
}
