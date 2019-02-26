// Copyright 2016 the pokefans authors. see copying.md for legal info.
using Pokefans.Data;
using Pokefans.Data.Fanwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.fanart.Models
{
    public class FanartIndexViewModel
    {
        public Content Teaser { get; set; }

        public List<Fanart> Fanarts { get; set; }
    }
}