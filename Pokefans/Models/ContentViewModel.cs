// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.Collections.Generic;
using Pokefans.Data;

namespace Pokefans.Models
{
    public class ContentViewModel
    {
        public Content Content { get; set; }

        public IEnumerable<Content> RelatedNews { get; set; }

        public IEnumerable<Content> News { get; set; } 
    }
}