// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pokefans.Areas.mitarbeit.Models
{
    public class UserMenuModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the profile URL of the current user.
        /// </summary>
        /// <value>
        /// The profile URL of the current user.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the active menu element key.
        /// </summary>
        /// <value>
        /// The active menu element key.
        /// </value>
        /// <remarks> Maybe we should refactor this to an ENUM instead of a string key.</remarks>
        public string Active { get; set; }
    }
}