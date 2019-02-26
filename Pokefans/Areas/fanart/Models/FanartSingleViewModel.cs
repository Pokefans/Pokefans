// Copyright 2016 the pokefans authors. See copying.md for legal info
using Pokefans.Data.Comments;
using Pokefans.Data.Fanwork;
using Pokefans.Models;
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
        
        public bool IsRatingActive { get; set; }

        public CommentsViewModel Comments { get; set; }

        public List<Fanart> Related { get; set; }
        public Dictionary<int, string> Categories { get; internal set; }
        public Dictionary<int, string> CategoriesName { get; internal set; }
    }
}