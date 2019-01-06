// Copyright 2017 the pokefans authors. See copyright.md for legal info.
using Ganss.XSS;
using Pokefans.Caching;
using Pokefans.Data;
using Pokefans.Data.Wifi;
using Pokefans.Models;
using Pokefans.Util.Search;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Lucene.Net.Analysis;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Pokefans.Security;
using Pokefans.Util.Comments;
using Pokefans.Util;
using Lucene.Net.QueryParsers;

namespace Pokefans.Controllers
{
    public class TradingController : Controller
    {
        Entities db;
        Cache cache;
        private Analyzer analyzer;
        private Searcher searcher;
        private IndexWriter writer;
        ApplicationUserManager userManager;
        NotificationManager notificationManager;

        public TradingController(Entities ents, Cache c, IndexWriter wrtr, Searcher srchr, Analyzer ana, ApplicationUserManager um, NotificationManager mgr)
        {
            db = ents;
            cache = c;
            analyzer = ana;
            searcher = srchr;
            writer = wrtr;
            userManager = um;
            notificationManager = mgr;
        }

        // GET: Trading
        public ActionResult Index(int start = 0)
        {
            TradingIndexViewModel tivm = new TradingIndexViewModel();

            if (!cache.Contains("TradingTeaser"))
            {
                int teaserid = int.Parse(ConfigurationManager.AppSettings["TradingTeaserId"]);
                tivm.TeaserContent = db.Contents.FirstOrDefault(g => g.Id == teaserid);
                if (tivm.TeaserContent != null)
                    cache.Add("TradingTeaser", tivm.TeaserContent, TimeSpan.FromDays(1));
            }
            else
            {
                tivm.TeaserContent = cache.Get<Content>("TradingTeaser");
            }
            var query = db.WifiOffers.Include("Pokemon").Include("User").Where(x => x.Status == TradingStatus.Offer);

            if (start != 0)
                query = query.Where(x => x.Id < start);

            tivm.Offers = query.OrderByDescending(g => g.UpdateTime).Take(50).ToList();

            return View("~/Views/Trading/Index.cshtml", tivm);
        }

        [Authorize]
        public ActionResult Reactivate(int id)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            if (!db.WifiOffers.Any(x => x.Id == id && x.UserId == uid))
                return RedirectToAction("Details", new { id });

            Offer o = db.WifiOffers.First(x => x.Id == id);

            o.UpdateTime = DateTime.Now;

            db.SetModified(o);
            db.SaveChanges();

            return RedirectToAction("Details", new { id });

        }

        [Authorize]
        public ActionResult Protocol(int start = 0) {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var query = db.TradeLogs.Include("Offer").Include("Offer.Pokemon").Include("Offer.Item").Include("UserFrom").Include("UserTo").Where(x => x.UserFromId == uid || x.UserToId == uid);

            if(start > 0)
                query = query.Where(x => x.Id < start);

            var log = query.Take(50).ToList();

            return View(log);

        }

        [Authorize]
        public ActionResult ConfirmTrade(int id)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var log = db.TradeLogs.FirstOrDefault(x => x.IsValid == false && x.Id == id && x.UserToId == uid);
            if (log == null)
            {
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/ConfirmImpossible.cshtml");
            }

            log.IsValid = true;
            log.ValidOn = DateTime.Now;

            db.SetModified(log);
            db.SaveChanges();

            return RedirectToAction("Protocol");
        }


        [Authorize]
        public ActionResult Rate(int id, int amount)
        {
            if(amount < -1 || amount > 1)
            {
                HttpContext.Response.StatusCode = 400;
                HttpContext.Response.Status = "Bad Request";
                return View("~/Views/Trading/Errors/RateTamper.cshtml");
            }

            TradeLog log = db.TradeLogs.FirstOrDefault(x => x.Id == id);

            if (log == null)
                return RedirectToAction("Protocol");

            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if(log.UserToId != uid && log.UserFromId != uid)
            {
                HttpContext.Response.StatusCode = 400;
                HttpContext.Response.Status = "Bad Request";
                return View("~/Views/Trading/Errors/RateTamper.cshtml");
            }

            if (log.UserToId == uid)
                log.SellerRating = (TradeRating)amount;
            else
                log.CustomerRating = (TradeRating)amount;

            db.SetModified(log);
            db.SaveChanges();

            // re-calculate the ratings for the seller
            calculateRating(log.UserToId);

            // re-calculate the ratings for the buyer
            calculateRating(log.UserFromId);


            return RedirectToAction("Protocol");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowCors]
        public ActionResult RateJson(int id, int amount)
        {
            if (amount < -1 || amount > 1)
            {
                HttpContext.Response.StatusCode = 400;
                HttpContext.Response.Status = "Bad Request";
                return Json(new { success = false });
            }

            TradeLog log = db.TradeLogs.FirstOrDefault(x => x.Id == id);

            if (log == null)
            {
                HttpContext.Response.StatusCode = 404;
                HttpContext.Response.Status = "Not Found";
                return Json(new { success = false });
            }

            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (log.UserToId != uid && log.UserFromId != uid)
            {
                HttpContext.Response.StatusCode = 400;
                HttpContext.Response.Status = "Bad Request";
                return Json(new { success = false });
            }

            if (log.UserToId == uid)
                log.SellerRating = (TradeRating)amount;
            else
                log.CustomerRating = (TradeRating)amount;

            db.SetModified(log);
            db.SaveChanges();

            // re-calculate the ratings for the seller
            calculateRating(log.UserToId);

            // re-calculate the ratings for the buyer
            calculateRating(log.UserFromId);
            return Json(new { success = true });
        }

        private void calculateRating(int uid)
        {
            int buyeruid = uid;

            int buyerratecount = db.TradeLogs.Count(x => x.UserFromId == buyeruid && x.SellerRating != null) + db.TradeLogs.Count(x => x.UserToId == buyeruid && x.CustomerRating != null);

            int buyerPositive = db.TradeLogs.Count(x => x.UserFromId == buyeruid && x.SellerRating == TradeRating.Positive) + db.TradeLogs.Count(x => x.UserToId == buyeruid && x.CustomerRating == TradeRating.Positive);
            int buyerNeutral = db.TradeLogs.Count(x => x.UserFromId == buyeruid && x.SellerRating == TradeRating.Neutral) + db.TradeLogs.Count(x => x.UserToId == buyeruid && x.CustomerRating == TradeRating.Neutral);
            int buyerNegative = db.TradeLogs.Count(x => x.UserFromId == buyeruid && x.SellerRating == TradeRating.Negative) + db.TradeLogs.Count(x => x.UserToId == buyeruid && x.CustomerRating == TradeRating.Negative);

            User buyer = db.Users.First(x => x.Id == buyeruid);
            buyer.TradingPercentPositive = (int)(buyerPositive * 100d / buyerratecount);
            buyer.TradingPercentNeutral = (int)(buyerNeutral * 100d / buyerratecount);
            buyer.TradingPercentNegative = (int)(buyerNegative * 100d / buyerratecount);

            db.SetModified(buyer);
            db.SaveChanges();
        }

		[Authorize]
		public ActionResult My(int start = 0)
		{
			int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            //var query = db.TradeLogs.Include("Offer").Include("Offer.Pokemon").Include("Offer.Item").Include("UserTo").Where(x => x.UserFromId == uid);
            var query = db.WifiOffers.Include("Pokemon").Include("Item").Include("User").Where(x => x.UserId == uid);

			if (start > 0)
				query = query.Where(x => x.Id < start);

			var log = query.Take(50).ToList();

			return View(log);

		}

        [Authorize]
        public ActionResult AddOffer()
        {
            fetchAddData();

            return View();
        }

        private void fetchAddData()
        {
            //TODO: maybe centralize the caching. I have a feeling that these caches will be needed in other places too.

            List<SelectListItem> pokemon;
            List<SelectListItem> items;
            List<SelectListItem> abilities;
            List<SelectListItem> attacks;
            List<SelectListItem> natures;

            if (cache.Contains("PokemonSelectList"))
                pokemon = cache.Get<List<SelectListItem>>("PokemonSelectList");
            else {
                pokemon = new List<SelectListItem>();
                db.Pokemon.OrderBy(g => g.PokedexId.National).ToList().ForEach(g => pokemon.Add(new SelectListItem() { Value = g.Id.ToString(), Text = g.Name.German }));
                cache.Add("PokemonSelectList", pokemon);
            }
            if (cache.Contains("ItemSelectList"))
                items = cache.Get<List<SelectListItem>>("ItemSelectList");
            else {
                items = new List<SelectListItem>();
                items.Add(new SelectListItem() { Text = "Kein", Value = null });
                db.Items.ToList().ForEach(g => items.Add(new SelectListItem() { Value = g.Id.ToString(), Text = g.Name.German }));
            }

            if (cache.Contains("AbilitySelectList"))
                abilities = cache.Get<List<SelectListItem>>("AbilitySelectList");
            else {
                abilities = new List<SelectListItem>();
                abilities.Add(new SelectListItem() { Text = "Keine Angabe", Value = null });
                db.Abilities.ToList().ForEach(g => abilities.Add(new SelectListItem() { Value = g.Id.ToString(), Text = g.Name.German }));
            }

            if (cache.Contains("AttackSelectList"))
                attacks = cache.Get<List<SelectListItem>>("AttackSelectList");
            else {
                attacks = new List<SelectListItem>();
                attacks.Add(new SelectListItem() { Text = "Keine", Value = null });
                db.Attacks.ToList().ForEach(g => attacks.Add(new SelectListItem() { Value = g.Id.ToString(), Text = g.Name.German }));
            }

            if (cache.Contains("NatureSelectList"))
                natures = cache.Get<List<SelectListItem>>("NatureSelectList");
            else {
                natures = new List<SelectListItem>();
                natures.Add(new SelectListItem() { Text = "Keine Angabe", Value = null });
                db.Natures.ToList().ForEach(g => natures.Add(new SelectListItem() { Value = g.Id.ToString(), Text = g.Name }));
            }

            ViewBag.Pokemon = pokemon;
            ViewBag.Items = items;
            ViewBag.Natures = natures;
            ViewBag.Attacks = attacks;
            ViewBag.Abilities = abilities;

            Content infosection;

            if (!cache.Contains("TradingAddInfoSection"))
            {
                int teaserid = int.Parse(ConfigurationManager.AppSettings["TradingAddInfoSection"]);
                infosection = db.Contents.FirstOrDefault(g => g.Id == teaserid);
                if (infosection != null)
                    cache.Add("TradingAddInfoSection", infosection, TimeSpan.FromDays(1));
            }
            else
            {
                infosection = cache.Get<Content>("TradingAddInfoSection");
            }

            ViewBag.Infosection = infosection;
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddOffer(NormalOffer no)
        {

            if(no.Title == null)
            {
                no.Title = db.Pokemon.Where(x => x.Id == no.PokemonId).Select(x => x.Name.German).First();
            }

            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            no.Description = sanitizer.Sanitize(no.DescriptionCode);

            no.UserId = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            no.UpdateTime = DateTime.Now;

            db.WifiOffers.Add(no);
            db.SaveChanges();

            // now we have the database id, let's add this to the search index
            NormalOffer o = db.WifiOffers.Include("Pokemon")
                                         .Include("Attack1")
                                         .Include("Attack2")
                                         .Include("Attack3")
                                         .Include("Ability")
                                         .Include("User")
                                         .Include("Pokeball")
                                         .Include("Nature").OfType<NormalOffer>().First(g => g.Id == no.Id);

            writer.AddDocument(DocumentGenerator.WifiOffer(o));

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            // just get normal offers, for now. Has to be expanded if we have more types in the future
            // TODO: replace with stuff from cache where appropriate
            NormalOffer o = db.WifiOffers.Include("Pokemon")
                                         .Include("Attack1")
                                         .Include("Attack2")
                                         .Include("Attack3")
                                         .Include("Ability")
                                         .Include("User")
                                         .Include("Pokeball")
                                         .Include("Nature").OfType<NormalOffer>().First(g => g.Id == id);

            ViewBag.OfferCount = db.WifiOffers.Where(g => g.UserId == o.UserId).Count();

            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            OfferViewModel offerViewModel = new OfferViewModel();


            if(o.UserId != uid)
            {
                offerViewModel.Interest = db.WifiInterests.FirstOrDefault(g => g.OfferId == id && g.UserId == uid);
            }

            TradingCommentManager tcm = new TradingCommentManager(db, cache, HttpContext, notificationManager);
            CommentsViewModel cvm = new CommentsViewModel();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User currentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
                cvm.CanHideComment = currentUser.IsInRole("fanart-moderator", cache, db);
                cvm.CurrentUser = currentUser;
            }
            else
            {
                cvm.CanHideComment = false;
                cvm.CurrentUser = null;
            }

            cvm.Comments = tcm.GetCommentsForObjectId(id);
            cvm.CommentedObjectId = id;
            cvm.Context = CommentContext.Trading;

            cvm.Level = 0; //note that this is the level we start from, so we can arbitrarily limit comment indentation between 0 and 4 levels
            cvm.Manager = tcm;

            offerViewModel.Offer = o;
            offerViewModel.Comments = cvm;

            return View(offerViewModel);
        }

        [Authorize]
        public ActionResult SelectPartner(int id, int interest)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (!db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                // technically also 404 is handled here which is not ideal but eh...
                // nobody cares if it only affects real users
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/ManageOnlyOwn.cshtml");
            }
            if (!db.WifiInterests.Any(x => x.Id == interest && x.OfferId == id))
            {
                // technically also 404 is handled here which is not ideal but eh...
                // nobody cares if it only affects real users
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/ManageOnlyOwn.cshtml");
            }

            // We have now verified that (a) a suitable Offer and (b) a suitable interest exist
            Offer offer = db.WifiOffers.First(x => x.Id == id);
            offer.Status = TradingStatus.PartnerChosen;

            Interest i = db.WifiInterests.First(x => x.Id == interest);

            TradeLog log = new TradeLog()
            {
                IsValid = false,
                ValidOn = null,
                CompletedTime = null,
                UserFromId = uid,
                UserToId = i.UserId,
                OfferId = offer.Id
            };

            db.SetModified(offer);
            db.TradeLogs.Add(log);
            db.SaveChanges();

            notificationManager.SendNotification(i.UserId, $"Du wurdest als Tauschpartner für <a href=\"{Url.Map("tausch/" + offer.Id.ToString(), "")}\">{HttpUtility.HtmlEncode(offer.Title)}</a> ausgewählt." +
                                                            "Der/Die Anbieter_in wird sich bald bei dir melden.",
                                                 "<i class=\"fa fa-refresh\"></i>");

            return RedirectToAction("Manage", new { id });
        }

        [Authorize]
        public ActionResult Withdraw(int id)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (!db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                // technically also 404 is handled here which is not ideal but eh...
                // nobody cares if it only affects real users
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/ManageOnlyOwn.cshtml");

            }
            Offer offer = db.WifiOffers.First(x => x.Id == id);

            if (offer.Status == TradingStatus.Completed || offer.Status == TradingStatus.Deleted)
                return RedirectToAction("Manage", new { id });

            offer.Status = TradingStatus.Withdrawn;

            db.SetModified(offer);
            db.SaveChanges();

            // remove from the search index
            var query = new BooleanQuery();

            var typeQuery = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "type", analyzer);
            query.Add(typeQuery.Parse("wifioffer"), Occur.MUST);

            var idQuery = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Id", analyzer);
            query.Add(idQuery.Parse(offer.Id.ToString()), Occur.MUST);

            writer.DeleteDocuments(query);

            return RedirectToAction("Manage", new { id });
        }

        [Authorize]
        public ActionResult Reopen(int id)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (!db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                // technically also 404 is handled here which is not ideal but eh...
                // nobody cares if it only affects real users
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/ManageOnlyOwn.cshtml");

            }
            Offer offer = db.WifiOffers.First(x => x.Id == id);

            if (offer.Status != TradingStatus.Withdrawn)
                return RedirectToAction("Manage", new { id });

            offer.Status = TradingStatus.Offer;

            db.SetModified(offer);
            db.SaveChanges();

            return RedirectToAction("Manage", new { id });
        }

        [Authorize]
        public ActionResult SetDone(int id)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (!db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                // technically also 404 is handled here which is not ideal but eh...
                // nobody cares if it only affects real users
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/ManageOnlyOwn.cshtml");

            }
            Offer offer = db.WifiOffers.First(x => x.Id == id);

            if (offer.Status != TradingStatus.PartnerChosen)
                return RedirectToAction("Manage", new { id });

            offer.Status = TradingStatus.Completed;
            db.SetModified(offer);

            TradeLog log = db.TradeLogs.Where(x => x.OfferId == id).First();
            log.CompletedTime = DateTime.Now;
            log.ValidOn = DateTime.Now.AddHours(48);

            db.SetModified(log);
            db.SaveChanges();

            notificationManager.SendNotification(log.UserToId, $"Der Tausch <a href=\"{Url.Map("tausch/" + offer.Id.ToString(), "")}\">{HttpUtility.HtmlEncode(offer.Title)}</a> wurde von." +
                                                            $"deinem_r Tauschpartner_in als erledigt markiert. Du kannst das in deinem <a href=\"{Url.Map("tausch/protokoll", "")}\">Tauschprotokoll</a> bestätigen.",
                                                 "<i class=\"fa fa-refresh\"></i>");

            return RedirectToAction("Manage", new { id });
        }

        [Authorize]
        public ActionResult BackToPartnerSelect(int id)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (!db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                // technically also 404 is handled here which is not ideal but eh...
                // nobody cares if it only affects real users
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/ManageOnlyOwn.cshtml");

            }
            Offer offer = db.WifiOffers.First(x => x.Id == id);

            if (offer.Status != TradingStatus.PartnerChosen)
                return RedirectToAction("Manage", new { id });

            TradeLog log = db.TradeLogs.First(x => x.OfferId == id);
            db.TradeLogs.Remove(log);

            offer.Status = TradingStatus.Offer;
            db.SetModified(offer);

            db.SaveChanges();

            return RedirectToAction("Manage", new { id });
        }

        [Authorize]
        public ActionResult Manage(int id)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            if (!db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                // technically also 404 is handled here which is not ideal but eh...
                // nobody cares if it only affects real users
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/ManageOnlyOwn.cshtml");
            }

            Offer offer = db.WifiOffers.First(x => x.Id == id);

            switch (offer.Status)
            {
                case TradingStatus.Completed: return showArchive(offer);
                case TradingStatus.Deleted: return View("~/Views/Trading/Errors/ModerationDeleted.cshtml");
                case TradingStatus.Withdrawn: return showWithdrawn(offer);
                case TradingStatus.Offer: return showInterests(offer);
                case TradingStatus.PartnerChosen: return showCompletion(offer);
                default: return View("~/Views/Trading/Errors/Status.cshtml");
            }
        }

        private ActionResult showInterests(Offer o) {
            ManageInterestViewModel viewModel = new ManageInterestViewModel();

            viewModel.Offer = o;
            viewModel.Interests = db.WifiInterests.Include("User").Where(x => x.OfferId == o.Id).OrderBy(x => x.Timestamp).ToList();

            return View("~/Views/Trading/ManageInterests.cshtml", viewModel);
        }

        private ActionResult showArchive(Offer o)
        {
            OfferArchiveViewModel viewModel = new OfferArchiveViewModel();

            viewModel.Offer = o;
            viewModel.TradeLog = db.TradeLogs.First(x => x.OfferId == o.Id);

            return View("~/Views/Trading/ManageArchive.cshtml", viewModel);
        }

        private ActionResult showWithdrawn(Offer o)
        {
            return View("~/Views/Trading/ManageWithdrawn.cshtml", o);
        }

        private ActionResult showCompletion(Offer o)
        {
            OfferArchiveViewModel viewModel = new OfferArchiveViewModel();

            viewModel.Offer = o;
            viewModel.TradeLog = db.TradeLogs.Include("UserTo").First(x => x.OfferId == o.Id);

            return View("~/Views/Trading/ManageCompletion.cshtml", viewModel);
        }

        [Authorize]
        public ActionResult Interest(int id)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if(db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/NoInterestOwn.cshtml");
            }
            if(db.WifiInterests.Any(x => x.UserId == uid && x.OfferId == id))
            {
                return View("~/Views/Trading/Errors/AlreadyInterested.cshtml");
            }

            Offer offer = db.WifiOffers.First(x => x.Id == id);

            return View(offer);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Interest(int id, string comment)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return View("~/Views/Trading/Errors/NoInterestOwn.cshtml");
            }
            if (db.WifiInterests.Any(x => x.UserId == uid && x.OfferId == id))
            {
                return View("~/Views/Trading/Errors/AlreadyInterested.cshtml");
            }

            Interest interest = new Interest();
            interest.OfferId = id;
            interest.Comment = comment;
            interest.Timestamp = DateTime.Now;
            interest.UserId = uid;
            interest.Ip = SecurityUtils.GetIPAddressAsString(HttpContext);

            db.WifiInterests.Add(interest);
            db.SaveChanges();

            Offer o = db.WifiOffers.Include("User").First(x => x.Id == id);

            User u = db.Users.First(x => x.Id == uid);
            notificationManager.SendNotification(o.UserId,
                                                 $"<a href=\"{Url.Map("profil/" + u.Url, "user")}\">{u.UserName}</a> ist an deinem Tauschbörsen-Angebot " +
                                                 $"<a href=\"{Url.Map("tausch/" + o.Id.ToString(), "")}\">{HttpUtility.HtmlEncode(o.Title)}</a> interessiert.",
                                                "<i class=\"fa fa-shopping-cart\"></i>");

            return View("~/Views/Trading/InterestSuccess.cshtml", id);
        }

        [Authorize]
        [HttpPost]
        [AllowCors]
        public ActionResult InterestApi(int id, string comment)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
            if (db.WifiOffers.Any(x => x.UserId == uid && x.Id == id))
            {
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.Status = "Forbidden";
                return Json(new { success = false, error = "Can't interest own offer" });
            }
            if (db.WifiInterests.Any(x => x.UserId == uid && x.OfferId == id))
            {
                return Json(new { success = false, error = "Already interested" });
            }

            Interest interest = new Interest();
            interest.OfferId = id;
            interest.Comment = comment;
            interest.Timestamp = DateTime.Now;
            interest.UserId = uid;
            interest.Ip = SecurityUtils.GetIPAddressAsString(HttpContext);

            db.WifiInterests.Add(interest);
            db.SaveChanges();


            Offer o = db.WifiOffers.Include("User").First(x => x.Id == id);
            User u = db.Users.First(x => x.Id == uid);
            notificationManager.SendNotification(o.UserId,
                                                 $"<a href=\"{Url.Map("profil/" + u.Url, "user")}\">{u.UserName}</a> ist an deinem Tauschbörsen-Angebot " +
                                                 $"<a href=\"{Url.Map("tausch / " + o.Id.ToString(), "")}\">{HttpUtility.HtmlEncode(o.Title)}</a> interessiert.",
                                                "<i class=\"fa fa-shopping-cart\"></i>");

            return Json(new { success = true, error = "" });
        }

    }
}
