// Copyright 2015-2016 the pokefans authors. See copying.md for details.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Pokefans.Areas.mitarbeit.Models;
using Pokefans.Data;
using Pokefans.Security;
using Pokefans.Caching;
using Pokefans.Util;
using Ganss.XSS;
using System.Data.Entity.Validation;
using Pokefans.Data.Fanwork;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Pokefans.Data.Base;
using Pokefans.Data.Wifi;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles = "moderator")]
    public class UserController : Controller
    {
        private ApplicationUserManager userManager;
        private Entities db;
        private Cache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="umgr">The umgr.</param>
        public UserController(ApplicationUserManager umgr, Entities ents, Cache c)
        {
            userManager = umgr;
            db = ents;
            cache = c;
        }

        // GET: mitarbeit/User
        public ActionResult Index()
        {
            if (this.HttpContext.Request.Params["error"] != null)
            {
                if (this.HttpContext.Request.Params["error"] == "self")
                {
                    ViewBag.Error = "Du kannst dich nicht selbst verwalten.";
                }
                else if (this.HttpContext.Request.Params["error"] == "higher")
                {
                    ViewBag.Error = "Du kannst keine Moderatoren gleichen oder höheren Levels verwalten.";
                }
                else 
                {
                    ViewBag.Error = "Der angegebene Benutzer konnte leider nicht gefunden werden. Vielleicht hast du dich vertippt?";
                }
            }
            return View("~/Areas/mitarbeit/Views/User/Index.cshtml");
        }

        /// <summary>
        /// Wird aufgerufen, bevor die Aktionsmethode aufgerufen wird.
        /// </summary>
        /// <param name="filterContext">Informationen über die aktuelle Anforderung und Aktion.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(filterContext.ActionParameters.ContainsKey("id") && !filterContext.RequestContext.HttpContext.Request.Path.StartsWith("api")) // api must validate manually, too.
            {
                User current = userManager.FindByNameAsync(User.Identity.Name).Result;
                if(!current.IsInRole("superadmin", cache, db))
                {
                    // todo: fix that
                    int id;
                    if (int.TryParse(filterContext.ActionParameters["id"].ToString(), out id)) // no id? nothing to check.
                    {
                        if (id == current.Id)
                        {
                            filterContext.Result = RedirectToAction("Index", new { error = "self" });
                        }
                        // now check the BVS roles.
                        User u = userManager.FindByIdAsync(id).Result;
                        if(u.IsInRole("bereichsassistent", cache, db) && !u.IsInRole("global-moderator", cache, db))
                        {
                            filterContext.Result = RedirectToAction("Index", new { error = "higher" });
                        }
                        if (u.IsInRole("global-moderator", cache, db) && !u.IsInRole("bereichsleiter", cache, db))
                        {
                            filterContext.Result = RedirectToAction("Index", new { error = "higher" });
                        }
                        if (u.IsInRole("bereichsleiter", cache, db) && !u.IsInRole("administrator", cache, db))
                        {
                            filterContext.Result = RedirectToAction("Index", new { error = "higher" });
                        }
                        if (u.IsInRole("administrator", cache, db))
                        {
                            filterContext.Result = RedirectToAction("Index", new { error = "higher" });
                        }
                    }
                }
            }
        }

        // POST: mitarbeit/user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserSerachViewModel usvm)
        {
            int uid;
            if (int.TryParse(usvm.UserToFind, out uid))
            {
                return RedirectToAction("Notes", new { id = uid });
            }
            else
            {
                User u = userManager.FindByNameAsync(usvm.UserToFind).Result;
                if (u != null)
                {
                    return RedirectToAction("Notes", new { id = u.Id });
                }
                ViewBag.Error = "Der angegebene Benutzer konnte leider nicht gefunden werden. Vielleicht hast du dich vertippt?";
            }

            return View("~/Areas/mitarbeit/Views/User/Index.cshtml", usvm);
        }

        // GET: mitarbeit/user/{id}
        public ActionResult Notes(int id)
        {
            User u = userManager.FindByIdAsync(id).Result;

            if (u == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            int cuid = userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            UserNoteViewModel unvm = new UserNoteViewModel()
            {
                User = u,
                Notes = (from s in db.UserNotes
                         join ur in db.UserRoles on s.RoleIdNeeded equals ur.PermissionId
                         where s.UserId == id && ur.UserId == cuid
                         select s).OrderByDescending(g => g.Created).Include(g => g.Author).ToList(),
                Actions = cache.Get<Dictionary<int, string>>("UserNoteActions")
            };

            return View("~/Areas/mitarbeit/Views/User/Notes.cshtml", unvm);
        }

        [HttpPost]
        public ActionResult MoreNotes(int id, int skip)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return Json(null);
            }

            var notes = db.UserNotes.Include(s => s.Author)
                                    .Where(x => x.UserId == id).OrderByDescending(x => x.Created)
                                    .Skip(skip).Take(15).ToList();
            return Json(notes);
        }

        // GET: user/{id}/bans
        public ActionResult Bans(int id)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            UserBanViewModel ubvm = new UserBanViewModel()
            {
                User = idproof,
                GlobalBan = db.UserBanlist.FirstOrDefault(g => g.UserId == idproof.Id),
                FanartBan = db.FanartBanlist.FirstOrDefault(g => g.UserId == idproof.Id),
                WifiBan = db.WifiBanlist.FirstOrDefault(g => g.UserId == idproof.Id)
            };

            return View("~/Areas/mitarbeit/Views/User/Bans.cshtml", ubvm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles="global-moderator")]
        public ActionResult UpdateBan(int id, string reason, string bvs, DateTime? expireTime)
        {
            // check if the user id is valid
            if (!db.Users.Any(x => x.Id == id))
                return HttpNotFound();

            // get the current users id
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            // ... and the accompanying users
            User banner = db.Users.Find(uid);
            User bannee = db.Users.Find(id);

            // superadmins cannot get banned.
            if(bannee.IsInRole("super-administrator", cache, db))
            {
                // haha, nice try.
                Response.StatusCode = 403;
                Response.Status = "Forbidden";
                return Json(null);
            }

            // for all other stuff:
            // superadmins can ban anyone.
            // gm's cannot ban each other.
            if (bannee.IsInRole("global-moderator", cache, db) &&
               banner.IsInRole("global-moderator", cache, db) &&
              !banner.IsInRole("super-administrator", cache, db))
            {
                Response.StatusCode = 403;
                Response.Status = "Forbidden";
                return Json(null);
            }

            // is this user already banned? if so, unban him.
            if(db.UserBanlist.Any(x => x.UserId == id))
            {
                UserBan b = db.UserBanlist.First(x => x.UserId == id);
                db.UserBanlist.Remove(b);
                db.SaveChanges();

                return Json(new
                {
                    Message = bannee.UserName + " ist nicht gesperrt.",
                    Button = "Sperren"
                });
            }

            UserBan ban = new UserBan();
            ban.IsBanned = true;
            ban.ExpiresOn = expireTime;
            ban.BanReason = reason;
            ban.UserId = bannee.Id;

            db.UserBanlist.Add(ban);
            db.SaveChanges();

            ban.User = bannee;

            string message = bannee.UserName + " ist ";
            if(expireTime != null)
            {
                message += "bis zum " + expireTime.Value.ToString("dd.MM.yyyy HH:mm");
            }
            else
            {
                message += "dauerhaft";
            }

            message += " gesperrt.";
            Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");

            UserNote n = new UserNote()
            {
                AuthorId = uid,
                ActionId = actions["lock-account"],
                Created = DateTime.Now,
                IsDeletable = false,
                RoleIdNeeded = db.Roles.First(x => x.Name == "moderator").Id,
                UserId = id,
                Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/Ban.cshtml", new UserBanNoteViewModel()
                {
                    Message = bvs,
                    Ban = ban
                }),
                UnparsedContent = ""
            };
            db.UserNotes.Add(n);
            db.SaveChanges();

            return Json(new
            {
                Message = message,
                Button = "Entsperren"
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "wifi-moderator")]
        public ActionResult WifiBan(int id, bool add, bool interest, DateTime? addExpire, DateTime? interestExpire)
        {
            WifiBanlist ban = db.WifiBanlist.FirstOrDefault(x => x.UserId == id);
            bool dbadd = false;

            if (ban == null)
            {
                ban = new WifiBanlist();
                dbadd = true;
            }

            ban.CanAddOffers = add;
            ban.CanInterest = interest;

            if(addExpire != null)
                ban.ExpireAddOffers = addExpire;

            if (interestExpire != null)
                ban.ExpireInterest = interestExpire;

            if (dbadd)
                db.WifiBanlist.Add(ban);
            else
                db.SetModified(ban);

            db.SaveChanges();

            return Json(null);
        }

        // POST: api/user/bans/fanart
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ModifyFanartBan(int id, FanartBanlist b)
        {
            if (!User.IsInRole("fanart-moderator"))
            {
                Response.StatusCode = 403;
                return Json(false);
            }

            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 400;
                Response.TrySkipIisCustomErrors = true;
                return Json(false);
            }

            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            FanartBanlist fb = db.FanartBanlist.FirstOrDefault(g => g.UserId == id);

            if(fb == null)
            {
                fb = new FanartBanlist()
                {
                    UserId = id,
                    CanDelete = b.CanDelete,
                    CanUpload = b.CanUpload,
                    CanEdit = b.CanEdit,
                    CanRate = b.CanRate
                };

                db.FanartBanlist.Add(fb);
            }
            else
            {
                fb.CanDelete = b.CanDelete;
                fb.CanEdit = fb.CanEdit;
                fb.CanRate = fb.CanRate;
                fb.CanUpload = fb.CanUpload;

                db.SetModified(fb);
            }

            fb.User = idproof;

            Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");

            UserNote n = new UserNote()
            {
                AuthorId = uid,
                ActionId = actions["lock-account"],
                Created = DateTime.Now,
                IsDeletable = false,
                RoleIdNeeded = db.Roles.First(x => x.Name == "moderator").Id,
                UserId = id,
                Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/FanartBan.cshtml", fb),
                UnparsedContent = ""
            };

            db.SaveChanges();
            
            return Json(true);
        }

        // GET: mitarbeit/user/{id}/neue-notiz
        public ActionResult AddNote(int id)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            Dictionary<int, string> actions = cache.Get<Dictionary<int, string>>("UserUserNoteActions");
            Dictionary<int, string> bvsroles = cache.Get<Dictionary<int, string>>("BvsRoles");

            UserAddNoteViewModel uanvm = new UserAddNoteViewModel()
            {
                User = idproof,
                NoteActions = new List<SelectListItem>(),
                BvsRoles = new List<SelectListItem>()
            };

            foreach (var action in actions)
            {
                uanvm.NoteActions.Add(new SelectListItem() { Text = action.Value, Value = action.Key.ToString() });
            }

            foreach (var role in bvsroles)
            {
                uanvm.BvsRoles.Add(new SelectListItem() { Text = role.Value, Value = role.Key.ToString() });
            }

            return View("~/Areas/mitarbeit/Views/User/AddNote.cshtml", uanvm);
        }

        // POST: mitarbeit/user/{id}/neue-notiz
        [HttpPost]
        public ActionResult AddNote(int id, UserAddNoteViewModel uanote)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            UserNote note = uanote.NoteToAdd;

            note.AuthorId = userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            note.Created = DateTime.Now;
            note.IsDeletable = true;
            // Until the BB-Code-Parser is ready, let's just tidy up HTML to be on the safe side, I guess.
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Clear(); // Disallow everything.
            note.Content = sanitizer.Sanitize(note.UnparsedContent);
            note.UserId = id;


            if (String.IsNullOrEmpty(note.Content) || note.ActionId < 0 || note.RoleIdNeeded < 0)
            {
                int i = this.ModelState.Count;
                Dictionary<int, string> actions = cache.Get<Dictionary<int, string>>("UserUserNoteActions");
                Dictionary<int, string> bvsroles = cache.Get<Dictionary<int, string>>("BvsRoles");

                UserAddNoteViewModel uanvm = new UserAddNoteViewModel()
                {
                    User = idproof,
                    NoteActions = new List<SelectListItem>(),
                    BvsRoles = new List<SelectListItem>()
                };

                foreach (var action in actions)
                {
                    uanvm.NoteActions.Add(new SelectListItem() { Text = action.Value, Value = action.Key.ToString() });
                }

                foreach (var role in bvsroles)
                {
                    uanvm.BvsRoles.Add(new SelectListItem() { Text = role.Value, Value = role.Key.ToString() });
                }

                return View("~/Areas/mitarbeit/Views/User/AddNote.cshtml", uanvm);
            }

            db.UserNotes.Add(note);
            db.SaveChanges();

            return RedirectToAction("Notes", new { id = id });
        }

        // GET: mitarbeit/user/{id}/werbung
        public ActionResult AddAdvertising(int id)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            ViewBag.AdvertisingForms = cache.Get<List<UserAdvertisingForm>>("AdvertisingFormsFull");
            ViewBag.AdvertisingFormIds = cache.Get<Dictionary<int, string>>("AdvertisingForms");

            UserAdvertisingViewModel uavm = new UserAdvertisingViewModel()
            {
                RecordedAdvertisings = db.UserAdvertising.Include(x => x.AdvertisingFrom).Where(x => x.AdvertisingFromId == id).OrderByDescending(x => x.ReportTime).ToList(),
                User = idproof
            };

            return View("~/Areas/mitarbeit/Views/User/Advertising.cshtml", uavm);
        }

        // POST: mitarbeit/user/{id}/werbung
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddAdvertising(int id, UserAdvertisingViewModel advertisingvm)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            UserAddAdvertisingViewModel advertising = advertisingvm.AdvertisingToAdd;

            UserAdvertisingViewModel uavm;


            var forms = cache.Get<List<UserAdvertisingForm>>("AdvertisingFormsFull");
            ViewBag.AdvertisingForms = forms;
            ViewBag.AdvertisingFormIds = cache.Get<Dictionary<int, string>>("AdvertisingForms");

            int? adtoid = null;
            bool isTargeted = forms.First(x => x.Id == advertising.AdvertisingFormId).IsTargeted;

            if (advertising.UserName != null && isTargeted) // if we don't have a username to work with, error out.
            {
                User u = userManager.FindByNameAsync(advertising.UserName).Result;

                // If we did not find a username and the advertising form is targeted, error out.
                if (u == null && isTargeted)
                {
                    return AddAdvertisingError(idproof, advertising);
                }
                else if (u != null && isTargeted) // If we did find a user and it is a targeted advertising form, set the target user.
                {
                    adtoid = u.Id;
                }
            }
            else if (advertising.UserName == null && isTargeted)
            {
                return AddAdvertisingError(idproof, advertising);
            }

            if(advertising.UserName != null) 
            {
                advertising.UserUrl = db.Users.Where(x => x.UserName == advertising.UserName).Select(x => x.Url).FirstOrDefault();
                advertising.UserNameExistsInDb = advertising.UserUrl != null;

            }

            int currentUserId = userManager.FindByNameAsync(User.Identity.Name).Result.Id;

            UserAdvertising ad = new UserAdvertising()
            {
                AdvertisingFormId = advertising.AdvertisingFormId,
                ReportTime = DateTime.Now,
                ScreenshotUrl = advertising.ScreenshotUrl ?? "",
                Url = advertising.Url,
                AdvertisingFromId = id,
                AdvertisingToId = adtoid,
                AuthorId = currentUserId
            };

            Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");
            Dictionary<int, string> bvsroles = cache.Get<Dictionary<int, string>>("BvsRoles");

            UserAdvertisingNoteViewModel uanvm = new UserAdvertisingNoteViewModel()
            {
                Uaavm = advertising,
                AdvertisingForm = forms.Where(g => g.Id == advertising.AdvertisingFormId).First().Name
            };

            UserNote n = new UserNote()
            {
                AuthorId = currentUserId,
                ActionId = actions["advertising"],
                Created = DateTime.Now,
                IsDeletable = false,
                RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                UserId = id,
                Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/Advertising.cshtml", uanvm),
                UnparsedContent = ""
            };

            db.UserAdvertising.Add(ad);
            db.UserNotes.Add(n);
            db.SaveChanges();



            uavm = new UserAdvertisingViewModel()
            {
                RecordedAdvertisings = db.UserAdvertising.Include(x => x.AdvertisingFrom).Where(x => x.AdvertisingFromId == id).OrderByDescending(x => x.ReportTime).ToList(),
                User = idproof
            };

            return View("~/Areas/mitarbeit/Views/User/Advertising.cshtml", uavm);
        }

        [NonAction]
        private ActionResult AddAdvertisingError(User idproof, UserAddAdvertisingViewModel advertising)
        {
            UserAdvertisingViewModel uavm = new UserAdvertisingViewModel()
            {
                RecordedAdvertisings = db.UserAdvertising.Include(x => x.AdvertisingFrom).Where(x => x.AdvertisingFromId == idproof.Id).OrderByDescending(x => x.ReportTime).ToList(),
                User = idproof
            };
            uavm.AdvertisingToAdd = advertising;
            ViewBag.Error = true;
            return View("~/Areas/mitarbeit/Views/User/Advertising.cshtml", uavm);
        }

        // GET: /user/{id}/accounts
        public ActionResult Multiaccount(int id)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            var accounts = db.UserMultiaccounts.Where(g => g.UserFromId == id || g.UserToId == id)
                                                .Include(g => g.UserTo)
                                                .Include(g => g.UserFrom)
                                                .Include(g => g.Moderator);

            UserMultiaccountViewModel umvm = new UserMultiaccountViewModel()
            {
                Accounts = accounts.ToList(),
                User = idproof
            };

            return View("~/Areas/mitarbeit/Views/User/Multiaccounts.cshtml", umvm);
        }
        
        // POST: mitarbeit/user/{id}/accounts
        [HttpPost]
        public ActionResult Multiaccount(int id, UserMultiaccountViewModel umvm)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            UserMultiaccountAddViewModel umavm = umvm.MultiaccountToAdd;

            User u2 = userManager.FindByNameAsync(umavm.UserName).Result;

            if (!db.UserMultiaccounts.Any(g => (g.UserFromId == id && g.UserToId == u2.Id) || (g.UserFromId == u2.Id && g.UserToId == id)))
            {
                // if the multiaccount isn't known yet, add it.
                UserMultiaccount um = new UserMultiaccount()
                {
                    UserFromId = id,
                    UserToId = u2.Id,
                    ModeratorUserId = userManager.FindByNameAsync(User.Identity.Name).Result.Id,
                    Note = umavm.Reason,
                    Status = MultiaccountStatus.Unspecified,
                    Time = DateTime.Now
                };

                db.UserMultiaccounts.Add(um);

                Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");
                Dictionary<int, string> bvsroles = cache.Get<Dictionary<int, string>>("BvsRoles");
                UserNote n = new UserNote()
                {
                    AuthorId = userManager.FindByNameAsync(User.Identity.Name).Id,
                    ActionId = actions["multiaccount"],
                    Created = DateTime.Now,
                    IsDeletable = false,
                    RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                    UserId = um.UserFromId,
                    Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/Multiaccount.cshtml", u2.UserName),
                    UnparsedContent = ""
                };
                UserNote n2 = new UserNote()
                {
                    AuthorId = userManager.FindByNameAsync(User.Identity.Name).Id,
                    ActionId = actions["multiaccount"],
                    Created = DateTime.Now,
                    IsDeletable = false,
                    RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                    UserId = um.UserToId,
                    Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/Multiaccount.cshtml", idproof.UserName),
                    UnparsedContent = ""
                };
                db.UserNotes.Add(n);
                db.UserNotes.Add(n2);

                db.SaveChanges();
            }
            var accounts = db.UserMultiaccounts.Where(g => g.UserFromId == id || g.UserToId == id)
                                                .Include(g => g.UserTo)
                                                .Include(g => g.UserFrom)
                                                .Include(g => g.Moderator);
            umvm = new UserMultiaccountViewModel()
            {
                Accounts = accounts.ToList(),
                User = idproof
            };

            return View("~/Areas/mitarbeit/Views/User/Multiaccounts.cshtml", umvm);
        }

        // POST: api/multiaccount/{id}/set-exception
        [HttpPost]
        [Authorize(Roles = "bereichsleiter")]
        public ActionResult SetMultiaccountException(int id)
        {
            UserMultiaccount mua = db.UserMultiaccounts.Include(g => g.UserFrom).Include(g => g.UserTo).Where(x => x.Id == id).FirstOrDefault();

            if (mua == null)
                return Json(false);

            mua.Status = MultiaccountStatus.AllowedMultiaccount;

            db.SetModified(mua);

            Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");
            Dictionary<int, string> bvsroles = cache.Get<Dictionary<int, string>>("BvsRoles");
            UserNote n = new UserNote()
            {
                AuthorId = userManager.FindByNameAsync(User.Identity.Name).Id,
                ActionId = actions["multiaccount"],
                Created = DateTime.Now,
                IsDeletable = false,
                RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                UserId = mua.UserFromId,
                Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/MultiaccountException.cshtml", mua.UserTo.UserName),
                UnparsedContent = ""
            };
            UserNote n2 = new UserNote()
            {
                AuthorId = userManager.FindByNameAsync(User.Identity.Name).Id,
                ActionId = actions["multiaccount"],
                Created = DateTime.Now,
                IsDeletable = false,
                RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                UserId = mua.UserToId,
                Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/MultiaccountException.cshtml", mua.UserFrom.UserName),
                UnparsedContent = ""
            };
            db.UserNotes.Add(n);
            db.UserNotes.Add(n2);

            db.SaveChanges();

            return Json(true);
        }

        // POST: api/multiaccount/{id}/set-no-multiaccount
        [HttpPost]
        [Authorize(Roles = "bereichsleiter")]
        public ActionResult SetNoMultiaccount(int id)
        {
            UserMultiaccount mua = db.UserMultiaccounts.Include(g => g.UserFrom).Include(g => g.UserTo).Where(x => x.Id == id).FirstOrDefault();

            if (mua == null)
                return Json(false);

            mua.Status = MultiaccountStatus.FalsePositive;

            db.SetModified(mua);
            
            Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");
            Dictionary<int, string> bvsroles = cache.Get<Dictionary<int, string>>("BvsRoles");
            UserNote n = new UserNote()
            {
                AuthorId = userManager.FindByNameAsync(User.Identity.Name).Id,
                ActionId = actions["multiaccount"],
                Created = DateTime.Now,
                IsDeletable = false,
                RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                UserId = mua.UserFromId,
                Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/NoMultiaccount.cshtml", mua.UserTo.UserName),
                UnparsedContent = ""
            };
            UserNote n2 = new UserNote()
            {
                AuthorId = userManager.FindByNameAsync(User.Identity.Name).Id,
                ActionId = actions["multiaccount"],
                Created = DateTime.Now,
                IsDeletable = false,
                RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                UserId = mua.UserToId,
                Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/NoMultiaccount.cshtml", mua.UserFrom.UserName),
                UnparsedContent = ""
            };
            db.UserNotes.Add(n);
            db.UserNotes.Add(n2);

            db.SaveChanges();

            return Json(true);
        }

        // GET: /user/{id}/rechte
        [Authorize(Roles = "role-manager, superadmin")]
        public ActionResult Roles(int id)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            if (idproof == null)
            {
                Response.StatusCode = 404;
                Response.TrySkipIisCustomErrors = true;
                return View("~/Areas/mitarbeit/Views/User/_NotFound.cshtml");
            }

            List<Role> rolelist;
            User currentUser = userManager.FindByNameAsync(User.Identity.Name).Result;

            if(currentUser.IsInRole("superadmin", cache, db))
            {
                rolelist = db.Roles.ToList();
            }
            else
            {

                rolelist = (from s in db.Roles
                            join e in db.UserRoles on s.MetapermissionId equals e.PermissionId
                            where s.MetapermissionId != null && e.UserId == currentUser.Id
                            select s).ToList();
                
            }

            UserRolesViewModel urvm = new UserRolesViewModel()
            {
                Roles = rolelist,
                UserRoles = db.UserRoles.Where(x => x.UserId == id).ToList(),
                RoleChain = db.RoleChain.ToList(),
                User = idproof
            };

            return View("~/Areas/mitarbeit/Views/User/Roles.cshtml", urvm);
        }

        /// <summary>
        /// POST: api/rolemanager/{id}/set-role
        /// </summary>
        /// <param name="id">The UserID (extracted from URI).</param>
        /// <param name="role">The role id.</param>
        /// <param name="status">if set to <c>true</c>, user will be put in role. If false, user will be removed from role.</param>
        /// <remarks>Adding permissions respects the permission chain, removing a top role however doesn't remove the roles down the chain.</remarks>
        /// <returns>JSON</returns>
        [HttpPost]
        [Authorize(Roles = "role-manager, superadmin")]
        public ActionResult SetRole(int id, int role, bool status)
        {
            User idproof = userManager.FindByIdAsync(id).Result;

            User currentUser = userManager.FindByNameAsync(User.Identity.Name).Result;

            if (!currentUser.IsInRole("superadmin", cache, db))
            {
                if (currentUser.Id == id)
                {
                    Response.StatusCode = 401;
                    Response.TrySkipIisCustomErrors = true;
                    return Json(null);
                }

                Role r = db.Roles.Find(role);
                if(r.MetapermissionId == null || 
                    !db.UserRoles.Any(g => g.PermissionId == r.MetapermissionId && g.UserId == userManager.FindByNameAsync(User.Identity.Name).Result.Id))
                {
                    Response.StatusCode = 401;
                    Response.TrySkipIisCustomErrors = true;
                    return Json(-2);
                }
            }

            if (idproof == null)
            {
                return Json(0);
            }

            if (status == false)
            {
                if (isInParent(id, role))
                {
                    return Json(-1);
                }
                var uRole = db.UserRoles.Where(g => g.UserId == id && g.PermissionId == role).FirstOrDefault();
                if (uRole != null)
                {
                    db.UserRoles.Remove(uRole);
                    UserRoleAddNoteViewModel uranvm = new UserRoleAddNoteViewModel()
                    {
                        User = userManager.FindByNameAsync(User.Identity.Name).Result,
                        Role = db.Roles.Find(role)
                    };

                    Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");
                    Dictionary<int, string> bvsroles = cache.Get<Dictionary<int, string>>("BvsRoles");
                    UserNote n = new UserNote()
                    {
                        AuthorId = uranvm.User.Id,
                        ActionId = actions["roles"],
                        Created = DateTime.Now,
                        IsDeletable = false,
                        RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                        UserId = id,
                        Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/RoleRemoved.cshtml", uranvm),
                        UnparsedContent = ""
                    };
                    db.UserNotes.Add(n);
                    db.SaveChanges();
                }
            }
            else
            {
                // set true is recursive, set false isn't.
                givePermission(id, role);
            }

            return Json(1);
        }

        /// <summary>
        /// Gives the permission.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="role">The role.</param>
        [NonAction]
        private void givePermission(int id, int role)
        {
            var roles = db.RoleChain.Where(g => g.ParentRoleId == role);


            // avoid setting it multiple times
            if (!db.UserRoles.Any(x => x.UserId == id && x.PermissionId == role))
            {
                db.UserRoles.Add(new UserRole() { UserId = id, PermissionId = role });

                UserRoleAddNoteViewModel uranvm = new UserRoleAddNoteViewModel()
                {
                    User = userManager.FindByNameAsync(User.Identity.Name).Result,
                    Role = db.Roles.Find(role)
                };

                Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");
                Dictionary<int, string> bvsroles = cache.Get<Dictionary<int, string>>("BvsRoles");
                UserNote n = new UserNote()
                {
                    AuthorId = uranvm.User.Id,
                    ActionId = actions["roles"],
                    Created = DateTime.Now,
                    IsDeletable = false,
                    RoleIdNeeded = bvsroles.First(g => g.Value == "Bereichsassistent").Key,
                    UserId = id,
                    Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/Role.cshtml", uranvm),
                    UnparsedContent = ""
                };
                db.UserNotes.Add(n);

                db.SaveChanges();
            }

            var rolesList = roles.ToList();
            foreach (var r in rolesList)
            {
                givePermission(id, r.ChildRoleId);
            }
        }

        /// <summary>
        /// Determines whether the User with the id <c>id</c> is in the role with the id <c>role</c>.
        /// </summary>
        /// <param name="id">The userid.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [NonAction]
        private bool isInParent(int id, int role)
        {
            var roles = db.RoleChain.Where(g => g.ChildRoleId == role).ToList();

            foreach (var r in roles)
            {
                if (db.UserRoles.Any(x => x.UserId == id && x.PermissionId == r.ParentRoleId) || isInParent(id, r.ParentRoleId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}