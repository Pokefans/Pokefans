// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class ContentUrlsViewModel
    {
        public Content Content { get; set; }

        public int ContentId { get; set; }

        public bool Saved { get; set; }

        [Display(Name = "Adresse")]
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_\-/]+", ErrorMessage = "Die URL darf nur alphanumerische Zeichen und Unter- sowie Bindestriche enthalten.")]
        public string UrlName { get; set; }

        [Display(Name = "Als Primär-Url setzen")]
        [DefaultValue(false)]
        public bool SetDefault { get; set; }

        public bool IsContentAdministrator { get; set; }
    }
}