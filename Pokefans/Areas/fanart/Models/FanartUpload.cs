using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.fanart.Models
{
    public class FanartUpload
    {
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public int License { get; set; }
    }
}