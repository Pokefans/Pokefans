using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokefans.Data.UserData.Activities
{
    public class ContentFeedbackInsert : ContentActivity
    {
        public int FeedbackId { get; set; }

        [ForeignKey("FeedbackId")]
        public ContentFeedback Feedback { get; set; }
    }
}