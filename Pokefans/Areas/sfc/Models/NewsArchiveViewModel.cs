// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using Pokefans.Data;

namespace Pokefans.Areas.sfc.Models
{
    public class NewsArchiveViewModel
    {
        public List<Content> News { get; set; }

        public bool HasPrev { get; set; }

        public bool HasNext { get; set; }

        public long Prev { get; set; }

        public long Next { get; set; }
    }
}