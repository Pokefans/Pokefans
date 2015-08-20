// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data
{
    /// <summary>
    /// 
    /// </summary
    [Table("content_tracking")]
    public class ContentTrackingSource : TrackingSource
    {
        /// <summary>
        /// Id of the corresponding Content Object
        /// </summary>
        [Required]
        public int ContentId { get; set; }

        /// <summary>
        /// Corresponding Content Object
        /// </summary>
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}
