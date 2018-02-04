// Copyright 2017 the pokefans authors. See copying.md for legal info.
using System;
using Pokefans.Data.Wifi;
namespace Pokefans.Areas.user.Models.Feed
{
    public class NewWifiOfferFeedContent : IFeedContent
    {
        public DateTime Timestamp { get; set; }
        public string Username { get; set; }
        public string Url { get; set; }

        public Offer Offer { get; set; }
    }
}
