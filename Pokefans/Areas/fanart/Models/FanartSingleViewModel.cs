using Pokefans.Data.Fanwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.fanart.Models
{
    public class FanartSingleViewModel
    {
        public Fanart Fanart { get; set; }

        public string prevUri { get; set; }

        public string nextUri { get; set; }
    }
}