// copyright 2016 the pokefans authors. see copying.md for legal info
using System;
using System.Collections;
using System.Collections.Generic;
using Pokefans.Data;
using Pokefans.Data.Fanwork;

namespace Pokefans.Models
{
    public class MainPageViewModel
    {
        public Content StartTeaser { get; set; }

        public Content Recommendations { get; set; }

        public Content Carousel { get; set; }

        public List<Content> News { get; set; }

        public List<Fanart> Fanarts { get; set; }

        public Content Trivia { get; set; }

        public List<Content> LatestArticles { get; set; }
    }
}
