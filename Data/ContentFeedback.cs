// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data
{
    public enum FeedbackPriority { Normal, High, Low }

    public enum FeedbackQuality { Normal, High, Low }

    public enum FeedbackStatus { Waiting, InProgress, Done, Rejected }

    [Table("content_feedback")]
    public class ContentFeedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ContentId { get; set; }

        public string Feedback { get; set; }

        [DefaultValue(FeedbackPriority.Normal)]
        public FeedbackPriority Priority { get; set; }

        public int AuthorId { get; set; }

        public string IPAddress { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        public int EditorId { get; set; }

        public FeedbackQuality Quality { get; set; }

        [DefaultValue(FeedbackStatus.Waiting)]
        public FeedbackStatus Status { get; set; }

        public string Proposal { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        [ForeignKey("EditorId")]
        public virtual User Editor { get; set; }
    }
}
