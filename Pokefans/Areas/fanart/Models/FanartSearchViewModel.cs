// Copyright 2016 the pokefans authors. See copying.md for legal info
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.fanart.Models
{
    public class FanartSearchViewModel
    {
        public int Id { get; set; }

        public string ThumbnailUrl { get; set; }

        public string DetailUrl { get; set; }
    }
}