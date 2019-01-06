using System;
using Pokefans.Data.Wifi;

namespace Pokefans.Models
{
    public class OfferViewModel
    {
        public NormalOffer Offer { get; set; }
        public CommentsViewModel Comments { get; set; }

        public Interest Interest { get; set; }
    }
}
