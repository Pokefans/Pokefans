// Copyright 2015 the pokefans authors. See copying.md for legal info.
using System;

namespace Pokefans.Data.Wifi
{
    public class AuctionOffer : Offer
    {
        public DateTime AuctionEnd { get; set; }

        public int CurrentPrice { get; set; }

        public int Reserve { get; set; }
    }
}

