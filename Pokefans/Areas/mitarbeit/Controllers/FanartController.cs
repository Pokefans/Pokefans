// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Data;
using Pokefans.Data.Fanwork;
using Pokefans.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//TODO: Add Anti Forgery Token to ajax calls!
namespace Pokefans.Areas.mitarbeit.Controllers
{
    [Authorize(Roles = "fanart-manager")]
    public class FanartController : Controller
    {
        Entities db;
        Cache cache;


        public FanartController(Entities ents, Cache c)
        {
            db = ents;
            cache = c;
        }

        // GET: mitarbeit/Fanart
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the category add/edit/delete screen. Add/edit/delete operations are done by AJAX.
        /// </summary>
        /// <returns>ActionResult</returns>
        [Authorize(Roles = "superadmin")]
        public ActionResult Categories()
        {
            var cats = from s in db.FanartCategories
                       orderby s.Order ascending
                       select s;

            return View(cats.ToList());
        }

        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "superadmin")]
        [HttpPost]
        public ActionResult AddCategory(string name)
        {
            if (!db.FanartCategories.Any(x => x.Name == name))
            {
                FanartCategory cat = new FanartCategory()
                {
                    Name = name,
                    Uri = name.ToLower().Replace(' ', '-'),
                    MaxFileSize = 1048576,
                    MaximumDimension = 2000,
                    Order = 100
                };

                db.FanartCategories.Add(cat);
                db.SaveChanges();

                refillCache();

                return Json(cat);
            }

            return Json(false);
        }

        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "superadmin")]
        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            if (!db.FanartCategories.Any(x => x.Id == id))
            {
                return Json(false);
            }

            db.FanartCategories.Remove(db.FanartCategories.FirstOrDefault(g => g.Id == id));
            db.SaveChanges();

            refillCache();

            return Json(true);
        }

        [Authorize(Roles = "superadmin")]
        [HttpPost]
        public ActionResult EditCategory(int pk, string name, string value)
        {
            FanartCategory c = db.FanartCategories.FirstOrDefault(x => x.Id == pk);

            if (c == null)
            {
                Response.StatusCode = 400;
                return Json(false);
            }

            int newSize;
            switch (name)
            {
                case "name":

                    c.Name = value;
                    c.Uri = value.ToLower().Replace(' ', '-');
                    break;
                case "size":
                    if (int.TryParse(value, out newSize))
                    {
                        c.MaxFileSize = newSize;
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return Json(false);
                    }
                    break;
                case "dimension":
                    if (int.TryParse(value, out newSize))
                    {
                        c.MaximumDimension = newSize;
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return Json(false);
                    }
                    break;
                case "order":
                    if (int.TryParse(value, out newSize))
                    {
                        c.Order = newSize;
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return Json(false);
                    }
                    break;
                default:
                    Response.StatusCode = 400;
                    return Json(false);
            }
            db.SetModified(c);
            db.SaveChanges();

            refillCache();

            return Json(c);

        }

        /// <summary>
        /// Updates the category cache. This is done synchronously; it is not worth the overhead making this some complicated
        /// threaded mess due to the very very very very limited occassions where this will be used.
        /// </summary>
        private void refillCache()
        {
            Dictionary<int, string> fanartCategories = new Dictionary<int, string>();

            var catlist = db.FanartCategories.ToList();
            catlist.ForEach(x => fanartCategories.Add(x.Id, x.Name));
            cache.Remove("FanartCategories");
            cache.Add("FanartCategories", fanartCategories);
        }
    }
}