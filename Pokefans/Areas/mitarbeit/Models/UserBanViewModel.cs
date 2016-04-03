// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.Data;
using Pokefans.Data.Fanwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserBanViewModel
    {
        public User User { get; set; }
        public FanartBanlist FanartBan { get; set; }
    }
}