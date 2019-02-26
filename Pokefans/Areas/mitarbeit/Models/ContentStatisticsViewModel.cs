// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System.Collections.Generic;
using System.Linq;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class ContentStatisticsHostEntry
    {
        public string Host { get; set; }

        public int Count { get; set; }

        public IEnumerable<ContentStatisticsLinkEntry> Urls { get; set; }
    }

    public class ContentStatisticsLinkEntry
    {
        public string Url { get; set; }

        public int Count { get; set; }
    }

    public class ContentStatisticsViewModel
    {
        public Content Content { get; set; }

        public int ViewCount { get; set; }

        // TODO: Create own data structure
        public IEnumerable<ContentStatisticsHostEntry> TrackerSources { get; set; }
    }
}