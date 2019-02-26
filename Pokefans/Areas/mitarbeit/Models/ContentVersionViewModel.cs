// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class ContentVersionViewModel
    {
        public ContentVersion CurrentVersion { get; set; }

        public ContentVersion PreviousVersion { get; set; }

        public string ContentDiff { get; set; }

        public string StylesheetDiff { get; set; }
    }
}