// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserAdvertisingNoteViewModel
    {
        /// <summary>
        /// Gets or sets the advertising to add.
        /// </summary>
        /// <value>
        /// The advertising to add.
        /// </value>
        public UserAddAdvertisingViewModel Uaavm { get; set; }

        /// <summary>
        /// Gets or sets the Name of the advertising form.
        /// </summary>
        /// <value>
        /// The Name of advertising form.
        /// </value>
        public string AdvertisingForm { get; set; }
    }
}