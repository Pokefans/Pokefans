// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pokefans.Areas.mitarbeit.Controllers
{
    public class PokemonApiController : Controller
    {

        Entities db;

        public PokemonApiController(Entities ents)
        {
            db = ents;
        }

        public ActionResult Names(string query)
        {
            //TODO: Let sphinx handle this
            return Json(db.Pokemon.Where(g => g.Name.German.Contains(query) || g.Name.English.Contains(query)).Select(g => new { Id = g.Id, Name = g.Name }).Take(50).ToList());
        }
    }
}