// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentFeedback : Feedback
    {
        /// <summary>
        /// Id of the corresponding Content Object
        /// </summary>
        [Required]
        public int ContentId { get; set; }

        /// <summary>
        /// Proposed Content Change
        /// </summary>
        public string Proposal { get; set; }

        /// <summary>
        /// Proposed Title Change
        /// </summary>
        public string ProposedTitle { get; set; }

        /// <summary>
        /// Proposed Stylesheet Change
        /// </summary>
        public string ProposedStylesheet { get; set; }

        /// <summary>
        /// Proposed Teaser Change
        /// </summary>
        public string ProposedTeaser { get; set; }

        /// <summary>
        /// Corresponding Content Object
        /// </summary>
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}
