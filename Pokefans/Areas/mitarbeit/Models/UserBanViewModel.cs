// Copyright 2016-2019 the pokefans authors. See copying.md for legal info.
using Pokefans.Data;
using Pokefans.Data.Base;
using Pokefans.Data.Fanwork;
using Pokefans.Data.Wifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserBanViewModel
    {
        public User User { get; set; }
        public UserBan GlobalBan { get; set; }
        public FanartBanlist FanartBan { get; set; }
        public WifiBanlist WifiBan { get; set; }
    }
}