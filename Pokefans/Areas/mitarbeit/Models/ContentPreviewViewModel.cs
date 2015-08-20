// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class ContentPreviewViewModel
    {
        public int ContentId { get; set; }

        public string Contents { get; set; }

        public string Title { get; set; }

        public string Teaser { get; set; }

        public string Stylesheet { get; set; }
    }
}