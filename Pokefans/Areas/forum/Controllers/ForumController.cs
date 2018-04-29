// copyright 2018 the pokefans authors. see copying.md for legal info.
using System;
using System.Web.Mvc;
using Pokefans.Data;
using System.Linq;
using Pokefans.Security;
using Microsoft.AspNet.Identity;
using Pokefans.Caching;
using Pokefans.Areas.forum.Models;
using Pokefans.Data.Forum;
using Pokefans.Models;
using Pokefans.Util;
using Pokefans.Util.Parser;

namespace Pokefans.Areas.forum.Controllers
{
    public class ForumController : Controller
    {
        readonly Entities db;
        Cache cache;
        const int ThreadsPerPage = 25;

        public ForumController(Entities ents, Cache c) 
        {
            db = ents;
            cache = c;

        }

        public ActionResult ViewForum(string url, int start = 0) 
        {
            int boardid;

            try
            {
                boardid = db.Boards.Where(x => x.Url == url.ToLower()).Select(x => x.Id).First();
            }
            catch(Exception) { // TODO: don't catch all but only not found
                return new HttpNotFoundResult();
            }
            User currentUser = null;
            if (User != null && User.Identity.IsAuthenticated)
            {
                int currentUserId = User.Identity.GetUserId<int>();
                currentUser = db.Users.FirstOrDefault(g => g.Id == currentUserId);
            }
            var security = new ForumSecurity(db, cache, currentUser);

            if(!security.CanReadBoard(boardid)) 
            {
                HttpContext.Response.StatusCode = 403;
                return View("~/Areas/forum/Views/Shared/Unauthorized.cshtml");
            }

            // okay, so we can read this board. We now have to load threads in
            // this board and the viewing users permissions. It's okay to reduce
            // this to bools here, the complicated overriding methodology is
            // already done at this point.

            ViewForumViewModel vfvm = new ViewForumViewModel(security, boardid);

            vfvm.Announcements = (from s in db.Thread.Include("Author")
                                 where (s.BoardId == boardid && s.Type == ThreadType.Announcement)
                                        || s.Type == ThreadType.GlobalAnnouncement
                                 join p in db.Post.Include("Author") on s.LastPostId equals p.Id
                                  orderby p.PostTime descending
                                  select new ViewForumThreadViewModel() { LastPost = p, Thread = s }).ToList();

            if (start == 0) 
            {
                // we are on the first page, get the stickies, do not count them 
                // to the maximum threads per page.
                vfvm.Sticky = (from s in db.Thread.Include("Author")
                               where (s.BoardId == boardid && s.Type == ThreadType.Sticky)
                               join p in db.Post.Include("Author") on s.LastPostId equals p.Id
                               orderby p.PostTime descending
                               select new ViewForumThreadViewModel() { LastPost = p, Thread = s }).ToList();
            }

            // now, get the threads themself
            vfvm.Threads = (from s in db.Thread.Include("Author")
                            where (s.BoardId == boardid && s.Type == ThreadType.Announcement)
                                   || s.Type == ThreadType.GlobalAnnouncement
                            join p in db.Post.Include("Author") on s.LastPostId equals p.Id
                            orderby p.PostTime descending
                            select new ViewForumThreadViewModel() { LastPost = p, Thread = s }).Skip(start).Take(ThreadsPerPage).ToList();

            int totalThreads = db.Thread.Where(x => x.Type == ThreadType.Normal).Count();
            
            vfvm.Board = db.Boards.Find(boardid);
            UrlHelper helper = new UrlHelper();
            vfvm.Pagination = new PaginationViewModel(1, (totalThreads / ThreadsPerPage) + 1, (start / ThreadsPerPage) + 1, helper.Map(url + "/" + "page{0}.html"), ThreadsPerPage);

            return View("~/Areas/forum/Views/Forum/Forum.cshtml", vfvm);
        }

        public ActionResult NewThread(string url) {
            int boardid;

            try
            {
                boardid = db.Boards.Where(x => x.Url == url.ToLower()).Select(x => x.Id).First();
            }
            catch (Exception)
            { // TODO: don't catch all but only not found
                return new HttpNotFoundResult();
            }
            User currentUser = null;
            if (User != null && User.Identity.IsAuthenticated)
            {
                int currentUserId = User.Identity.GetUserId<int>();
                currentUser = db.Users.FirstOrDefault(g => g.Id == currentUserId);
            }
            var security = new ForumSecurity(db, cache, currentUser);

            if (!security.CanWriteBoard(boardid))
            {
                HttpContext.Response.StatusCode = 403;
                return View("~/Areas/forum/Views/Shared/Unauthorized.cshtml");
            }

            NewThreadViewModel ntvm = new NewThreadViewModel();
            ntvm.Board = db.Boards.Find(boardid);

            return View("~/Areas/forum/Views/Forum/NewThread.cshtml", ntvm); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewThread(string url, NewThreadViewModel ntvm) {
            int boardid;

            try
            {
                boardid = db.Boards.Where(x => x.Url == url.ToLower()).Select(x => x.Id).First();
            }
            catch (Exception)
            { // TODO: don't catch all but only not found
                return new HttpNotFoundResult();
            }
            User currentUser = null;
            if (User != null && User.Identity.IsAuthenticated)
            {
                int currentUserId = User.Identity.GetUserId<int>();
                currentUser = db.Users.FirstOrDefault(g => g.Id == currentUserId);
            }
            var security = new ForumSecurity(db, cache, currentUser);

            if (!security.CanWriteBoard(boardid))
            {
                HttpContext.Response.StatusCode = 403;
                return View("~/Areas/forum/Views/Shared/Unauthorized.cshtml");
            }


            if(ntvm.Type > ThreadType.Normal) {
                if(!security.CanModerateBoard(boardid) && ntvm.Type == ThreadType.GlobalAnnouncement) {
                    ntvm.Type = ThreadType.Normal;
                }
                if(!User.IsInRole("global-moderator") && ntvm.Type == ThreadType.GlobalAnnouncement) {
                    ntvm.Type = ThreadType.Normal;
                }
            }

            Thread t = new Thread()
            {
                Title = ntvm.Subject,
                Type = ntvm.Type,
                ThreadIcon = 0, //TODO: Icon Support
                BoardId = boardid,
                ThreadStartTime = DateTime.Now,
                AuthorId = currentUser?.Id,
                IsLocked = false,
                Replies = 0,
                Visits = 0
            };

            Parser parser = new Parser();

            Post p = new Post()
            {
                Subject = ntvm.Subject,
                PostTime = DateTime.Now,
                LastEditTime = null,
                AuthorId = currentUser?.Id,
                NeedsApproval = (currentUser == null),
                BodyRaw = ntvm.Body,
                Body = parser.Parse(ntvm.Body),
                ReactionHeart = 0,
                ReactionLol = 0,
                ReactionThumbsUp = 0,
                IpAddress = SecurityUtils.GetIPAddressAsString(HttpContext),
                IsSolution = false,
            };

            db.Thread.Add(t);

            db.SaveChanges();

            p.ThreadId = t.Id;

            db.Post.Add(p);
            db.SaveChanges();

            t.LastPostId = p.Id;

            Board board = db.Boards.Find(boardid);

            return RedirectToAction("ViewThread", "Thread", new { area = "forum", id = t.Id, url = board.Url });
        }
    }
}
