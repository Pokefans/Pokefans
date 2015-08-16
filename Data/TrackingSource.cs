// Copyright 2015 the pokefans-core authors. See copying.md for legal info.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Table("tracking_sources")]
    public class TrackingSource
    {
        /// <summary>
        /// Unique Id for the TrackingSource Object
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Target url
        /// </summary>
        [MaxLength(255)]
        public string TargetUrl { get; set; }

        /// <summary>
        /// Referral source url
        /// </summary>
        [MaxLength(255)]
        public string SourceUrl { get; set; }

        /// <summary>
        /// Referral source host
        /// </summary>
        [MaxLength(255)]
        public string SourceHost { get; set; }

        /// <summary>
        /// Search term that was used or null if the referral was no search
        /// </summary>
        [MaxLength(255)]
        public string SearchTerm { get; set; }

        /// <summary>
        /// Time the request was done
        /// </summary>
        [Required]
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// Request's ip adress
        /// </summary>
        [Required]
        [MaxLength(46)]
        public string IpAdress { get; set; }
    }
}
