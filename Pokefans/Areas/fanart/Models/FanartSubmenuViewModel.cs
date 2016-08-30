// Copyright 2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.fanart.Models
{
    public class FanartSubmenuViewModel
    {
        public string ActiveMenuKey { get; set; }

        public string SearchTerm { get; set; }

        public FanartSubmenuViewModel(string menu, string term)
        {
            ActiveMenuKey = menu;
            SearchTerm = term;
        }
    }
}