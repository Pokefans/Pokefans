using Pokefans.Data;
using Pokefans.Data.Wifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Models
{
    public class TradingIndexViewModel
    {
        public Content TeaserContent { get; set; }

        public List<Offer> Offers { get; set; }
    }
}