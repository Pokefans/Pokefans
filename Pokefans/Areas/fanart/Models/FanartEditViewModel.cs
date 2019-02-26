// Copyright 2016 the pokefans authors. see copying.md for legal info.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.fanart.Models
{
    public class FanartEditViewModel
    {
        [Display(Name="Titel")]
        public string Title { get; set; }

        [Display(Name = "Tags")]
        public string Taglist { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name="Reihenfolge")]
        [Range(-1000,1000)]
        public int Order { get; set; }
    }
}