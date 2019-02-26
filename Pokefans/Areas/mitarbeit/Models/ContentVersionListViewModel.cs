// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class ContentVersionListViewModel
    {
        public Content Content { get; set; }

        public IEnumerable<ContentVersion> Versions { get; set; }
    }
}