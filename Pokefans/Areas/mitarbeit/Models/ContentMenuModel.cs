// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

namespace Pokefans.Areas.mitarbeit.Models
{
    public enum ContentMenuViewType
    {
        Index,

        Includes,

        Edit,

        Statistics,

        Versions,

        Urls,

        New
    }

    public class ContentMenuModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the active menu element key.
        /// </summary>
        /// <value>
        /// The active menu element key.
        /// </value>
        public ContentMenuViewType Active { get; set; }
    }
}