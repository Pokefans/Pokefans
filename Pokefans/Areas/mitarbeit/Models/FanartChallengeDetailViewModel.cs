// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Data.Fanwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class FanartChallengeDetailViewModel
    {
        public FanartChallenge Challenge { get; set; }

        public List<Fanart> Fanarts { get; set; }

        public List<FanartChallengeVote> Votes { get; set; }
    }
}