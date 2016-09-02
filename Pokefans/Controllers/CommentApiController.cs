// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Caching;
using Pokefans.Data;
using Pokefans.Data.Comments;
using Pokefans.Models;
using Pokefans.Security;
using Pokefans.Util;
using Pokefans.Util.Comments;
using Pokefans.Util.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Pokefans.Controllers
{
    public class CommentApiController : Controller
    {
        private Entities db;
        private Cache cache;
        private ApplicationUserManager userManager;

        public CommentApiController(Entities ents, Cache c, ApplicationUserManager umgr)
        {
            db = ents;
            cache = c;
            userManager = umgr;
        }

        [HttpPost]
        [Authorize]
        [Throttle(Name = "NewCommentThrottle", Seconds = 5)]
        [AllowCors]
        public ActionResult Add(NewCommentViewModel newComment)
        {
            User currentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            ParserConfiguration config = ParserConfiguration.Default;
            config.EnableInsideCodes = false;
            config.EscapeHtml = true;
            config.NewlineToHtml = true;
            Parser parser = new Parser(config);

            // you *cannot* have comment with id 0 as parent.
            if (newComment.ParentId.HasValue && newComment.ParentId == 0) newComment.ParentId = null;

            Comment c = new Comment()
            {
                AuthorId = currentUser.Id,
                Level = 0,
                DisplayPublic = true,
                CommentedObjectId = newComment.CommentedObjectId,
                Context = newComment.Context,
                ParentCommentId = newComment.ParentId ?? null,
                ParsedComment = parser.Parse(newComment.Text),
                SubmitTime = DateTime.Now,
                UnparsedComment = newComment.Text
            };

            CommentManager manager = getManager((CommentContext)newComment.Context);

            manager.AddComment(c);

            return Json(new CommentViewModel(c));
        }

        [Authorize]
        [Throttle(Name = "CommentDeleteThrottle", Seconds = 5)]
        [AllowCors]
        public ActionResult Delete(int commentId)
        {
            Comment comment = CommentManager.LoadCommentWithChildrenById(commentId, db);

            CommentManager manager = getManager((CommentContext)comment.Context);
            User currentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            if (manager.CanDelete(currentUser, comment))
            {
                manager.DeleteComment(comment);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "comment-moderator")]
        [AllowCors]
        public ActionResult Hide(int commentId)
        {
            Comment comment = db.Comments.Find(commentId);
            User currentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            comment.DisplayPublic = !comment.DisplayPublic;
            db.SetModified(comment);
            db.SaveChanges();

            CommentViewModel cvm = new CommentViewModel(comment);

            return Json(new { commentText = cvm.Text, commentDisplay = comment.DisplayPublic }, JsonRequestBehavior.AllowGet);
        }

        [AllowCors]
        public ActionResult GetCommentsFor(int ctx, int commentedObjectId)
        {
            CommentManager manager = getManager((CommentContext)ctx);

            List<Comment> comments = manager.LoadAll(commentedObjectId);

            return Json(commentsToViewModel(comments, manager), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<CommentViewModel> commentsToViewModel(List<Comment> comments, CommentManager manager)
        {
            List<CommentViewModel> cvm = new List<CommentViewModel>();

            User currentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            foreach (var comment in comments)
            {
                CommentViewModel c = new CommentViewModel(comment);
                if (currentUser != null && manager.CanDelete(currentUser, comment))
                    c.IsDeletable = true;
                c.Children = commentsToViewModel(comment.Children, manager);
            }

            return cvm;
        }

        private CommentManager getManager(CommentContext context)
        {

            CommentManager manager;
            switch (context)
            {
                case CommentContext.Content:
                    return manager = new ContentCommentManager(db, cache, HttpContext);
                case CommentContext.Fanart:
                    return manager = new FanartCommentManager(db, cache, HttpContext);
                case CommentContext.Movesets:
                    return manager = new FanartCommentManager(db, cache, HttpContext);
                default:
                    throw new Exception("CommentManager not found");
            }

        }
    }
}