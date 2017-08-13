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

namespace Pokefans.Controllers
{
    public class TradingController : Controller
    {
        Entities db;
        Cache cache;
        private Analyzer analyzer;
        private Searcher searcher;
        private IndexWriter writer;

        public TradingController(Entities ents, Cache c, IndexWriter wrtr, Searcher srchr, Analyzer ana)
        {
            db = ents;
            cache = c;
            analyzer = ana;
            searcher = srchr;
            writer = wrtr;
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

            return View(tivm);
        }

        [Authorize]
        public ActionResult Protocol(int start = 0) {
            int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            var query = db.TradeLogs.Include("Offer").Include("Offer.Pokemon").Include("Offer.Item").Include("UserTo").Where(x => x.UserFromId == uid);

            if(start > 0)
                query = query.Where(x => x.Id < start);

            var log = query.Take(50).ToList();

            return View(log);

        }

		[Authorize]
		public ActionResult My(int start = 0)
		{
			int uid = int.Parse(((ClaimsIdentity)HttpContext.User.Identity).GetUserId());

            //var query = db.TradeLogs.Include("Offer").Include("Offer.Pokemon").Include("Offer.Item").Include("UserTo").Where(x => x.UserFromId == uid);
            var query = db.WifiOffers.Include("Pokemon").Include("Item").Where(x => x.UserId == uid);

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

            return View(o);
        }
    }
}
