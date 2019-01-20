using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ChartJSCore.Models;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Microsoft.AspNet.Identity;
using Pokefans.Areas.mitarbeit.Models;
using Pokefans.Data;
using Pokefans.Data.Wifi;
using Pokefans.Util;
using Pokefans.Util.Search;
using Pokefans.Caching;

namespace Pokefans.Controllers
{
    [Authorize(Roles = "wifi-moderator")]
    public class WifiController : Controller
    {

        Entities db;
        NotificationManager notificationManager;
        private Analyzer analyzer;
        private Searcher searcher;
        private IndexWriter writer;
        Cache cache;

        public WifiController(Entities ents, NotificationManager mgr, IndexWriter wrtr, Searcher srchr, Analyzer ana, Cache c)
        {
            db = ents;
            notificationManager = mgr;
            analyzer = ana;
            searcher = srchr;
            writer = wrtr;
            cache = c;
        }

        public ActionResult Index()
        {
            return View("~/Areas/mitarbeit/Views/Wifi/Index.cshtml");
        }

        public ActionResult Banlist(int start = 0)
        {
            var bans = db.WifiBanlist.Include("User").OrderBy(x => x.User.UserName).Skip(start).Take(50);

            ViewBag.HasMore = db.WifiBanlist.Count() > (start + 50);
            ViewBag.Start = start;

            return View("~/Areas/mitarbeit/Views/Wifi/Banlist.cshtml", bans);
        }

        public ActionResult Reports(int start = 0)
        {
            var reports = db.OfferReports.Include("User").Include("Offer").Include("Offer.User").OrderByDescending(x => x.ReportedOn).Skip(start).Take(50);

            ViewBag.HasMore = db.OfferReports.Count() > (start + 50);
            ViewBag.Start = start;

            return View("~/Areas/mitarbeit/Views/Wifi/Reports.cshtml", reports);
        }

        #region Wifi Dashboard API

        [AllowCors]
        public ActionResult Report(int id)
        {
            string profilebase = Url.Profile("");
            string offerbase = Url.Map("tausch/detail/", null);

            var result = (from x in db.OfferReports.Include("User").Include("Offer").Include("Offer.User")
                          where x.Id == id
                          select x)
                         .AsEnumerable()
                         .Select(x => new // dynamic object generation must be done with LINQ to Object
                         {                 // because DateTime.ToString()
                             title = x.Offer.Title,
                             reportingUser = new
                             {
                                 name = x.User.UserName,
                                 css = x.User.Color,
                                 url = profilebase + x.User.Url
                             },
                             user = new
                             {
                                 name = x.Offer.User.UserName,
                                 css = x.Offer.User.Color,
                                 url = profilebase + x.Offer.User.Url
                             },
                             reportText = x.Comment,
                             time = x.ReportedOn.ToString("dd.MM.yyyy HH:mm"),
                             offerUrl = offerbase + x.Offer.Id.ToString(),
                             canDelete = x.Offer.Status != TradingStatus.Completed,
                             isDeleted = x.Offer.Status == TradingStatus.Deleted,
                             isResolved = x.Resolved,
                             isCheat = x.Offer.CheatUsed
                         }).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowCors]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cheat(int id, string comment)
        {
            var offer = (from s in db.OfferReports
                         join x in db.WifiOffers on s.OfferId equals x.Id
                         where s.Id == id
                         select x).FirstOrDefault();

            if (offer != null)
            {

                TradingNoteViewModel nvm = new TradingNoteViewModel()
                {
                    Offer = offer,
                    Comment = comment
                };
                int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());
                Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");

                offer.CheatUsed = !offer.CheatUsed;
                db.SetModified(offer);




                if (offer.CheatUsed)
                {
                    notificationManager.SendNotification(offer.UserId, $"Dein Tauschangebot <a href=\"{Url.Map("tausch/" + offer.Id.ToString(), "")}\">{HttpUtility.HtmlEncode(offer.Title)}</a> wurde von" +
                                                                    $"einem_r Moderator_in als Cheat markiert. Bitte achte in Zukunft darauf, deine Angebote korrekt zu kennzeichnen.",
                                                         "<i class=\"fa fa-refresh\"></i>");


                    UserNote n = new UserNote()
                    {
                        AuthorId = uid,
                        ActionId = actions["tb-offer-cheat-add"],
                        Created = DateTime.Now,
                        IsDeletable = false,
                        RoleIdNeeded = db.Roles.First(x => x.Name == "wifi-moderator").Id,
                        UserId = offer.UserId,
                        Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/WifiOfferCheatAdd.cshtml", nvm),
                        UnparsedContent = ""
                    };

                    db.UserNotes.Add(n);
                }
                else
                {
                    UserNote n = new UserNote()
                    {
                        AuthorId = uid,
                        ActionId = actions["tb-offer-cheat-remove"],
                        Created = DateTime.Now,
                        IsDeletable = false,
                        RoleIdNeeded = db.Roles.First(x => x.Name == "wifi-moderator").Id,
                        UserId = offer.UserId,
                        Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/WifiOfferRemove.cshtml", nvm),
                        UnparsedContent = ""
                    };

                    db.UserNotes.Add(n);
                }

                db.SaveChanges();
            }

            return Report(id);
        }

        [AllowCors]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string comment)
        {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var offer = (from s in db.OfferReports
                         join x in db.WifiOffers on s.OfferId equals x.Id
                         where s.Id == id
                         select x).FirstOrDefault();

            TradingNoteViewModel nvm = new TradingNoteViewModel()
            {
                Offer = offer,
                Comment = comment
            };
            Dictionary<string, int> actions = cache.Get<Dictionary<string, int>>("SystemUserNoteActions");

            if (offer != null && offer.Status != TradingStatus.Completed)
            {
                if (offer.Status == TradingStatus.Deleted)
                {
                    offer.Status = TradingStatus.Offer;

                    writer.AddDocument(DocumentGenerator.WifiOffer((NormalOffer)offer));

                    notificationManager.SendNotification(offer.UserId, $"Dein Tauschangebot <a href=\"{Url.Map("tausch/" + offer.Id.ToString(), "")}\">{HttpUtility.HtmlEncode(offer.Title)}</a> wurde von" +
                                                                       $"der Moderation wieder freigeschalten. Viel Spaß beim Tauschen!",
                                                         "<i class=\"fa fa-refresh\"></i>");

                    UserNote n = new UserNote()
                    {
                        AuthorId = uid,
                        ActionId = actions["tb-offer-reopened"],
                        Created = DateTime.Now,
                        IsDeletable = false,
                        RoleIdNeeded = db.Roles.First(x => x.Name == "wifi-moderator").Id,
                        UserId = offer.UserId,
                        Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/WifiOfferReopened.cshtml", nvm),
                        UnparsedContent = ""
                    };

                    db.UserNotes.Add(n);
                }
                else
                {
                    if (offer.Status == TradingStatus.PartnerChosen)
                    {
                        // delete the trade log
                        TradeLog log = db.TradeLogs.First(x => x.OfferId == offer.Id);
                        db.TradeLogs.Remove(log);
                    }
                    offer.Status = TradingStatus.Deleted;

                    // remove from the search index
                    var query = new BooleanQuery();

                    var typeQuery = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "type", analyzer);
                    query.Add(typeQuery.Parse("wifioffer"), Occur.MUST);

                    var idQuery = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Id", analyzer);
                    query.Add(idQuery.Parse(offer.Id.ToString()), Occur.MUST);

                    writer.DeleteDocuments(query);

                    notificationManager.SendNotification(offer.UserId, $"Dein Tauschangebot <a href=\"{Url.Map("tausch/" + offer.Id.ToString(), "")}\">{HttpUtility.HtmlEncode(offer.Title)}</a> wurde von" +
                                                                    $"der Moderation wegen eines Regelverstoßes gelöscht. Bitte halte dich in Zukunft an unsere Community-Richtlinien!",
                                                         "<i class=\"fa fa-refresh\"></i>");

                    UserNote n = new UserNote()
                    {
                        AuthorId = uid,
                        ActionId = actions["tb-offer-deleted"],
                        Created = DateTime.Now,
                        IsDeletable = false,
                        RoleIdNeeded = db.Roles.First(x => x.Name == "wifi-moderator").Id,
                        UserId = offer.UserId,
                        Content = this.RenderViewToString("~/Areas/mitarbeit/Views/_NoteTemplates/WifiOfferDeleted.cshtml", nvm),
                        UnparsedContent = ""
                    };

                    db.UserNotes.Add(n);
                }
                db.SetModified(offer);
                db.SaveChanges();
            }

            return Report(id);
        }

        [AllowCors]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resolve(int id)
        {
            var report = db.OfferReports.Include("Offer").FirstOrDefault(x => x.Id == id);
            report.Resolved = !report.Resolved;
            db.SetModified(report);
            db.SaveChanges();

            var offer = report.Offer;

            notificationManager.SendNotification(report.UserId, $"Deine Meldung zum Tausch <a href=\"{Url.Map("tausch/" + offer.Id.ToString(), "")}\">{HttpUtility.HtmlEncode(offer.Title)}</a> wurde gerade von" +
                                                            $"einem_r Moderator_in bearbeitet. Danke für deine Mithilfe!",
                                                 "<i class=\"fa fa-refresh\"></i>");

            return Report(id);
        }

        [AllowCors]
        public ActionResult Statistics()
        {
            DateTime weekAgo = DateTime.Now.AddDays(-7);

            object ret = new
            {
                total = db.WifiOffers.Count(),
                totalOpen = db.WifiOffers.Count(x => x.Status == Data.Wifi.TradingStatus.Offer),
                newOffers = db.WifiOffers.Count(x => x.CreationDate > weekAgo),
                finished = db.TradeLogs.Count(x => x.CompletedTime > weekAgo),
                openReports = db.OfferReports.Count(x => x.Resolved == false),
                reports = db.OfferReports.Count(x => x.ReportedOn > weekAgo)
            };

            return Json(ret, JsonRequestBehavior.AllowGet);

        }

        [AllowCors]
        public ActionResult Activity(DateTime from, DateTime until)
        {
            Chart chart = new Chart();
            chart.Type = "line";

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            var reportsInTime = db.WifiOffers.Where(x => x.UpdateTime >= from && x.UpdateTime <= until).ToList();

            var ReportsPerDay = new Dictionary<DateTime, int>();
            DateTime current = from;
            while (current < until)
            {
                ReportsPerDay.Add(current, reportsInTime.Count(x => x.UpdateTime.DayOfYear == current.DayOfYear));
                current = current.AddDays(1);
            }

            data.Labels = ReportsPerDay.Select(x => x.Key.ToString("dd.MM.yyyy")).ToList();
            data.Datasets = new List<Dataset>();

            data.Datasets.Add(new LineDataset()
            {
                Label = "Aktive Angebote",
                Data = ReportsPerDay.Select(x => (double)x.Value).ToList(),
                BackgroundColor = "rgba(210, 214, 222, 1)"
            });


            var completedInTime = db.TradeLogs.Where(x => x.CompletedTime != null && x.CompletedTime >= from && x.CompletedTime <= until).ToList();

            ReportsPerDay = new Dictionary<DateTime, int>();
            current = from;
            while (current < until)
            {
                ReportsPerDay.Add(current, completedInTime.Count(x => x.CompletedTime.Value.DayOfYear == current.DayOfYear));
                current = current.AddDays(1);
            }


            data.Datasets.Add(new LineDataset()
            {
                Label = "Abgeschlossene Angebote",
                Data = ReportsPerDay.Select(x => (double)x.Value).ToList(),
                BackgroundColor = "rgba(60,141,188,1)"
            });


            chart.Options.Legend = new Legend();
            chart.Options.Legend.Display = true;
            chart.Options.Responsive = true;
            chart.Options.MaintainAspectRatio = true;
            chart.Options.Scales = new Scales();
            chart.Options.Scales.YAxes = new List<Scale>();
            chart.Options.Scales.YAxes.Add(new CartesianScale()
            {
                Ticks = new CartesianLinearTick()
                {
                    SuggestedMin = 0,
                    SuggestedMax = 20,
                    StepSize = 2
                }
            });
            chart.Data = data;

            return Content(chart.SerializeBody(), "text/json");
        }

        [AllowCors]
        public ActionResult Generations(DateTime from, DateTime until)
        {
            Chart chart = new Chart();
            chart.Type = "bar";

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            var generations = db.WifiOffers.Select(x => x.Generation).Distinct().OrderBy(x => x).ToList();
            data.Labels = generations.Select(x => x.ToString()).ToList();

            var barData = new List<double>();
            var bgColor = new List<string>();

            foreach (var gen in generations)
            {
                barData.Add(db.WifiOffers.Count(x => x.Generation == gen));
                bgColor.Add("rgba(60, 141, 188, 1)");
            }

            BarDataset set = new BarDataset()
            {
                Data = barData,
                BackgroundColor = bgColor
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(set);

            chart.Options.Legend = new Legend();
            chart.Options.Legend.Display = false;
            chart.Options.Responsive = true;
            chart.Options.MaintainAspectRatio = true;

            chart.Data = data;

            return Content(chart.SerializeBody(), "text/json");
        }

        public ActionResult PokemonTop10(DateTime from, DateTime until)
        {

            DateTime fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            DateTime untilDate = new DateTime(until.Year, until.Month, until.Day, 23, 59, 59);


            var results = (from a in
                               from k in
                                   from x in db.WifiOffers
                                   group x by x.PokemonId into y
                                   select y.Key
                               from z in db.Pokemon
                               where k == z.Id
                               select new { Id = k, pokemon = z.Name.German }
                           join b in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Offer
                             && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.PokemonId != null
                             group x by x.PokemonId into offen
                             select new { Id = offen.Key.Value, offen = offen.Count() }
                            on a.Id equals b.Id into tb
                           from b in tb.DefaultIfEmpty(new { Id = -1, offen = 0 })
                           join c in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Completed
                               && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                               && x.PokemonId != null
                             group x by x.PokemonId into completed
                             select new { Id = completed.Key.Value, completed = completed.Count() }
                            on a.Id equals c.Id into tc
                           from c in tc.DefaultIfEmpty(new { Id = -1, completed = 0 })
                           join d in
                             from x in db.WifiOffers
                             where x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.PokemonId != null
                             group x by x.PokemonId into gesamt
                             select new { Id = gesamt.Key.Value, gesamt = gesamt.Count() }
                            on a.Id equals d.Id into td
                           from d in td.DefaultIfEmpty(new { Id = -1, gesamt = 0 })
                           orderby d.gesamt descending
                           select new { a.pokemon, b.offen, c.completed, sum = d.gesamt }).Take(10);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PokemonTop10Completed(DateTime from, DateTime until)
        {
            DateTime fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            DateTime untilDate = new DateTime(until.Year, until.Month, until.Day, 23, 59, 59);


            var results = (from a in
                               from k in
                                   from x in db.WifiOffers
                                   group x by x.PokemonId into y
                                   select y.Key
                               from z in db.Pokemon
                               where k == z.Id
                               select new { Id = k, pokemon = z.Name.German }
                           join b in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Offer
                             && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.PokemonId != null
                             group x by x.PokemonId into offen
                             select new { Id = offen.Key.Value, offen = offen.Count() }
                            on a.Id equals b.Id into tb
                           from b in tb.DefaultIfEmpty(new { Id = -1, offen = 0 })
                           join c in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Completed
                               && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                               && x.PokemonId != null
                             group x by x.PokemonId into completed
                             select new { Id = completed.Key.Value, completed = completed.Count() }
                            on a.Id equals c.Id into tc
                           from c in tc.DefaultIfEmpty(new { Id = -1, completed = 0 })
                           join d in
                             from x in db.WifiOffers
                             where x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.PokemonId != null
                             group x by x.PokemonId into gesamt
                             select new { Id = gesamt.Key.Value, gesamt = gesamt.Count() }
                            on a.Id equals d.Id into td
                           from d in td.DefaultIfEmpty(new { Id = -1, gesamt = 0 })
                           where c.completed > 0
                           orderby c.completed descending
                           select new { a.pokemon, b.offen, c.completed, sum = d.gesamt }).Take(10);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PokemonTop10Open(DateTime from, DateTime until)
        {
            DateTime fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            DateTime untilDate = new DateTime(until.Year, until.Month, until.Day, 23, 59, 59);


            var results = (from a in
                               from k in
                                   from x in db.WifiOffers
                                   group x by x.PokemonId into y
                                   select y.Key
                               from z in db.Pokemon
                               where k == z.Id
                               select new { Id = k, pokemon = z.Name.German }
                           join b in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Offer
                             && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.PokemonId != null
                             group x by x.PokemonId into offen
                             select new { Id = offen.Key.Value, offen = offen.Count() }
                            on a.Id equals b.Id into tb
                           from b in tb.DefaultIfEmpty(new { Id = -1, offen = 0 })
                           join c in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Completed
                               && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                               && x.PokemonId != null
                             group x by x.PokemonId into completed
                             select new { Id = completed.Key.Value, completed = completed.Count() }
                            on a.Id equals c.Id into tc
                           from c in tc.DefaultIfEmpty(new { Id = -1, completed = 0 })
                           join d in
                             from x in db.WifiOffers
                             where x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.PokemonId != null
                             group x by x.PokemonId into gesamt
                             select new { Id = gesamt.Key.Value, gesamt = gesamt.Count() }
                            on a.Id equals d.Id into td
                           from d in td.DefaultIfEmpty(new { Id = -1, gesamt = 0 })
                           where b.offen > 0
                           orderby b.offen descending
                           select new { a.pokemon, b.offen, c.completed, sum = d.gesamt }).Take(10);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        // Items
        public ActionResult ItemTop10(DateTime from, DateTime until)
        {

            DateTime fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            DateTime untilDate = new DateTime(until.Year, until.Month, until.Day, 23, 59, 59);


            var results = (from a in
                               from k in
                                   from x in db.WifiOffers
                                   group x by x.ItemId into y
                                   select y.Key
                               from z in db.Items
                               where k == z.Id
                               select new { Id = k, item = z.Name.German }
                           join b in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Offer
                             && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.ItemId != null
                             group x by x.ItemId into offen
                             select new { Id = offen.Key.Value, offen = offen.Count() }
                            on a.Id equals b.Id into tb
                           from b in tb.DefaultIfEmpty(new { Id = -1, offen = 0 })
                           join c in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Completed
                               && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                               && x.ItemId != null
                             group x by x.ItemId into completed
                             select new { Id = completed.Key.Value, completed = completed.Count() }
                            on a.Id equals c.Id into tc
                           from c in tc.DefaultIfEmpty(new { Id = -1, completed = 0 })
                           join d in
                             from x in db.WifiOffers
                             where x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.ItemId != null
                             group x by x.ItemId into gesamt
                             select new { Id = gesamt.Key.Value, gesamt = gesamt.Count() }
                            on a.Id equals d.Id into td
                           from d in td.DefaultIfEmpty(new { Id = -1, gesamt = 0 })
                           orderby d.gesamt descending
                           select new { a.item, b.offen, c.completed, sum = d.gesamt }).Take(10);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemTop10Completed(DateTime from, DateTime until)
        {
            DateTime fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            DateTime untilDate = new DateTime(until.Year, until.Month, until.Day, 23, 59, 59);


            var results = (from a in
                               from k in
                                   from x in db.WifiOffers
                                   where x.ItemId != null
                                   group x by x.ItemId into y
                                   select y.Key.Value
                               from z in db.Items
                               where k == z.Id
                               select new { Id = k, item = z.Name.German }
                           join b in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Offer
                             && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.ItemId != null
                             group x by x.ItemId into offen
                             select new { Id = offen.Key.Value, offen = offen.Count() }
                            on a.Id equals b.Id into tb
                           from b in tb.DefaultIfEmpty(new { Id = -1, offen = 0 })
                           join c in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Completed
                               && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                               && x.ItemId != null
                             group x by x.ItemId into completed
                             select new { Id = completed.Key.Value, completed = completed.Count() }
                            on a.Id equals c.Id into tc
                           from c in tc.DefaultIfEmpty(new { Id = -1, completed = 0 })
                           join d in
                             from x in db.WifiOffers
                             where x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.ItemId != null
                             group x by x.ItemId into gesamt
                             select new { Id = gesamt.Key.Value, gesamt = gesamt.Count() }
                            on a.Id equals d.Id into td
                           from d in td.DefaultIfEmpty(new { Id = -1, gesamt = 0 })
                           where c.completed > 0
                           orderby c.completed descending
                           select new { a.item, b.offen, c.completed, sum = d.gesamt }).Take(10);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemTop10Open(DateTime from, DateTime until)
        {
            DateTime fromDate = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            DateTime untilDate = new DateTime(until.Year, until.Month, until.Day, 23, 59, 59);


            var results = (from a in
                               from k in
                                   from x in db.WifiOffers
                                   group x by x.ItemId into y
                                   select y.Key
                               from z in db.Items
                               where k == z.Id
                               select new { Id = k, item = z.Name.German }
                           join b in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Offer
                             && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.ItemId != null
                             group x by x.ItemId into offen
                             select new { Id = offen.Key.Value, offen = offen.Count() }
                            on a.Id equals b.Id into tb
                           from b in tb.DefaultIfEmpty(new { Id = -1, offen = 0 })
                           join c in
                             from x in db.WifiOffers
                             where x.Status == TradingStatus.Completed
                               && x.CreationDate >= fromDate && x.CreationDate <= untilDate
                               && x.ItemId != null
                             group x by x.ItemId into completed
                             select new { Id = completed.Key.Value, completed = completed.Count() }
                            on a.Id equals c.Id into tc
                           from c in tc.DefaultIfEmpty(new { Id = -1, completed = 0 })
                           join d in
                             from x in db.WifiOffers
                             where x.CreationDate >= fromDate && x.CreationDate <= untilDate
                             && x.ItemId != null
                             group x by x.ItemId into gesamt
                             select new { Id = gesamt.Key.Value, gesamt = gesamt.Count() }
                            on a.Id equals d.Id into td
                           from d in td.DefaultIfEmpty(new { Id = -1, gesamt = 0 })
                           where b.offen > 0
                           orderby b.offen descending
                           select new { a.item, b.offen, c.completed, sum = d.gesamt }).Take(10);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
