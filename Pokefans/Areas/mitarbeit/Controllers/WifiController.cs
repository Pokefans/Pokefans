using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChartJSCore.Models;
using Pokefans.Data;
using Pokefans.Data.Wifi;
using Pokefans.Util;

namespace Pokefans.Controllers
{
    [Authorize(Roles="wifi-moderator")]
    public class WifiController : Controller
    {

        Entities db;

        public WifiController(Entities ents)
        {
            db = ents;
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

        public ActionResult SelectOffer(int start = 0)
        {
            return View("~/Areas/mitarbeit/Views/Wifi/SelectOffer.cshtml");
        }

        public ActionResult Modify(int id)
        {
            return View("~/Areas/mitarbeit/Views/WiFi/Modify.cshtml");
        }

        #region Wifi Dashboard API

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

            foreach(var gen in generations)
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
