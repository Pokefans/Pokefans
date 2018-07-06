using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Microsoft.AspNet.Identity;
using Pokefans.Data;
using Pokefans.Data.UserData;
using Pokefans.Data.Forum;
using Pokefans.Areas.user.Models;
using Pokefans.Util.Parser;
using Pokefans.Security;

namespace Pokefans.Areas.user.Controllers
{
    [Authorize]
    public class PrivateMessageController : Controller
    {
        private Entities db;

        private static int PERPAGE = 25;

        public PrivateMessageController(Entities ents) 
        {
            db = ents;
        }


        public ActionResult Index(int page = 0, int? label = null)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var vm = new PrivateMessageFolderViewModel<PrivateMessageInbox>();

            vm.Labels = db.PrivateMessageLabels.Where(x => x.OwnerUserId == uid).ToDictionary(x => x.Id);

            vm.HasMore = db.PrivateMessagesInbox.Count(x => x.UserToId == uid) > PERPAGE * (page + 1);

            var messages = db.PrivateMessagesInbox.Include(x => x.Message).Include(x => x.Labels).Include(x => x.From).Where(x => x.UserToId == uid);

            if(label != null)
            {
                messages = messages.Where(x => x.Labels.Any(y => y.PrivateMessageLabelId == label));
            }

            vm.Messages = messages.OrderByDescending(g => g.Message.Sent).Skip(page * PERPAGE).Take(PERPAGE).ToList();

            return View("~/Areas/user/Views/PrivateMessage/Inbox.cshtml", vm);

        }

        public ActionResult Outbox(int page=0, int? label = null) 
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var vm = new PrivateMessageFolderViewModel<PrivateMessageSent>();

            vm.Labels = db.PrivateMessageLabels.Where(x => x.OwnerUserId == uid).ToDictionary(x => x.Id);



            var messages = db.PrivateMessagesSent.Include(x => x.Message).Where(x => x.UserFromId == uid).Select(x => x.Message.Id);


            var unread = db.PrivateMessagesInbox.Where(x => x.Read == false && messages.Contains(x.PrivateMessageId)).Select(x => x.PrivateMessageId);

            vm.HasMore = unread.Count() > PERPAGE * (page + 1);

            var outbox = db.PrivateMessagesSent.Include(x => x.Message).Include(x => x.Labels).Where(x => unread.Contains(x.PrivateMessageId));

            if (label != null)
            {
                outbox = outbox.Where(x => x.Labels.Any(y => y.PrivateMessageLabelId == label));
            }

            vm.Messages = outbox.OrderByDescending(g => g.Message.Sent).Skip(page * PERPAGE).Take(PERPAGE);

            return View("~/Areas/user/Views/PrivateMessage/Outbox.cshtml", vm);
        }

        public ActionResult Sent(int page = 0, int? label = null)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var vm = new PrivateMessageFolderViewModel<PrivateMessageSent>();

            vm.Labels = db.PrivateMessageLabels.Where(x => x.OwnerUserId == uid).ToDictionary(x => x.Id);

            var messages = db.PrivateMessagesSent.Include(x => x.Message).Where(x => x.UserFromId == uid).Select(x => x.Message.Id);


            var read = db.PrivateMessagesInbox.Where(x => x.Read == true && messages.Contains(x.PrivateMessageId)).Select(x => x.PrivateMessageId);

            vm.HasMore = read.Count() > PERPAGE * (page + 1);

            var outbox = db.PrivateMessagesSent.Include(x => x.Message).Include(x => x.Labels).Where(x => read.Contains(x.PrivateMessageId));

            if (label != null)
            {
                outbox = outbox.Where(x => x.Labels.Any(y => y.PrivateMessageLabelId == label));
            }

            vm.Messages = outbox.OrderByDescending(g => g.Message.Sent).Skip(page * PERPAGE).Take(PERPAGE);

            return View("~/Areas/user/Views/PrivateMessage/Sent.cshtml", vm);
        }


        public ActionResult View(int id) 
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (!db.PrivateMessagesInbox.Any(x => (x.UserToId == uid) && x.PrivateMessageId == id) && 
                !db.PrivateMessagesSent.Any(x => x.UserFromId == uid && x.PrivateMessageId == id)) {
                // if we land here, the user did neither recieve nor send the message, so it does not exist for him/her.
                // This also catches non-existing messages.
                return HttpNotFound();
            }

            PrivateMessageViewModel vm = new PrivateMessageViewModel();

            vm.Labels = db.PrivateMessageLabels.Where(x => x.OwnerUserId == uid).ToDictionary(x => x.Id);

            vm.Message = db.PrivateMessages.First(x => x.Id == id);

            if(db.PrivateMessageReports.Any(x => x.UserReportId == uid && x.PrivateMessageId == id))
                vm.ReportTime = db.PrivateMessageReports.Where(x => x.UserReportId == uid && x.PrivateMessageId == id).Select(x => x.Timestamp).First();

            if(db.PrivateMessagesInbox.Any(x => x.PrivateMessageId == id && x.UserToId == uid)) 
            {
                vm.From = (from x in db.PrivateMessagesInbox
                           from y in db.Users
                           where x.PrivateMessageId == id && x.UserToId == uid && y.Id == x.UserFromId
                           select y).First();

				var inbox = db.PrivateMessagesInbox.Where(x => x.PrivateMessageId == id && x.UserToId == uid).First();

				if(!inbox.Read) {
					inbox.Read = true;
					db.SetModified(inbox);
					db.SaveChanges();
				}

				vm.IsInbox = true;
				vm.DeleteKey = inbox.Id;

                vm.MessageLabels = (from x in db.PrivateMessagesInbox
                                    from y in db.PrivateMessagesInboxLabels
                                    from z in db.PrivateMessageLabels
                                    where x.PrivateMessageId == id &&
                                          x.UserToId == uid &&
                                          y.PrivateMessageInboxId == x.Id &&
                                          z.Id == y.PrivateMessageLabelId
                                    select z).ToList();
            }
            else 
            {
                vm.From = (from x in db.PrivateMessagesSent
                           from y in db.Users
                           where x.PrivateMessageId == id && 
                                 x.UserFromId == uid      && 
                                 y.Id == x.UserFromId
                           select y).First();


				vm.IsInbox = false;
				var inbox = db.PrivateMessagesSent.Where(x => x.PrivateMessageId == id && x.UserFromId == uid).First();
				vm.DeleteKey = inbox.Id;

                vm.MessageLabels = (from x in db.PrivateMessagesSent
                                    from y in db.PrivateMessagesSentLabels
                                    from z in db.PrivateMessageLabels
                                    where x.PrivateMessageId == id       &&
                                          x.UserFromId == uid            &&
                                          y.PrivateMessageSentId == x.Id &&
                                          z.Id == y.PrivateMessageLabelId
                                    select z).ToList();
            }

            return View("~/Areas/user/Views/PrivateMessage/View.cshtml", vm);
        }

        public ActionResult Compose(int? to = null, int? fquote = null, int? reply = null) 
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            PrivateMessageComposeViewModel viewModel = new PrivateMessageComposeViewModel();

            if (to.HasValue) {
                int val = to.Value;

                viewModel.To = db.Users.Where(x => x.Id == val).Select(x => x.UserName).FirstOrDefault();
            }
            if (fquote.HasValue) {

                int val = fquote.Value;
                var post = db.Post.Include(x => x.Author).Where(x => x.Id == val).FirstOrDefault();

                if (post != null)
                {

                    viewModel.Body = "[quote=\"\"]" + post.BodyRaw + "[/quote]";
                }
            }

            if (reply.HasValue) 
            {
                int val = reply.Value;

                if(db.PrivateMessagesInbox.Any(x => (x.UserToId == uid || x.UserFromId == uid) && x.PrivateMessageId == val) || 
                   db.PrivateMessagesSent.Any (x => (x.UserFromId == uid) && x.PrivateMessageId == val))
                {
                    viewModel.ReplyTo = val;

                    var from = db.PrivateMessagesInbox.Include(x => x.From).Where(x => x.PrivateMessageId == val).Select(x => x.From).FirstOrDefault();
                    if (from == null)
                        from = db.PrivateMessagesSent.Include(x => x.From).Where(x => x.PrivateMessageId == val).Select(x => x.From).First();

                    viewModel.To = from.UserName;

                    string subject = db.PrivateMessages.Where(x => x.Id == val).Select(x => x.Subject).First();

                    if (!subject.StartsWith("Re: ", StringComparison.CurrentCulture))
                        subject = "Re: " + subject;

                    viewModel.Subject = subject;

					Guid conversationid = db.PrivateMessages.Where(x => x.Id == val).Select(x => x.ConversationId).First();

					var oldmessages = (from x in db.PrivateMessagesInbox
									  from y in db.PrivateMessages
									  from z in db.Users
									  where x.PrivateMessageId == y.Id &&
											z.Id == x.UserFromId &&
                                            y.ConversationId == conversationid &&
                                            x.UserToId == uid
					                   select new OldMessageViewModel() { User = z, Message = y }).ToList();

					var oldmessages2 = (from x in db.PrivateMessagesSent
										from y in db.PrivateMessages
										from z in db.Users
										where x.PrivateMessageId == y.Id &&
											  z.Id == x.UserFromId &&
											  y.ConversationId == conversationid &&
										      x.UserFromId == uid
					                    select new OldMessageViewModel() { User = z, Message = y}).ToList();

					oldmessages.AddRange(oldmessages2);
					//oldmessages = oldmessages.GroupBy(x => x.Message.Id).Select(group => group.First()).ToList();
					oldmessages.Sort((x, y) => x.Message.Sent.CompareTo(y.Message.Sent));
					viewModel.OldMessages = oldmessages;
                }
            }


            return View("~/Areas/user/Views/PrivateMessage/Compose.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Compose(PrivateMessageComposeViewModel viewModel) 
        {
            if(viewModel.To == null)
            {
                ViewBag.Error = true;
                return View("~/Areas/user/Views/PrivateMessagae/Compose.cshtml", viewModel);
            }

            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            string[] tonames = viewModel.To.Split(',');
            List<string> toUsers = new List<string>();
            foreach(var name in tonames) {
                if (name.StartsWith("gruppe:", StringComparison.InvariantCultureIgnoreCase))
                {
                    string name2 = name.Substring(7);
                    if(!db.ForumGroups.Any(x => x.Name == name2)) 
                    {
                        ViewBag.Error = true;
                        return View("~/Areas/user/Views/PrivateMessagae/Compose.cshtml", viewModel);
                    }
                    var y = from u in db.Users
                            from g in db.ForumGroups
                            from m in db.ForumGroupsUsers
                            where g.Name == name2 && m.GroupId == g.Id && m.UserId == u.Id
                            select u.UserName;
                    toUsers.AddRange(y);
                }
                else
                {
                    string trimmed = name.Trim();
                    if (!db.Users.Any(x => x.UserName == name))
                    {
                        ViewBag.Error = true;
                        return View("~/Areas/user/Views/PrivateMessagae/Compose.cshtml", viewModel);
                    }
                    toUsers.Add(trimmed);
                }
            }

            List<string> bccUsers = new List<string>();
            if (viewModel.Bcc != null)
            {
                string[] bccnames = viewModel.Bcc.Split(',');

                foreach (var name in bccnames)
                {
                    if (name.StartsWith("gruppe:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string name2 = name.Substring(7);
                        if (!db.ForumGroups.Any(x => x.Name == name2))
                        {
                            ViewBag.Error = true;
                            return View("~/Areas/user/Views/PrivateMessagae/Compose.cshtml", viewModel);
                        }
                        var y = from u in db.Users
                                from g in db.ForumGroups
                                from m in db.ForumGroupsUsers
                                where g.Name == name2 && m.GroupId == g.Id && m.UserId == u.Id
                                select u.UserName;
                        bccUsers.AddRange(y);
                    }
                    else
                    {
                        string trimmed = name.Trim();
                        if (!db.Users.Any(x => x.UserName == name))
                        {
                            ViewBag.Error = true;
                            return View("~/Areas/user/Views/PrivateMessagae/Compose.cshtml", viewModel);
                        }
                        bccUsers.Add(trimmed);
                    }
                }
            }

            if(!viewModel.ConversationId.HasValue) 
            {
                viewModel.ConversationId = Guid.NewGuid();   
            }

            Parser parser = new Parser();
            PrivateMessage message = new PrivateMessage();
            message.BodyRaw = viewModel.Body;
            message.ConversationId = viewModel.ConversationId.Value;
            message.Body = parser.Parse(viewModel.Body);
            message.ToLine = viewModel.To;
            message.Subject = viewModel.Subject;
            message.Sent = DateTime.Now;
            message.SenderIpAddress = SecurityUtils.GetIPAddressAsString(HttpContext);
            if (viewModel.ReplyTo.HasValue)
            {
                int val = viewModel.ReplyTo.Value;

                if (db.PrivateMessagesInbox.Any(x => (x.UserToId == uid || x.UserFromId == uid) && x.PrivateMessageId == val) ||
                   db.PrivateMessagesSent.Any(x => (x.UserFromId == uid) && x.PrivateMessageId == val))
                {
                    viewModel.ReplyTo = val;
                }
            }

            db.PrivateMessages.Add(message);
            db.SaveChanges();

            foreach(string name in tonames) 
            {
                int toid = db.Users.Where(x => x.UserName == name).First().Id;

                PrivateMessageInbox inbox = new PrivateMessageInbox()
                {
                    UserToId = toid,
                    UserFromId = uid,
                    IsBcc = false,
                    PrivateMessageId = message.Id,
                    Read = false
                };
                db.PrivateMessagesInbox.Add(inbox);
            }

            PrivateMessageSent sent = new PrivateMessageSent();
            sent.PrivateMessageId = message.Id;
            sent.UserFromId = uid;

            db.PrivateMessagesSent.Add(sent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		public ActionResult Delete(int id, bool inbox) {
			
			int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
			if ((inbox && !db.PrivateMessagesInbox.Any(x => x.Id == id && x.UserToId   == uid)) ||
			   (!inbox && !db.PrivateMessagesSent .Any(x => x.Id == id && x.UserFromId == uid)))
				return new HttpNotFoundResult();

			var vm = new PrivateMessageDeleteViewModel();
			vm.Id = id;
			vm.Inbox = inbox;
            
			return View("~/Areas/user/Views/PrivateMessage/Delete.cshtml", vm);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id, bool inbox)
		{
			int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if ((inbox && !db.PrivateMessagesInbox.Any(x => x.Id == id && x.UserToId == uid)) ||
               (!inbox && !db.PrivateMessagesSent.Any(x => x.Id == id && x.UserFromId == uid)))
                return new HttpNotFoundResult();

			int pmid;

			if(inbox) 
			{
				var im = db.PrivateMessagesInbox.First(x => x.Id == id && x.UserToId == uid);
				pmid = im.PrivateMessageId;
				db.PrivateMessagesInbox.Remove(im);
			}
			else 
			{
				var sm = db.PrivateMessagesSent.First(x => x.Id == id && x.UserFromId == uid);
				pmid = sm.PrivateMessageId;
				db.PrivateMessagesSent.Remove(sm);
			}
			db.SaveChanges();
            
			if(!db.PrivateMessagesInbox.Any(x => x.PrivateMessageId == pmid) && 
               !db.PrivateMessagesSent.Any(x => x.PrivateMessageId == pmid) && 
               !db.PrivateMessageReports.Any(x => x.PrivateMessageId == pmid)) {
				// Nothing references this PM anymore, delte it.
				var pm = db.PrivateMessages.First(x => x.Id == pmid);
				db.PrivateMessages.Remove(pm);
				db.SaveChanges();
			}

            if(inbox)
			    return RedirectToAction("Index");
			return RedirectToAction("Outbox");
		}

        public ActionResult Report(int id) 
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (!db.PrivateMessagesInbox.Any(x => x.Id == id && x.UserToId == uid)) {
                return new HttpNotFoundResult();
            }

            var message = db.PrivateMessages.Find(id);

            return View("/Areas/user/Views/PrivateMessage/Report.cshtml", message);
        }

        [HttpPost]
        [ActionName("Report")]
        [ValidateAntiForgeryToken]
        public ActionResult ReportConfirmed(int id, string details)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (!db.PrivateMessagesInbox.Any(x => x.Id == id && x.UserToId == uid))
            {
                return new HttpNotFoundResult();
            }

            var report = new PrivateMessageReport();

            report.Details = details;
            report.PrivateMessageId = id;
            report.UserFromId = db.PrivateMessagesInbox.Where(x => x.PrivateMessageId == id).Select(x => x.UserFromId).First();
            report.UserReportId = uid;
            report.Resolved = false;
            report.IpAddress = SecurityUtils.GetIPAddressAsString(HttpContext);
            report.Timestamp = DateTime.Now;

            db.PrivateMessageReports.Add(report);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ManageLabels()
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            var labels = db.PrivateMessageLabels.Where(x => x.OwnerUserId == uid);

            return View("~/Areas/user/Views/PrivateMessage/Labels.cshtml", labels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageLabels(string label, string color)
        {
            if(label == null || color == null)
            {
                return ManageLabels();
            }

            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            PrivateMessageLabel pmlabel = new PrivateMessageLabel()
            {
                OwnerUserId = uid,
                Label = label,
                Color = color
            };

            db.PrivateMessageLabels.Add(pmlabel);
            db.SaveChanges();

            return ManageLabels();
        }

        [HttpPost]
        public ActionResult DeleteLabel(int id) 
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var label = db.PrivateMessageLabels.FirstOrDefault(x => x.OwnerUserId == uid && x.Id == id);

            if (label == null)
                return HttpNotFound();

            db.PrivateMessageLabels.Remove(label);
            db.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public ActionResult EditLabel(int pk, string name, string value) {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var label = db.PrivateMessageLabels.FirstOrDefault(x => x.OwnerUserId == uid && x.Id == pk);

            if (label == null)
                return HttpNotFound();

            switch(name)
            {
                case "color":
                    if (value.Substring(0, 1) != "#")
                        break;
                    label.Color = value.Substring(0, 7);
                    break;
                case "label":
                    label.Label = value;
                    break;
                default: break;
            }

            db.SetModified(label);
            db.SaveChanges();

            return Json(label);
        }

        [HttpPost]
        public ActionResult ValidateUser(string username) 
        {
            if(username.StartsWith("gruppe:", StringComparison.InvariantCultureIgnoreCase)) {
                username = username.Substring(7);
                return Json(db.ForumGroups.Any(x => x.Name == username));
            }

            return Json(db.Users.Any(x => x.UserName == username));
        }
    }
}
