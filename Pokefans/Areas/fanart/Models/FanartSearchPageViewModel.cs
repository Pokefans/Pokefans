// Copyright 2016 the pokefans authors. See copying.md for legal info
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.fanart.Models
{
    public class FanartSearchPageViewModel
    {
        public List<FanartSearchViewModel> Results { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; }

        public int TotalResults { get; internal set; }

        public string SearchTerm { get; internal set; }
    }
}