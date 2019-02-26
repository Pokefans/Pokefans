// Copyright 2016 the pokefans authors. See copying.md for legal info
using Lucene.Net.Index;
using Lucene.Net.Search;
using Pokefans.Data;
using Pokefans.Data.Fanwork;
using Pokefans.Security;
using Pokefans.Util;
using Pokefans.Util.Search;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Pokefans.Areas.fanart.Controllers
{
    public class ApiController : Controller
    {
        ApplicationUserManager userManager;
        Entities db;
        IndexWriter writer;

        public ApiController(ApplicationUserManager um, Entities ents, IndexWriter wrt)
        {
            userManager = um;
            db = ents;
            writer = wrt;
        }
        // GET: fanart/Api
        [HttpPost]
        [Authorize]
        [Throttle(Name = "FanartRatingThrottle", Seconds = 10)]
        [AllowCors("fanart")]
        public ActionResult Rate(int fanartId, int rating)
        {
            User currentUser = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;

            if (db.FanartBanlist.Any(x => x.UserId == currentUser.Id && x.CanRate == false))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json("Du wurdest für die Bewertungsfunktion gesperrt. Wende dich an die Moderation.");
            }

            if (!db.Fanarts.Any(x => x.Id == fanartId))
            {
                return HttpNotFound();
            }

            if(db.FanartRatings.Any(x => x.FanartId == fanartId && x.RatingUserId == currentUser.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return Json("Du hast dieses Werk bereits bewertet.");
            }

            if (db.Fanarts.Any(x => x.Id == fanartId && x.UploadUserId == currentUser.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Aus Gründen der Fairness kannst du deine eigenen Werke nicht bewerten.");
            }

            if (rating < 1 || rating > 5)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Die Bewertung muss zwischen 1 und 5 liegen.");
            }

            FanartRating frating = new FanartRating()
            {
                FanartId = fanartId,
                Rating = rating,
                RatingTime = DateTime.Now,
                RatingUserId = currentUser.Id,
                RatingIp = HttpContext.Request.UserHostAddress
            };

            db.FanartRatings.Add(frating);
            db.SaveChanges();


            // this is pretty much a 1:1 port of the current rating system
            decimal finalRating = db.FanartRatings.Where(x => x.FanartId == fanartId).Average(x => x.Rating);
            int count = db.FanartRatings.Count(x => x.FanartId == fanartId);
            decimal bonus = 0;

            if (count > 3)
            {
                // exclude banned users from average if we have enough votes
                var banlist = db.FanartBanlist.Where(y => y.CanRate).Select(x => x.UserId).ToList();
                finalRating = db.FanartRatings.Where(x => x.FanartId == fanartId && !banlist.Contains(x.RatingUserId)).Average(x => x.Rating);
                int ncount = db.FanartRatings.Count(x => x.FanartId == fanartId && !banlist.Contains(x.RatingUserId));

                if (ncount > 2)
                {
                    decimal? ownerVote = null; // the owner's votes have slightly more weight.

                    int treshold = ncount > 3 ? (int)Math.Floor((decimal)(ncount / 4)) : 1;
                    int i = 0;
                    var ratings = db.FanartRatings.Where(x => x.FanartId == fanartId && !banlist.Contains(x.RatingUserId));
                    int acount = 0;
                    decimal sum = 0;

                    foreach (var srating in ratings)
                    {
                        i++;

                        // we effectively throw away the lower and upper 25% of the votes.
                        if (i > treshold && i <= (ncount - treshold))
                        {
                            acount++;
                            sum += srating.Rating;
                        }
                        if (srating.RatingUserId == int.Parse(ConfigurationManager.AppSettings["OwnerUserId"]))
                        {
                            ownerVote = srating.Rating;
                        }
                    }
                    finalRating = Math.Round((finalRating + sum / acount * 4) / 5, 2);
                    if (ownerVote.HasValue)
                    {
                        finalRating = Math.Round((finalRating * 9 + ownerVote.Value) / 10, 2);
                    }
                }
            }

            if (finalRating > 4.75M)
            {
                if (count >= 30) bonus = 0.10M;
                else if (count >= 20) bonus = 0.08M;
                else if (count >= 15) bonus = 0.06M;
                else if (count >= 10) bonus = 0.04M;
                else if (count >= 5) bonus = 0.02M;
                else if (count <= 3) bonus = -0.02M;
                else if (count <= 2) bonus = -0.03M;
                else if (count <= 1) bonus = -0.04M;

                if ((rating + bonus) > 5.04M)
                {
                    bonus = 5.04M - finalRating;
                }
            }

            Fanart f = db.Fanarts.Include("Tags").Include("Tags.Tag").Include("UploadUser").Include("Category").First(x => x.Id == fanartId);
            f.Rating = finalRating + bonus;
            f.RatingCount = count;

            db.SetModified(f);
            db.SaveChanges();

            // re-add the document to the search index
            BooleanQuery query = new BooleanQuery();
            query.Add(new BooleanClause(new TermQuery(new Term("type", "fanart")), Occur.MUST));
            query.Add(new BooleanClause(new TermQuery(new Term("Id", f.Id.ToString())), Occur.MUST));
            writer.DeleteDocuments(query);
            writer.Flush(false, false, true);
            writer.AddDocument(DocumentGenerator.Fanart(f));
            writer.Flush(false, true, false);

            return Json((float)f.Rating);
        }

        [HttpPost]
        [AllowCors("fanart")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "fanart-moderator")]
        public ActionResult Move(int fanartId, int targetCategoryId)
        {
            if(!db.FanartCategories.Any(x => x.Id == targetCategoryId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Zielkategorie nicht gefunden");
            }

            if(!db.Fanarts.Any(x => x.Id == fanartId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Zielfanart nicht gefunden");
            }

            Fanart f = db.Fanarts.Include("Tags").Include("Tags.Tag").Include("UploadUser").Include("Category").First(x => x.Id == fanartId);
            f.CategoryId = targetCategoryId;
            db.SetModified(f);
            db.SaveChanges();
            f = db.Fanarts.Include("Tags").Include("Tags.Tag").Include("UploadUser").Include("Category").First(x => x.Id == fanartId);

            // re-add the document to the search index
            BooleanQuery query = new BooleanQuery();
            query.Add(new BooleanClause(new TermQuery(new Term("type", "fanart")), Occur.MUST));
            query.Add(new BooleanClause(new TermQuery(new Term("Id", f.Id.ToString())), Occur.MUST));
            writer.DeleteDocuments(query);
            writer.Flush(false, false, true);
            writer.AddDocument(DocumentGenerator.Fanart(f));
            writer.Flush(false, true, false);

            return Json(true);
        }

        [HttpPost]
        [AllowCors("fanart")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "fanart-moderator")]
        public ActionResult Delete(int fanartId)
        {
            if (!db.Fanarts.Any(x => x.Id == fanartId))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Zielfanart nicht gefunden");
            }


            // delete the fanart, but keep comments and ratings.
            Fanart art = db.Fanarts.First(x => x.Id == fanartId);
            // delete images first
            System.IO.File.Delete(art.ImageDiskPath);
            if (art.Size.X > 130 || art.Size.Y > 130)
            {
                System.IO.File.Delete(art.SmallThumbnailDiskPath);
                System.IO.File.Delete(art.LargeThumbnailDiskPath);
            }
            db.Fanarts.Remove(art);
            db.SaveChanges();

            // re-add the document to the search index
            BooleanQuery query = new BooleanQuery();
            query.Add(new BooleanClause(new TermQuery(new Term("type", "fanart")), Occur.MUST));
            query.Add(new BooleanClause(new TermQuery(new Term("Id", art.Id.ToString())), Occur.MUST));
            writer.DeleteDocuments(query);
            writer.Flush(false, false, true);

            return Json(true);
        }
    }
}