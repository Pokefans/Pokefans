// Copyright 2015-2016 the pokefans authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.fanart.Models
{
    public class FanartUploadViewModel
    {
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public int License { get; set; }
    }
}