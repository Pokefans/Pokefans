// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Pokefans.Data;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class ContentIncludesViewModel
    {
        public Content Content { get; set; }

        public IEnumerable<SelectListItem> AvailableBoilerplates { get; set; }

        [Required]
        public int ContentId { get; set; }

        [Required]
        [Display(Name = "Textbaustein")]
        public int BoilerplateId { get; set; }

        [Required]
        [Display(Name = "Platzierung")]
        [MaxLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9 _\-]+$", ErrorMessage = "Der Name darf nur alphanumerische Zeichen und Unterstriche enthalten!")]
        public string BoilerplateName { get; set; }

        public bool Saved { get; set; }
    }
}