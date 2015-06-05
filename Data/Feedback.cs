// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    /// <summary>
    /// self explanatory
    /// </summary>
    public enum FeedbackPriority { Normal, High, Low }

    /// <summary>
    /// self explanatory
    /// </summary>
    public enum FeedbackQuality { Undefined, Normal, High, Low }

    /// <summary>
    /// self explanatory
    /// </summary>
    public enum FeedbackStatus { Waiting, InProgress, Done, Rejected }

    /// <summary>
    /// 
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// Unique Id for the Feedback Object
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Feedback Priority
        /// </summary>
        [DefaultValue(FeedbackPriority.Normal)]
        public FeedbackPriority Priority { get; set; }

        /// <summary>
        /// Id of the Author
        /// </summary>
        [Required]
        public int AuthorId { get; set; }

        /// <summary>
        /// Ip-Address the Feedback was submitted from
        /// </summary>
        [Required]
        public string IpAddress { get; set; }

        /// <summary>
        /// Time the Feedback was submitted
        /// </summary>
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Time the Feedback was last edited
        /// </summary>
        public DateTime Edited { get; set; }

        /// <summary>
        /// Id of the User who edited the Feedback
        /// </summary>
        public int EditorId { get; set; }

        /// <summary>
        /// Note for the Feedback
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Feedback Quality
        /// </summary>
        [DefaultValue(FeedbackQuality.Undefined)]
        public FeedbackQuality Quality { get; set; }

        /// <summary>
        /// Feedback Status
        /// </summary>
        [DefaultValue(FeedbackStatus.Waiting)]
        public FeedbackStatus Status { get; set; }

        /// <summary>
        /// Author User
        /// </summary>
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        /// <summary>
        /// Editor User
        /// </summary>
        [ForeignKey("EditorId")]
        public virtual User Editor { get; set; }
    }
}
