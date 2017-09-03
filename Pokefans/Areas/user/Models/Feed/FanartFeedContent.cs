// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;
using Pokefans.Data.Fanwork;

namespace Pokefans.Areas.user.Models.Feed
{
    public class FanartFeedContent : IFeedContent
    {
        public FanartFeedContent()
        {
        }

        public DateTime Timestamp { get; set; }
        public string Username { get; set; }

        public Fanart Fanart { get; set; }
    }
}
